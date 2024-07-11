using Chipin_Rewrite.Models.Entities;
using Chipin_Rewrite.Utility.WooCommerce;
using Microsoft.AspNetCore.Mvc;

namespace Chipin_Rewrite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpPost("receive-cart-data")]
        public IActionResult ReceiveCartData([FromBody] List<WooCommerceCartInfo> products)
        {
            if (products == null || products.Count == 0)
            {
                return BadRequest("No products received.");
            }

            // Process the received cart data
            // For example, save to the database or perform some business logic
            foreach (var product in products)
            {
                // Your processing logic here
            }

            return Ok(new { message = "Cart data received successfully." });
        }
    }
}
