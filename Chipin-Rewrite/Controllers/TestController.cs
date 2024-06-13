﻿using Chipin_Rewrite.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace Chipin_Rewrite.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        private readonly Random _random = new Random();
        private readonly ChipinDbContext _context;

        public TestController(ChipinDbContext context)
        {
            _context = context;

        }
        public IActionResult Index(string data)
        {
            // Assuming the data is in JSON format
            if (string.IsNullOrEmpty(data))
            {
                return BadRequest("No JSON data provided.");
            }
            string decodedData = System.Net.WebUtility.UrlDecode(data);
            ExternalProduct testModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ExternalProduct>(decodedData);
            if (testModel == null)
            {
                return BadRequest("JSON data could not be deserialized.");
            }
            var test = new TestModel();
            Console.WriteLine(testModel);
            test.externalProduct = testModel;
            Console.WriteLine(test.externalProduct == null);
            // populate the ViewBag with all ProductListWalletId's as IEnumerable<SelectListItems>
            ViewBag.ProductListWalletId = new SelectList(_context.ProductListWallets, "ProductListWalletId", "ProductListWalletId", test.productListItem.ProductListWalletId);

            //store test so that it can be accessed again in add
            HttpContext.Session.SetObjectAsJson("test", test);





            return View(test);
        }
        [HttpPost]
        public async Task<IActionResult> Add(TestModel model)
        {
            ViewBag.SelectedProductListWalletId = model.productListItem.ProductListWalletId;
            TestModel test = HttpContext.Session.GetObjectFromJson<TestModel>("test");
            ViewBag.ProductName = test.externalProduct.ProductName;
            _context.Add(test.externalProduct);
            await _context.SaveChangesAsync();
            model.productListItem.ExternalProductId = test.externalProduct.ExternalProductId;
            model.productListItem.Quantity = 1;
            _context.Add(model.productListItem);

            await _context.SaveChangesAsync();
            // You can perform any additional logic here if needed.

            return View();
            //access test from session storage
            /*TestModel test = HttpContext.Session.GetObjectFromJson<TestModel>("test");
            Console.WriteLine(item.ProductListWalletId);
            
            await _context.SaveChangesAsync();
            //Console.WriteLine(test.externalProduct.ExternalProductId);
            */

            //return View();
        }








    }

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }


}