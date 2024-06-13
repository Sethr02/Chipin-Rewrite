using CallPayTest.Models;
using Chipin_Rewrite.Models.CallPay;
using Chipin_Rewrite.Models.Entities;
using Chipin_Rewrite.Models.Payfast;
using Chipin_Rewrite.Utility.CallPay;
using Chipin_Rewrite.Utility.Email;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;

namespace Chipin_Rewrite.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CallPayController : Controller
    {
        private readonly ChipinDbContext _context;
        private readonly ILogger<CallPayController> _logger;

        public CallPayController(ChipinDbContext context, ILogger<CallPayController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("GetKey")]
        [HttpPost]
        public async Task<IActionResult> GetKey([FromForm] CallPayPaymentParams paymentParams)
        {
            Console.WriteLine(paymentParams);
            Console.WriteLine("Amount: " + paymentParams.Amount);
            Console.WriteLine("Token: " + paymentParams.Token);

            CallPayAuthTokenGenerator authTokenGenerator = new CallPayAuthTokenGenerator();
            CallPayAuthParams authParams = authTokenGenerator.GenerateToken();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Auth-Token", authParams.Token);
                httpClient.DefaultRequestHeaders.Add("Org-Id", authParams.OrgId.ToString());
                httpClient.DefaultRequestHeaders.Add("Timestamp", authParams.Timestamp.ToString());

                string endpoint = "https://services.callpay.com/api/v2/payment-key";
                string amount = paymentParams.Amount.ToString();
                amount.Replace(",", "");
                string merchantRef = paymentParams.MerchantReference;

                var requestData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("amount", amount),
                    new KeyValuePair<string, string>("merchant_reference", merchantRef),
                    new KeyValuePair<string, string>("notify_url", "https://chipinrewritewebappservice.azurewebsites.net/CallPay/notify"),
                    new KeyValuePair<string, string>("success_url", $"https://chipinrewritewebappservice.azurewebsites.net/CallPay/success"),
                    new KeyValuePair<string, string>("payment_type", "credit_card"),
                });

                //Print all fields and values in requestData
                foreach (var field in requestData.GetType().GetProperties())
                {
                    Console.WriteLine(field.Name + ": " + field.GetValue(requestData, null));
                }

                HttpResponseMessage response = await httpClient.PostAsync(endpoint, requestData);
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        CallPayKeyResponse keyResponse = JsonConvert.DeserializeObject<CallPayKeyResponse>(responseString);

                        // Redirect to the URL from the response
                        return Redirect(keyResponse.Url);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return StatusCode(500); // Internal Server Error
                    }
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    return StatusCode((int)response.StatusCode);
                }
            }
        }

        [Route("notify")]
        [HttpPost]
        public async Task<IActionResult> Notify()
        {
            var data = Request.Form;

            foreach (var field in data)
            {
                _logger.LogInformation(field.Key + ": " + field.Value);
            }

            //Print data
            if (data == null)
            {
                Console.WriteLine("No data");
                return BadRequest();
            }
            else
            {
                try
                {
                    if (data["success"] == "0")
                    {
                        return Ok();
                    }

                    float amount = 0;
                    if (float.TryParse(data["amount"], NumberStyles.Float, CultureInfo.InvariantCulture, out float x))
                    {
                        // The 'amount' variable now contains the parsed float value
                        amount = x;
                    }
                    else
                    {
                        throw new Exception("Unable to parse the amount as a float. " + data["amount"]);
                        Console.WriteLine("Unable to parse the amount as a float.");
                    }

                    ProductListWalletTransaction transaction = new ProductListWalletTransaction();
                    transaction.Amount = amount;
                    transaction.FromInvitedUser = 0;
                    transaction.ProductListWalletId = int.Parse(data["id"]);
                    transaction.TransactionMethod = "CallPay-EFT";
                    transaction.TransactionId = data["gateway_reference"];
                    transaction.PaymentKey = data["payment_key"];
                    transaction.CreatedAt = DateTime.Parse(data["created"]);

                    foreach (var field in transaction.GetType().GetProperties())
                    {
                        Console.WriteLine(field.Name + ": " + field.GetValue(transaction, null));
                    }

                    //Add transaction to database
                    _context.ProductListWalletTransactions.Add(transaction);

                    //Update product list wallet funded field by amount
                    ProductListWallet productListWallet = _context.ProductListWallets.Find(transaction.ProductListWalletId);
                    productListWallet.Funded += transaction.Amount;

                    //find chipin id of product list wallet and find the tokenwallet that has that chipin id
                    string chipinId = productListWallet.ChipinId;
                    TokenWallet tokenWallet = _context.TokenWallets.Where(x => x.ChipinId == chipinId).FirstOrDefault();
                    tokenWallet.Amount += transaction.Amount;

                    await _context.SaveChangesAsync();

                    //If the product list wallet is fully funded, send email to list owner
                    if (productListWallet.Funded >= productListWallet.Total)
                    {
                        //Find UserTable of list owner using chipin id of product list wallet
                        string listOwner = productListWallet.ChipinId;
                        UserTable userTable = _context.UserTables.Where(x => x.ChipinId == listOwner).FirstOrDefault();

                        //Send email to list owner
                        //get email of list owner
                        string listOwnerEmail = userTable.UserEmail;
                        string listName = productListWallet.Name;
                        string listUrl = "https://chipinrewritewebappservice.azurewebsites.net/ShareLists/Details/" + productListWallet.ProductListWalletId;
                        if (listOwnerEmail != null)
                        {
                            Email email = new Email(listOwnerEmail);
                            email.ListFullyFunded(listName, listUrl);
                        }
                    }
                    else
                    {
                        //Send email to list owner
                        string listOwner = productListWallet.ChipinId;
                        UserTable userTable = _context.UserTables.Where(x => x.ChipinId == listOwner).FirstOrDefault();
                        string listOwnerEmail = userTable.UserEmail;
                        string listName = productListWallet.Name;
                        string listUrl = "https://chipinrewritewebappservice.azurewebsites.net/ShareLists/Details/" + productListWallet.ProductListWalletId;
                        if (listOwnerEmail != null)
                        {
                            Email email = new Email(listOwnerEmail);
                            email.ListPartiallyFunded(listName, listUrl, "test", "testMessage", "test@test.com");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return Ok();
            }
        }

        [Route("success")]
        [HttpPost]
        public async Task<IActionResult> Success([FromBody] object data)
        {
            Console.WriteLine(data);
            return Ok();
        }

        [Route("cancel")]
        [HttpPost]
        public async Task<IActionResult> Cancel([FromBody] object data)
        {
            Console.WriteLine(data);
            return Ok();
        }

        [Route("validatepayment")]
        [HttpPost]
        public async Task<IActionResult> ValidatePayment([FromForm] BankDetails bankDetails)
        {
            CallPayAuthTokenGenerator authTokenGenerator = new CallPayAuthTokenGenerator();
            CallPayAuthParams authParams = authTokenGenerator.GenerateToken();

            string endpoint = "https://eftsecure.callpay.com/api/v2/payout/validate-account";

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Auth-Token", authParams.Token);
                httpClient.DefaultRequestHeaders.Add("Org-Id", authParams.OrgId.ToString());
                httpClient.DefaultRequestHeaders.Add("Timestamp", authParams.Timestamp.ToString());

                var requestData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("transaction[bank]", bankDetails.BankName),
                    new KeyValuePair<string, string>("transaction[branch]", bankDetails.BranchCode),
                    new KeyValuePair<string, string>("transaction[account]", bankDetails.AccountNumber),
                    new KeyValuePair<string, string>("transaction[customer_name]", bankDetails.CustomerName)
                });

                HttpResponseMessage response = await httpClient.PostAsync(endpoint, requestData);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response: " + result);
                    return Ok(result);
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                    return Ok(response);
                }
            }
        }

        [Route("payout")]
        [HttpPost]
        public async Task<IActionResult> Payout()
        {
            CallPayAuthTokenGenerator authTokenGenerator = new CallPayAuthTokenGenerator();
            CallPayAuthParams authParams = authTokenGenerator.GenerateToken();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Auth-Token", authParams.Token);
                httpClient.DefaultRequestHeaders.Add("Org-Id", authParams.OrgId.ToString());
                httpClient.DefaultRequestHeaders.Add("Timestamp", authParams.Timestamp.ToString());

                string endpoint = "https://services.callpay.com/api/v2/payout/key";

                var requestData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("amount", "10"),
                    new KeyValuePair<string, string>("customer_name", "Seth Ramsamy"),
                    new KeyValuePair<string, string>("reference", "1039339203Ref"),
                    new KeyValuePair<string, string>("redirect_url", $"https://chipinrewritewebappservice.azurewebsites.net/ShareLists/Create"),
                    new KeyValuePair<string, string>("cancel_url", $"https://chipinrewritewebappservice.azurewebsites.net/ShareLists/Create")
                });

                //Print all fields and values in requestData
                foreach (var field in requestData.GetType().GetProperties())
                {
                    Console.WriteLine(field.Name + ": " + field.GetValue(requestData, null));
                }

                HttpResponseMessage response = await httpClient.PostAsync(endpoint, requestData);
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        CallPayPayoutWidgetParams keyResponse = JsonConvert.DeserializeObject<CallPayPayoutWidgetParams>(responseString);

                        // Redirect to the URL from the response
                        return Redirect(keyResponse.Url);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return StatusCode(500); // Internal Server Error
                    }
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    return StatusCode((int)response.StatusCode);
                }
            }
        }

        public async Task UpdateProductAmount(int productId, float newAmount)
        {
            using (var context = new ChipinDbContext())
            {
                var product = await context.ProductListWallets.FindAsync(productId);
                if (product != null)
                {
                    product.Funded += newAmount; // Assuming 'Amount' is the field you want to update
                    await context.SaveChangesAsync();
                    Console.WriteLine($"Updated product {productId} with new amount {newAmount}");
                }
                else
                {
                    Console.WriteLine($"Product {productId} not found");
                }
            }
        }
    }
}
