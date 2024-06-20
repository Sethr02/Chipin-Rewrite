using Chipin_Rewrite.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Chipin_Rewrite.Controllers
{
    public class CheckoutOptionsController : Controller
    {
        private readonly ILogger<CheckoutOptionsController> _logger;

        public CheckoutOptionsController(ILogger<CheckoutOptionsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return BadRequest("Data parameter is required.");
            }

            try
            {
                var decodedData = System.Net.WebUtility.UrlDecode(data);
                var checkoutData = JsonConvert.DeserializeObject<CheckoutData>(decodedData);

                // Calculate the cart total
                decimal cartTotal = checkoutData.CartInfo.Sum(item => item.Total);

                HttpContext.Session.SetInt32("OrderId", checkoutData.OrderId); // Assuming OrderId is part of checkoutData
                HttpContext.Session.SetString("CartTotal", cartTotal.ToString());

                ViewBag.CartTotal = cartTotal.ToString("C");
                ViewBag.CartTotalRaw = cartTotal;
                ViewBag.OrderId = checkoutData.OrderId;

                _logger.LogInformation("ViewBag.CartTotal");

                return View(checkoutData);
            }
            catch (JsonException)
            {
                return BadRequest("Invalid data format.");
            }
        }

    }
}
