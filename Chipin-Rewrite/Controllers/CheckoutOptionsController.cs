using Chipin_Rewrite.Models;
using Chipin_Rewrite.Models.Entities;
using Chipin_Rewrite.Utility.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mysqlx.Crud;
using Newtonsoft.Json;
using NuGet.Common;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using System.Web;

namespace Chipin_Rewrite.Controllers
{
    public class CheckoutOptionsController : Controller
    {
        private readonly ChipinDbContext _context;
        private readonly ILogger<CheckoutOptionsController> _logger;

        public CheckoutOptionsController(ChipinDbContext context, ILogger<CheckoutOptionsController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [Authorize]
        public IActionResult Index(string data)
        {
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            //var objectIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);

            if (ControllerContext.ActionDescriptor.Properties.TryGetValue("ViewMode", out var viewmode))
            {
                ViewBag.viewMode = viewmode;
            }

            List<UserTable> modelx = new List<UserTable>();
            if (objectIdClaim != null)
            {
                modelx = _context.UserTables.Where(wallet => wallet.ChipinId.Equals(objectIdClaim.Value)).ToList();
                string clientChipinId = objectIdClaim.Value;

                ViewBag.NavInfo = _context.TokenWallets.Where(xwallet => xwallet.ChipinId.Equals(clientChipinId)).ToList().FirstOrDefault().Amount;

                List<ProductListWallet> wallet = new List<ProductListWallet>();

                wallet = _context.ProductListWallets.Where(wallet => wallet.ChipinId.Equals(clientChipinId)).ToList();

                ViewBag.FilteredProductListItems = _context.ProductListItems
                    .Where(item => item.ProductListWallet.ChipinId.Equals(clientChipinId))
                    .ToList();

                ViewBag.Products = _context.Products.ToList();
                ViewBag.ExternalProducts = _context.ExternalProducts.ToList();
                ViewBag.ProductListWallets = wallet;
            }
            else
                if (objectIdClaim == null || _context.UserTables == null)
            {
                return NotFound();
            }

            ViewBag.UserTable = modelx;

            if (string.IsNullOrEmpty(data))
            {
                return BadRequest("Data parameter is required.");
            }

            try
            {
                var decodedData = System.Net.WebUtility.UrlDecode(data);
                var checkoutData = JsonConvert.DeserializeObject<CheckoutData>(decodedData);

                // Calculate the cart total
                decimal cartTotal = checkoutData.CartTotal;

                // Extract product IDs
                var productIds = checkoutData.CartInfo.Select(item => item.ProductId).ToList();

                int orderId = checkoutData.OrderId; // Assuming OrderId is part of checkoutData

                // Prepare data to be saved
                var dataToSave = new OrderData
                {
                    OrderId = orderId,
                    CartTotal = cartTotal,
                    BillingInfo = checkoutData.BillingInfo,
                    ProductIds = productIds
                };

                // Define the path and ensure the directory exists
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "secure");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string filePath = Path.Combine(directoryPath, $"{orderId}.txt");
                string json = JsonConvert.SerializeObject(dataToSave);
                System.IO.File.WriteAllText(filePath, json);

                HttpContext.Session.SetString("OrderId", checkoutData.OrderId.ToString());
                HttpContext.Session.SetString("CartTotal", cartTotal.ToString());

                var ci = new CultureInfo("en-ZA");
                var strResult = cartTotal.ToString("C", ci);

                ViewBag.CartTotal = strResult;
                ViewBag.CartTotalRaw = cartTotal.ToString("0.00"); ;
                ViewBag.OrderId = checkoutData.OrderId;

                _logger.LogInformation("ViewBag.CartTotal");

                string chipinId = objectIdClaim.Value;

                var tokenWallet = _context.TokenWallets
                .FirstOrDefault(tw => tw.ChipinId == chipinId);

                if (tokenWallet == null)
                {
                    return NotFound();
                }

                var userTable = _context.UserTables
                    .FirstOrDefault(u => u.ChipinId == tokenWallet.ChipinId);

                if (userTable == null)
                {
                    return NotFound();
                }

                ViewData["Amount"] = tokenWallet.Amount;
                ViewData["ChipinId"] = tokenWallet.ChipinId;

                return View(userTable);
            }
            catch (JsonException)
            {
                return BadRequest("Invalid data format.");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        // GET: Create
        [Authorize]
        [HttpGet]
        public IActionResult Create(string product_name, string product_description, float price, string image, string store, string cust_string1, int cust_int1, string product_id, int quantity)
        {
            if (string.IsNullOrEmpty(product_id))
            {
                // Retrieve data from session if not present in the query string
                string productDataJson = HttpContext.Session.GetString("ProductData");
                if (!string.IsNullOrEmpty(productDataJson))
                {
                    var productData = JsonConvert.DeserializeObject<ProductData>(productDataJson);

                    product_name = productData.ProductName;
                    product_description = productData.ProductDescription;
                    price = productData.Price;
                    image = productData.Image;
                    store = productData.Store;
                    cust_string1 = productData.CustString1;
                    cust_int1 = productData.CustInt1;
                    product_id = productData.ProductId;
                    quantity = productData.Quantity;
                }
                else
                {
                    return BadRequest("Product data is missing.");
                }
            }
            else
            {
                // Store data in session
                var productData = new ProductData
                {
                    ProductName = product_name,
                    ProductDescription = product_description,
                    Price = price,
                    Image = image,
                    Store = store,
                    CustString1 = cust_string1,
                    CustInt1 = cust_int1,
                    ProductId = product_id,
                    Quantity = quantity
                };
                HttpContext.Session.SetString("ProductData", JsonConvert.SerializeObject(productData));
            }

            ViewData["ProductName"] = product_name;
            ViewData["ProductDescription"] = product_description;
            ViewData["Price"] = price;
            ViewData["Image"] = image;
            ViewData["Store"] = store;
            ViewData["CustString1"] = cust_string1;
            ViewData["CustInt1"] = cust_int1;
            ViewData["ProductId"] = product_id;
            ViewData["Quantity"] = quantity;

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProductListWalletId,Name,EndAt")] ProductListWallet productListWallet, string product_name, string product_description, float price, string image, string store, string cust_string1, int cust_int1, string product_id, int quantity)
        {
            var objectIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (objectIdClaim == null)
            {
                return Unauthorized();
            }

            productListWallet.ChipinId = objectIdClaim.Value;
            productListWallet.Total = 0;
            productListWallet.Funded = 0;
            productListWallet.CreatedAt = DateTime.UtcNow;
            productListWallet.UpdatedAt = DateTime.UtcNow;
            productListWallet.Closed = 0;
            _context.Add(productListWallet);
            await _context.SaveChangesAsync();

            await AddAll(productListWallet.ProductListWalletId, product_name, product_description, price, image, store, cust_string1, cust_int1, product_id, quantity);

            if (productListWallet != null)
            {
                // Calculate the new total based on the product quantities
                productListWallet.Total = productListWallet.ProductListItems.Sum(item =>
                {
                    var product = _context.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                    return product?.Price * item.Quantity ?? 0;
                });

                productListWallet.Total += productListWallet.ProductListItems.Sum(item =>
                {
                    var exproduct = _context.ExternalProducts.FirstOrDefault(p => p.ExternalProductId == item.ExternalProductId);
                    return exproduct?.Price * item.Quantity ?? 0;
                });
                // Save changes to the ProductListWallet in the database
                await _context.SaveChangesAsync();
            }

            // Pass data to ViewBag for logging
            ViewBag.ProductListWallet = productListWallet;
            //ViewBag.ExternalProductId = externalProductId;

            return RedirectToAction("Index", "ProductListWallets");
        }

        private async Task AddAll(int productListWalletId, string product_name, string product_description, float price, string image, string store, string cust_string1, int cust_int1, string product_id, int quantity)
        {
            //int externalproductId = await AddExternalProducts(product_name, product_description, price, image, store, cust_string1, cust_int1, product_id, quantity);

            var externalProduct = new ExternalProduct
            {
                ProductName = product_name,
                ProductDescription = product_description,
                Price = price,
                Image = image,
                Store = store,
                CustString1 = cust_string1,
                CustInt1 = cust_int1,
                ProductId = product_id,
                Quantity = quantity
            };

            _context.ExternalProducts.Add(externalProduct);
            await _context.SaveChangesAsync();

            var productListItem = new ProductListItem
            {
                ExternalProductId = externalProduct.ExternalProductId,
                ProductListWalletId = productListWalletId,
                Quantity = 1
            };

            ViewBag.ExternalProductId = externalProduct.ExternalProductId;

            _context.Add(productListItem);
            await _context.SaveChangesAsync();
        }

        [Authorize]
        public async Task<IActionResult> List()
        {
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            //var objectIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);

            if (objectIdClaim != null)
            {
                string chipinId = objectIdClaim.Value;

                if (string.IsNullOrEmpty(chipinId))
                {
                    return BadRequest("ChipinId is required");
                }

                var lists = await _context.ProductListWallets
                    .Where(plw => plw.ChipinId == chipinId)
                    .ToListAsync();

                return View(lists);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddExternalProduct(ExternalProduct externalProduct, int productListWalletId)
        {
            _context.ExternalProducts.Add(externalProduct);
            await _context.SaveChangesAsync();

            var productListItem = new ProductListItem
            {
                ProductListWalletId = productListWalletId,
                ExternalProductId = externalProduct.ExternalProductId,
                Quantity = externalProduct.Quantity
                //ProductId = int.Parse(externalProduct.ProductId)
            };

            _context.ProductListItems.Add(productListItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "ProductListWallets");
        }

        [HttpGet]
        public IActionResult EnterListName()
        {
            //ViewBag.QueryParams = queryParams;
            return View();
        }

        [HttpGet]
        public IActionResult EnterDeliveryDate()
        {
            /*ViewBag.ListName = listName;
            ViewBag.QueryParams = queryParams;*/
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateList(string deliveryDate, string listName, ExternalProduct externalProduct)
        {
            var objectIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (objectIdClaim == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(listName) || string.IsNullOrEmpty(deliveryDate))
            {
                return BadRequest("List name and delivery date are required.");
            }

            // Create the new list
            var newList = new ProductListWallet
            {
                Name = listName,
                CreatedAt = DateTime.UtcNow,
                EndAt = DateTime.Parse(deliveryDate),
                ChipinId = objectIdClaim.Value // Set this appropriately
            };

            _context.ProductListWallets.Add(newList);
            await _context.SaveChangesAsync();

            // Create the external product
            _context.ExternalProducts.Add(externalProduct);
            await _context.SaveChangesAsync();

            // Create the product list item
            var newProductListItem = new ProductListItem
            {
                ProductListWalletId = newList.ProductListWalletId,
                ExternalProductId = externalProduct.ExternalProductId,
                Quantity = 1
            };

            _context.ProductListItems.Add(newProductListItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "ProductListWallets");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
