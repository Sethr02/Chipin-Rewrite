using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chipin_Rewrite.Controllers
{
    [Authorize]
    public class ExternalAdditionsController : Controller
    {

        public ActionResult Index(string token, string data, string cart)
        {
            ViewBag.data = data;
            ViewBag.cart = cart;
            ViewBag.token = token;

            return View();
        }


        [HttpPost]

        public async Task<ActionResult> AddToList(string token, string data, string cart)
        {
            using (var httpClient = new HttpClient())
            {
                Console.WriteLine("BUTTON WORKING");
                //Use this logic in ProductListWalletsController rather to avoid having to transport data between controller functions


            }

            return RedirectToAction("Index", "ProductListWallets", new { token = token, data = data, cart = cart });


        }

        public ActionResult PayWithWallet()
        {
            // Implement logic for "Pay with Wallet" here
            return View();
        }

    }
}
