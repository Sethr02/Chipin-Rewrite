using Chipin_Rewrite.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;
using Newtonsoft.Json;
using System.Text;

namespace Chipin_Rewrite.Controllers
{
    public class CallPaySuccessController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CallPaySuccessController(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            string OrderId = _httpContextAccessor.HttpContext.Session.GetString("OrderId");
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "secure");
            string filePath = Path.Combine(directoryPath, $"{OrderId}.txt");

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

            //await UpdateOrderStatus(OrderId, "processing");

            return View();
        }

        public async Task<IActionResult> UpdateOrderStatus(string orderId, string success)
        {
            var data = new
            {
                order_id = orderId,
                status = success
            };

            string json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync("https://bestwayshop.com/wp-json/chipin/v1/update-order-status", content);

            if (response.IsSuccessStatusCode)
            {
                // Log success or perform additional actions if needed
                return Ok("Order status updated successfully");
            }
            else
            {
                // Handle the error, log it, etc.
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Failed to update order status: {errorContent}");
            }
        }
    }
}
