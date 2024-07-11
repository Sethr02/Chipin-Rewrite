using CallPayTest.Models;
using Chipin_Rewrite.Configuration;
using Chipin_Rewrite.Models.CallPay;
using Chipin_Rewrite.Models.Entities;
using Chipin_Rewrite.Models.Payfast;
using Chipin_Rewrite.Services;
using Chipin_Rewrite.Utility.CallPay;
using Chipin_Rewrite.Utility.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;
using System.Net.Mail;
using System.Text;

namespace Chipin_Rewrite.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CallPayController : Controller
    {
        private readonly ChipinDbContext _context;
        private readonly ILogger<CallPayController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailService _emailSender;

        public CallPayController(IMailService emailSender, IHttpContextAccessor httpContextAccessor, ChipinDbContext context, ILogger<CallPayController> logger)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("GetKey")]
        [HttpPost]
        public async Task<IActionResult> GetKey([FromForm] CallPayPaymentSharedListParams paymentParams)
        {
            CallPayAuthTokenGenerator authTokenGenerator = new CallPayAuthTokenGenerator();
            CallPayAuthParams authParams = authTokenGenerator.GenerateToken();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Auth-Token", authParams.Token);
                httpClient.DefaultRequestHeaders.Add("Org-Id", authParams.OrgId.ToString());
                httpClient.DefaultRequestHeaders.Add("Timestamp", authParams.Timestamp.ToString());

                string endpoint = "https://services.callpay.com/api/v2/payment-key";

                var requestData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("amount", paymentParams.Amount.ToString()),
                    new KeyValuePair<string, string>("merchant_reference", paymentParams.MerchantReference),
                    new KeyValuePair<string, string>("notify_url", "https://chipinrewritewebappservice.azurewebsites.net/CallPay/notify?id=" + paymentParams.ProductListWalletId + "&payment_type=" + paymentParams.PaymentMethod + "&amount=" + paymentParams.Amount.ToString()),
                    new KeyValuePair<string, string>("success_url", $"https://chipinrewritewebappservice.azurewebsites.net/PaymentSelector/Success?id=" + paymentParams.ProductListWalletId + "&payment_type=" + paymentParams.PaymentMethod + "&amount=" + paymentParams.Amount),
                    new KeyValuePair<string, string>("payment_method", paymentParams.PaymentMethod) // Add payment method to the request data
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
                        //return Redirect(keyResponse.Url);
                        return Ok(new { url = "https://agent.callpay.com/pay/hosted?payment_key=" + keyResponse.Key + "&payment_type=" + paymentParams.PaymentMethod });
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

        [Route("GetKeyForSharedList")]
        [HttpPost]
        public async Task<IActionResult> GetKeyForSharedList([FromForm] CallPayPaymentSharedListParams paymentParams)
        {
            CallPayAuthTokenGenerator authTokenGenerator = new CallPayAuthTokenGenerator();
            CallPayAuthParams authParams = authTokenGenerator.GenerateToken();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Auth-Token", authParams.Token);
                httpClient.DefaultRequestHeaders.Add("Org-Id", authParams.OrgId.ToString());
                httpClient.DefaultRequestHeaders.Add("Timestamp", authParams.Timestamp.ToString());

                string endpoint = "https://services.callpay.com/api/v2/payment-key";

                var requestData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("amount", paymentParams.Amount.ToString()),
                    new KeyValuePair<string, string>("merchant_reference", paymentParams.MerchantReference),
                    new KeyValuePair<string, string>("notify_url", "https://chipinrewritewebappservice.azurewebsites.net/CallPay/notifysharedlist?id=" + paymentParams.ProductListWalletId + "&payment_type=" + paymentParams.PaymentMethod + "&amount=" + paymentParams.Amount.ToString()),
                    new KeyValuePair<string, string>("success_url", $"https://chipinrewritewebappservice.azurewebsites.net/sharedlist7?id=" + paymentParams.ProductListWalletId + "&payment_type=" + paymentParams.PaymentMethod + "&amount=" + paymentParams.Amount),
                    new KeyValuePair<string, string>("payment_method", paymentParams.PaymentMethod) // Add payment method to the request data
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
                        //return Redirect(keyResponse.Url);
                        return Ok(new { url = "https://agent.callpay.com/pay/hosted?payment_key=" + keyResponse.Key + "&payment_type=" + paymentParams.PaymentMethod });
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

        private string GenerateEmailBody(string recipientName, string contributionDate, string reference, string amount, string listName, string recipient)
        {
            return $@"
    <div style='font-family: Arial, sans-serif;'>
        <div style='border: 1px solid #ccc; padding: 16px;'>
            <img src='https://yourlogo.url/logo.png' alt='Chipin' style='width: 100px;'>
            <p>Hi, {recipientName}</p>
            <p>Thank you for contributing towards {listName}!</p>
            <div style='border-bottom: 1px solid #ccc; margin: 16px 0;'></div>
            <p>Contribution Date: {contributionDate}</p>
            <p>Reference: {reference}</p>
            <p>Contribution Amount: {amount}</p>
            <p>Recipient: {recipient}</p>
            <div style='border-bottom: 1px solid #ccc; margin: 16px 0;'></div>
            <p>Thank You for choosing us!</p>
            <p>Chipin Team</p>
            <div style='background-color: #f0f0f0; padding: 16px; margin-top: 16px;'>
                <p>Need help? Contact us at <a href='mailto:info@chipin.co.za'>info@chipin.co.za</a></p>
                <p><a href='#'>Privacy</a> | <a href='#'>Terms</a></p>
                <p>&copy; Chipin 2023</p>
            </div>
        </div>
    </div>";
        }

        [Route("notifysharedlist")]
        [HttpPost]
        public async Task<IActionResult> NotifySharedList(int id, string payment_type, decimal amount)
        {
            try
            {
                // Create Odoo email service instance
                var odooEmailService = new OdooEmailService(
                    odooUrl: "https://your-odoo-instance.com",
                    dbName: "your-db-name",
                    username: "your-username",
                    password: "your-password"
                );

                // Send email
                await odooEmailService.SendEmail(
                    to: "recipient@example.com",
                    subject: "Shared List Notification",
                    body: $"A shared list has been notified with ID: {id}, Payment Type: {payment_type}, Amount: {amount}"
                );

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500); // Internal Server Error
            }
        } 

        [Route("notify")]
        [HttpPost]
        public async Task<IActionResult> Notify()
        {
            //PaymentSelector/Success?id=113&success=true&status=complete&merchant_reference=Chipin&gateway_reference=C6CFEF30-362E-49BE-B0C9-C28A3DF78304&transaction_id=133333040&organisation_id=18122
            /*var productListWalletId = Request.Query["id"];
            var chipinAmount = Request.Query["amount"];
            var transactionId = Request.Query["transaction_id"];
            var paymentType = Request.Query["payment_type"];
            var gatewayRef = Request.Query["gateway_reference"];

            // Get chipin id by finding the matching productlistwallet
            ProductListWallet productListWallet = _context.ProductListWallets.Find(int.Parse(productListWalletId));
            string chipinId = productListWallet.ChipinId;

            if (_context.ProductListWalletTransactions.Any(t => t.ProductListWalletTransactionId == transactionId))
            {
                return Ok("Transaction already processed");
            }
            else
            {
                // Save the transaction as processed
                _context.ProductListWalletTransactions.Add(new ProductListWalletTransaction
                {
                    ProductListWalletTransactionId = int.Parse(transactionId),
                    Amount = float.Parse(chipinAmount),
                    ChipinId = chipinId,
                    FromInvitedUser = 0,
                    ProductListWalletId = int.Parse(productListWalletId),
                    TransactionMethod = paymentType,
                });

                await _context.SaveChangesAsync();

                // Update product list wallet funded field by amount
                productListWallet.Funded += float.Parse(chipinAmount);

                // Use chipin id of product list wallet and find the tokenwallet that has that chipin id
                TokenWallet tokenWallet = _context.TokenWallets.Where(x => x.ChipinId == chipinId).FirstOrDefault();
                tokenWallet.Amount += float.Parse(chipinAmount);

                await _context.SaveChangesAsync();

                return Ok("Transaction Processed Successfully!");
            }*/
            return Ok();
        }

        /*[Route("add-transaction")]
        [HttpGet]
        public async Task<IActionResult> AddTransaction(int productListWalletTransactionId, float amount, string chipinId, sbyte fromInvitedUser, int productListWalletId, string transactionMethod)
        {
            var newTransaction = new ProductListWalletTransaction
            {
                ProductListWalletTransactionId = productListWalletTransactionId,
                Amount = amount,
                ChipinId = chipinId,
                FromInvitedUser = fromInvitedUser,
                ProductListWalletId = productListWalletId,
                TransactionMethod = transactionMethod,
            };

            *//*var newTransaction = new ProductListWalletTransaction
            {
                ProductListWalletTransactionId = 133333040,
                Amount = 15,
                ChipinId = "cdcdcdcc",
                FromInvitedUser = 0,
                ProductListWalletId = 113,
                TransactionMethod = "credit_card",
            };*//*

            _context.ProductListWalletTransactions.Add(newTransaction);
            await _context.SaveChangesAsync();

            return Ok(newTransaction);
        }*/

        public IActionResult Success()
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "secure");
            string filePath = Path.Combine(directoryPath, "20239.txt");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Order data not found.");
            }

            string json = System.IO.File.ReadAllText(filePath);
            var orderData = JsonConvert.DeserializeObject<OrderData>(json);

            ViewBag.OrderId = orderData.OrderId;
            ViewBag.CartTotal = orderData.CartTotal;
            ViewBag.BillingInfo = orderData.BillingInfo;
            ViewBag.ProductIds = orderData.ProductIds;

            return View();
        }

        /*private async Task UpdateOrderStatus(int orderId, string status)
        {
            var data = new
            {
                order_id = orderId,
                status = status
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://yourwordpresssite.com/wp-json/chipin/v1/update-order-status", content);

            if (response.IsSuccessStatusCode)
            {
                // Log success or perform additional actions if needed
            }
            else
            {
                // Handle the error, log it, etc.
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to update order status: {errorContent}");
            }
        }*/

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
