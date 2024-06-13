using Chipin_Rewrite.Models.Entities;
using Chipin_Rewrite.Models.Payfast;
using Chipin_Rewrite.Utility.Email;
using Chipin_Rewrite.Utility.Payfast;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace PayfastTest.Controllers
{
    [Route("payfast")]
    [ApiController]
    public class PayfastController : ControllerBase
    {
        private readonly IPayFastService _payFastService;
        private readonly ChipinDbContext _context;
        private readonly ILogger<PayfastController> _logger;
        public PayfastController(IPayFastService payFastService, ChipinDbContext context, ILogger<PayfastController> logger)
        {
            _payFastService = payFastService;
            _context = context;
            _logger = logger;
        }

        [Route("getlink")]
        [HttpPost]
        public async Task<IActionResult> PaymentLink([FromForm] PayfastActivate payfastActivate)
        {
            _logger.LogInformation("ProductListWalletId: " + payfastActivate.ProductListWalletId);
            _logger.LogInformation("Amount: " + payfastActivate.Amount);

            var productListWallet = await _context.ProductListWallets.FindAsync(payfastActivate.ProductListWalletId);
            _logger.LogInformation("ProductListWallet: " + productListWallet);
            string name = productListWallet.Name;


            var payfastPayload = new PayfastPayload
            {
                ItemName = name,
                ItemDescription = "description",
                CustomInt1 = payfastActivate.ProductListWalletId,
                Amount = payfastActivate.Amount,
                ReturnUrl = "https://chipin-rewrite20231212122214.azurewebsites.net/ShareLists/Create",
                CancelUrl = "https://chipin-rewrite20231212122214.azurewebsites.net/NonFunctionalPages/PurchaseFailure",
                NotifyUrl = "https://chipin-rewrite20231212122214.azurewebsites.net/payfast/notify"
            };

            //https://chipin-rewrite20231212122214.azurewebsites.net/

            string link = await _payFastService.GetPayFastPaymentLink(payfastPayload);
            //If link is null, return bad request
            if (link == null)
            {
                return BadRequest();
            }
            else
            {
                //Redirect to link
                string linkString = link.ToString();
                _logger.LogInformation("\n\nLink: " + linkString + "\n\n");
                return Redirect(linkString);
            }


        }

        [Route("notify")]
        [HttpPost]
        public async Task<IActionResult> Notify()
        {
            //print all fields in the request
            var data = Request.Form;
            foreach (var item in data)
            {
                _logger.LogInformation(item.Key + ": " + item.Value);
            }

            if (data == null)
            {
                return BadRequest();
            }


            // parse data[amount_gross] to float
            float amount = float.Parse(data["amount_gross"], CultureInfo.InvariantCulture.NumberFormat);



            if (data["payment_status"] != "COMPLETE")
            {
                throw new Exception("Payment status is not complete. " + data["payment_status"]);
            }

            ProductListWalletTransaction transaction = new ProductListWalletTransaction();
            transaction.Amount = amount;
            transaction.FromInvitedUser = 0;
            transaction.ProductListWalletId = int.Parse(data["custom_int1"]);
            transaction.TransactionMethod = "Payfast";
            transaction.TransactionId = data["pf_payment_id"];
            transaction.CreatedAt = DateTime.Now;

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
                string listUrl = "https://chipin-rewrite20231212122214.azurewebsites.net/ShareLists/Details/" + productListWallet.ProductListWalletId;
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
                string listUrl = "https://chipin-rewrite20231212122214.azurewebsites.net/ShareLists/Details/" + productListWallet.ProductListWalletId;
                if (listOwnerEmail != null)
                {
                    Email email = new Email(listOwnerEmail);
                    email.ListPartiallyFunded(listName, listUrl, "test", "testMessage", "test@test.com");
                }
            }

            return Ok();

        }







        [Route("success")]
        [HttpPost]
        public async Task<string> Success()
        {
            var data = Request.Form;
            string dataString = data.ToString();
            Console.WriteLine(data);
            return dataString;
        }

        [Route("cancel")]
        [HttpPost]
        public async Task<string> Cancel()
        {
            var data = Request.Form;
            string dataString = data.ToString();
            Console.WriteLine(data);
            return dataString;
        }

    }
}
