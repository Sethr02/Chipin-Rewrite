using Chipin_Rewrite.Models.CallPay;
using Chipin_Rewrite.Models.Entities;
using Chipin_Rewrite.Utility.CallPay;
using Chipin_Rewrite.Utility.ThirdPartyReturns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Chipin_Rewrite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnWebhookController : ControllerBase
    {
        private readonly IExternalProductReturns _externalProductReturns;
        private readonly ICallPayService _callPayPayout;
        private readonly ILogger<ReturnWebhookController> _logger;
        private readonly ChipinDbContext _context;
        public ReturnWebhookController(IExternalProductReturns externalProductReturns, ICallPayService callPayPayout, ChipinDbContext context, ILogger<ReturnWebhookController> logger)
        {
            _externalProductReturns = externalProductReturns;
            _callPayPayout = callPayPayout;
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SendReturnProducts([FromForm] int productListWalletId)
        {
            Console.WriteLine("WE'RE HERE");

            //TODO: Impliment this with actual webhooks to multiple sites to test if it works
            ProductListWallet productListWallet = await _context.ProductListWallets.FirstAsync(x => x.ProductListWalletId == productListWalletId);


            //Validate Bank Account
            //Validate Payment
            BankDetails bankDetails = new BankDetails
            {
                BankName = "ABSA",
                BranchCode = "632005",
                AccountNumber = "9325529261",
                CustomerName = "Alfred Rademan"
            };
            HttpResponseMessage result = await _callPayPayout.ValidatePayment(bankDetails);
            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                JObject resultObject = JObject.Parse(resultContent);
                foreach (var item in resultObject)
                {
                    _logger.LogInformation($"{item.Key} : {item.Value}");
                }
                string status = resultObject["success"].ToString();

                //Lamda expression for if status is True than bool success is true
                bool success = status == "True";

                if (success)
                {
                    //Do Payout
                    CallPayPayoutParams payoutParams = new CallPayPayoutParams
                    {
                        BankDetails = bankDetails,
                        Amount = (float)productListWallet.Total,
                        Reference = $"Chipin"
                    };
                    HttpResponseMessage payoutResult = await _callPayPayout.Payout(payoutParams);
                    if (payoutResult.IsSuccessStatusCode)
                    {
                        string payoutResultContent = await payoutResult.Content.ReadAsStringAsync();
                        JObject payoutResultObject = JObject.Parse(payoutResultContent);
                        _logger.LogInformation($"Payout Result : {payoutResultObject}");

                        string payoutStatus = payoutResultObject["success"].ToString();
                        bool sentToAll = await _externalProductReturns.SendReturnModel(productListWallet);
                        _logger.LogInformation($"sentToAll : {sentToAll}");
                        bool payoutSuccess = payoutStatus == "True";
                        _logger.LogInformation($"payoutSuccess : {payoutSuccess}");
                        if (payoutSuccess && sentToAll)
                        {
                            _logger.LogInformation("Payout Success");
                            //Get token wallet using chipin address from productListWallet
                            TokenWallet tokenWallet = await _context.TokenWallets.FirstAsync(x => x.ChipinId == productListWallet.ChipinId);
                            tokenWallet.Amount -= productListWallet.Total;
                            productListWallet.Closed = 1;
                            _context.ProductListWallets.Update(productListWallet);
                            _context.TokenWallets.Update(tokenWallet);
                            await _context.SaveChangesAsync();
                            return RedirectToAction("PurchaseSuccess", "NonFunctionalPages");
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }





            //Do Payout

            /* if (sentToAll)
             {
                 //Get token wallet using chipin address from productListWallet
                 TokenWallet tokenWallet = await _context.TokenWallets.FirstAsync(x => x.ChipinId == productListWallet.ChipinId);
                 tokenWallet.Amount -= productListWallet.Total;
                 productListWallet.Closed = 1;
                 _context.ProductListWallets.Update(productListWallet);
                 _context.TokenWallets.Update(tokenWallet);
                 await _context.SaveChangesAsync();
                 return RedirectToAction("PurchaseSuccess", "NonFunctionalPages");
             }
             else
             {
                 return BadRequest();
             }*/



        }
    }
}
