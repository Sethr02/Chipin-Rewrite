using Chipin_Rewrite.Models.Entities;
using Chipin_Rewrite.Models.SignedModels;
using Chipin_Rewrite.Utility.Email;
using Chipin_Rewrite.Utility.Signatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace Chipin_Rewrite.Controllers
{

    public class ProductListWalletsController : Controller
    {
        private readonly ChipinDbContext _context;
        private readonly ISignatureGenerator _signatureGenerator;
        private readonly ILogger<ProductListWalletsController> _logger;
        public ProductListWalletsController(ChipinDbContext context, ISignatureGenerator signatureGenerator, ILogger<ProductListWalletsController> logger)
        {
            _context = context;
            _signatureGenerator = signatureGenerator;
            _logger = logger;
        }

        // GET: ProductListWallets
        [Authorize]
        public async Task<IActionResult> Index(string? data, string? token, string? cart)
        {
            _logger.LogInformation("\n\n\n Index called \n\n\n");
            //if (ControllerContext.ActionDescriptor.Properties.TryGetValue("ViewMode", out var viewmode))
            //{
            //    ViewBag.viewMode = viewmode;
            //}
            //if (ViewBag.viewMode != "LoggedIn")
            //{
            //    return RedirectToAction("Index", "UserTables");
            //}

            ViewBag.ExternalAddition = false;
            Console.WriteLine("Data: " + data);
            using (var httpClient = new HttpClient())
            {
                //Get user's id if logged in
                var objectId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

                if (!string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(cart))
                {
                    ReturnChipinId returnChipin = new ReturnChipinId();
                    returnChipin.ChipinId = objectId.Value;
                    returnChipin.Signature = "TestSignature";
                    returnChipin.Cart = cart;
                    var jsonReturnChipin = JsonConvert.SerializeObject(returnChipin);
                    var jsonReturnChipinString = new StringContent(jsonReturnChipin, Encoding.UTF8, "application/json");
                    var externalProductData = await httpClient.PostAsync(data, jsonReturnChipinString);

                    if (!externalProductData.IsSuccessStatusCode)
                    {
                        return NotFound();
                    }

                    string externalProducts = await externalProductData.Content.ReadAsStringAsync();
                    //JObject externalProductInformation = JObject.Parse(externalProducts);
                    Console.WriteLine(externalProducts);
                    try
                    {
                        //ExternalProductInformation externalProductInformation = JsonConvert.DeserializeObject<ExternalProductInformation>(externalProducts);
                        ViewBag.ExternalProductInformation = externalProducts;
                        ViewBag.ExternalAddition = true;
                        // Console.WriteLine(externalProductInformation.Products[0].ProductName);

                    }
                    catch (Exception e)
                    {
                        ViewBag.ExternalAddition = false;
                        Console.WriteLine(e);
                    }
                }
            }

            //Get user's id if logged in
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
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

            ViewBag.Token = token;

            return View();
        }


        [AllowAnonymous]
        // GET: ProductListWallets/Details/5
        public async Task<IActionResult> Details(int? id, string? token)
        {
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            string clientChipinId = objectIdClaim.Value;
            ViewBag.NavInfo = _context.TokenWallets.Where(xwallet => xwallet.ChipinId.Equals(clientChipinId)).ToList().FirstOrDefault().Amount;


            if (id == null || _context.ProductListWallets == null)
            {

                return NotFound();
            }

            var productListWallet = await _context.ProductListWallets
                .Include(p => p.Chipin)
                .FirstOrDefaultAsync(m => m.ProductListWalletId == id);
            if (productListWallet == null)
            {
                await Console.Out.WriteLineAsync("ppppppp");

                return NotFound();
            }



            Console.WriteLine("HEREHEREHERE");
            ViewBag.Token = token;
            ViewBag.Products = _context.Products.ToList();
            ViewBag.ProductListItems = _context.ProductListItems.Where(wallet => wallet.ProductListWalletId == id).ToList();
            ViewBag.Wallets = productListWallet;
            ViewBag.ExternalProducts = _context.ExternalProducts.ToList();

            //Check if user is logged in and that the chipin belongs to them
            if (objectIdClaim != null)
            {
                if (productListWallet.ChipinId.Equals(objectIdClaim.Value))
                {
                    ViewBag.Addresses = _context.Addresses.Where(wallet => wallet.ChipinId.Equals(objectIdClaim.Value)).ToList();

                }
                else
                {
                    ViewBag.Addresses = null;
                }

            }
            else
            {
                ViewBag.Addresses = null;
            }

            return View(productListWallet);
        }

        [Authorize]
        [HttpPost]

        public async Task<IActionResult> NewListAddition(string? token, string? externalProductInformation)
        {
            Console.WriteLine("ExternalProductInformation: " + externalProductInformation);
            if (string.IsNullOrEmpty(externalProductInformation))
            {
                ViewBag.ExternalAddition = false;
            }
            else
            {
                ViewBag.ExternalAddition = true;
                ViewBag.ExternalProductInformation = externalProductInformation;
            }

            //get user's id if logged in
            string? chipinId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            //parse object id to string



            List<ProductListWallet> modelx = new List<ProductListWallet>();
            if (chipinId != null)
            {
                modelx = _context.ProductListWallets.Where(wallet => wallet.ChipinId == chipinId).ToList();
            }

            ViewBag.ProductListWallets = modelx;

            return View("Create");
        }


        [Authorize]
        [HttpPost]

        public async Task<IActionResult> Link(string? path, int? id, string? token)
        {
            await Console.Out.WriteLineAsync(path + " " + id + " " + token);
            if (path.Equals("Index"))
            {
                await Index("", token, "");
            }
            else if (path.Equals("Create"))
            {
                return View(path);
            }
            else if (path.Equals("Details"))
            {
                await Details(id, token);
            }
            else if (path.Equals("Edit"))
            {
                await Edit(id, token);
            }
            else if (path.Equals("Delete"))
            {
                await Delete(id, token);
                return View("Index");
            }
            else if (path.Equals("RedirectToProductDetails"))
            {
                return RedirectToAction("Details", "Products");
            }
            else if (path.Equals("RedirectToExternalProductDetails"))
            {
                return RedirectToAction("ExternalDetails", "Products");
            }
            return View(path); ;
        }

        // GET: ProductListWallets/Create
        [Authorize]
        public IActionResult Create(string? token, string? externalProductInformation)
        {
            Console.WriteLine("ExternalProductInformation: " + externalProductInformation);
            if (string.IsNullOrEmpty(externalProductInformation))
            {
                ViewBag.ExternalAddition = false;
            }
            else
            {
                ViewBag.ExternalAddition = true;
                ViewBag.ExternalProductInformation = externalProductInformation;
            }
            //if (ControllerContext.ActionDescriptor.Properties.TryGetValue("ViewMode", out var viewmode))
            //{
            //    ViewBag.viewMode = viewmode;
            //}
            //if (ViewBag.viewMode != "LoggedIn")
            //{
            //    return RedirectToAction("Index", "UserTables");
            //}

            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            List<ProductListWallet> modelx = new List<ProductListWallet>();
            //if (ControllerContext.ActionDescriptor.Properties.TryGetValue("chipinId", out var val))
            //{
            modelx = _context.ProductListWallets.Where(wallet => wallet.ChipinId.Equals(objectIdClaim.Value)).ToList();
            //}
            Console.WriteLine("This is  craxy : " + objectIdClaim.Value + "    capacity:  " + modelx.Capacity);
            ViewBag.ProductListWallets = modelx;

            return View();
        }

        // POST: ProductListWallets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProductListWalletId,Name,EndAt")] ProductListWallet productListWallet, string? token, string? externalProductInformation)
        {

            //if (ControllerContext.ActionDescriptor.Properties.TryGetValue("ViewMode", out var viewmode))
            //{
            //    ViewBag.viewMode = viewmode;
            //}
            //if (ViewBag.viewMode != "LoggedIn")
            //{
            //    return RedirectToAction("Index", "UserTables");
            //}


            //if (ControllerContext.ActionDescriptor.Properties.TryGetValue("chipinId", out var id))
            //{
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            await Console.Out.WriteLineAsync("my id: " + objectIdClaim.Value);
            productListWallet.ChipinId = objectIdClaim.Value;
            productListWallet.Total = 0;
            productListWallet.Funded = 0;
            productListWallet.CreatedAt = DateTime.UtcNow;
            productListWallet.UpdatedAt = DateTime.UtcNow;
            productListWallet.Closed = 0;
            _context.Add(productListWallet);
            await _context.SaveChangesAsync();
            if (!string.IsNullOrEmpty(externalProductInformation))
            {
                externalProductInformation = HttpUtility.HtmlDecode(externalProductInformation);
                KeyGenerator keyGenerator = new KeyGenerator();
                keyGenerator.GenerateKeys();
                Console.WriteLine("ExternalProductInformation: asdasdasdasdasdasdasd \n\n" + externalProductInformation);
                await AddExternalProducts(externalProductInformation, productListWallet.ProductListWalletId, token);

            }
            //Find email from chipinId
            var chipin = await _context.UserTables.FindAsync(objectIdClaim.Value);
            string recipientEmail = chipin.UserEmail;
            Email email = new Email(recipientEmail);
            bool sent = email.ListCreated(productListWallet.Name, "https://chipinrewritewebappservice.azurewebsites.net/ProductListWallets/Details?id=" + productListWallet.ProductListWalletId);
            //return await Link("Index", 0, token);
            return RedirectToAction("Index");

            //}
        }

        // GET: ProductListWallets/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id, string? token)
        {
            if (id == null || _context.ProductListWallets == null)
            {

                return NotFound();
            }

            var productListWallet = await _context.ProductListWallets.FindAsync(id);
            if (productListWallet == null)
            {

                return NotFound();
            }

            return View(productListWallet);
        }

        // POST: ProductListWallets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]

        public async Task<IActionResult> Edit(int id, [Bind("Name, EndAt")] ProductListWallet productListWallet, string? token)
        {



            var plw = await _context.ProductListWallets.FindAsync(id);
            if (plw == null)
            {
                return NotFound();
            }


            try
            {

                plw.Name = productListWallet.Name;
                plw.EndAt = productListWallet.EndAt;

                _context.Update(plw);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductListWalletExists(id))
                {

                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Details", new { id = id });




            return View(productListWallet);
        }

        // GET: ProductListWallets/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id, string? token)
        {
            if (id == null || _context.ProductListWallets == null)
            {
                return NotFound();
            }

            var productListWallet = await _context.ProductListWallets
                .Include(p => p.Chipin)
                .FirstOrDefaultAsync(m => m.ProductListWalletId == id);
            if (productListWallet == null)
            {
                return NotFound();
            }

            return await DeleteConfirmed(id.Value, token);
        }


        // POST: ProductListWallets/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id, string? token)
        {
            if (_context.ProductListWallets == null)
            {
                return Problem("Entity set 'ChipinDbContext.ProductListWallets'  is null.");
            }
            var productListWallet = await _context.ProductListWallets.FindAsync(id);
            if (productListWallet != null)
            {
                _context.ProductListWallets.Remove(productListWallet);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { token });
        }

        private bool ProductListWalletExists(int id)
        {
            return (_context.ProductListWallets?.Any(e => e.ProductListWalletId == id)).GetValueOrDefault();
        }

        [Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]

        public async Task<IActionResult> AddExternalProducts(string externalProductInformation, int productListWalletId, string? token)
        {

            ExternalProductInformation externalProducts = JsonConvert.DeserializeObject<ExternalProductInformation>(externalProductInformation);

            //TODO Flesh out signature validation


            string externalSignature = externalProducts.Signature;
            /*bool verified = _signatureGenerator.validateSignature(externalProductInformation, externalSignature);
            if (!verified)
            {
                return BadRequest("Signature is not valid");
            }*/

            foreach (ExternalProduct product in externalProducts.Products)
            {
                //Add Product to the context
                _context.Add(product);
                await _context.SaveChangesAsync();


                // Create a new ProductListItem and add it to the context
                ProductListItem productListItem = new ProductListItem
                {
                    ExternalProductId = product.ExternalProductId,
                    ProductListWalletId = productListWalletId,
                    Quantity = product.Quantity
                };
                _context.Add(productListItem);
                await _context.SaveChangesAsync();

                // Retrieve the relevant ProductListWallet
                var productListWallet = await _context.ProductListWallets
                    .Include(w => w.ProductListItems) // Include related ProductListItems
                    .FirstOrDefaultAsync(w => w.ProductListWalletId == productListItem.ProductListWalletId);

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
            }

            return RedirectToAction("Details", "ProductListWallets", new { id = productListWalletId, token });
        }

        // POST: ProductListItems1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInternalProduct([Bind("ChipinProductListEntryId,ProductId,Quantity,ProductListWalletId,ExternalProductId")] ProductListItem productListItem, string? token)
        {
            // Check if a similar product list item exists for the same ProductId and ProductListWalletId
            var existingItem = await _context.ProductListItems
                .FirstOrDefaultAsync(p => p.ProductId == productListItem.ProductId && p.ProductListWalletId == productListItem.ProductListWalletId);

            if (existingItem != null)
            {
                // If the item already exists, update the quantity
                existingItem.Quantity = productListItem.Quantity;
            }
            else
            {
                // If the item does not exist, add it to the context
                _context.Add(productListItem);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Retrieve the relevant ProductListWallet
            var productListWallet = await _context.ProductListWallets
                .Include(w => w.ProductListItems) // Include related ProductListItems
                .FirstOrDefaultAsync(w => w.ProductListWalletId == productListItem.ProductListWalletId);

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

            // Update other properties accordingly

            // Save changes to the ProductListWallet in the database
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = productListItem.ProductListWalletId, token });


            return View(productListItem);
        }

        // GET: ProductListItems1/DeleteFromList/5
        [Authorize]
        public async Task<IActionResult> DeleteFromList(int? id, string? token)
        {
            if (id == null || _context.ProductListItems == null)
            {
                await Console.Out.WriteLineAsync("dkdkdkdk " + id);
                return NotFound();
            }

            var productListItem = await _context.ProductListItems
                .Include(p => p.ExternalProduct)
                .Include(p => p.Product)
                .Include(p => p.ProductListWallet)
                .FirstOrDefaultAsync(m => m.ChipinProductListEntryId == id);
            if (productListItem == null)
            {
                return NotFound();
            }

            // Call the DeleteConfirmed method directly to perform deletion
            return await DeleteFromListConfirmed(productListItem.ChipinProductListEntryId, token);
        }

        [Authorize]
        [HttpPost, ActionName("DeleteFromList")]
        public async Task<IActionResult> DeleteFromListConfirmed(int id, string? token)
        {
            if (_context.ProductListItems == null)
            {
                return Problem("Entity set 'ChipinDbContext.ProductListItems' is null.");
            }

            var productListItem = await _context.ProductListItems.FindAsync(id);
            if (productListItem == null)
            {
                return NotFound();
            }

            // Update the relevant ProductListWallet total and other properties
            var productListWallet = await _context.ProductListWallets.FindAsync(productListItem.ProductListWalletId);
            var product = await _context.Products.FindAsync(productListItem.ProductId);
            var exproduct = await _context.ExternalProducts.FindAsync(productListItem.ExternalProductId);
            if (productListItem.ProductId != null)
            {
                productListWallet.Total -= (product.Price * productListItem.Quantity);
            }
            else if (productListItem.ExternalProductId != null)
            {
                productListWallet.Total -= (exproduct.Price * productListItem.Quantity);
            }

            // Remove the ProductListItem from the data context
            _context.ProductListItems.Remove(productListItem);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "ProductListWallets", new { id = productListItem.ProductListWalletId, token });
        }

        // GET: ProductListWallets/Edit/5
        [Authorize]
        public async Task<IActionResult> DeliveryAddress(int? id, string? token)
        {
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            await Console.Out.WriteLineAsync("ooooo");
            if (id == null || _context.ProductListWallets == null)
            {
                await Console.Out.WriteLineAsync("lldlldldldl");
                return NotFound();
            }

            var productListWallet = await _context.ProductListWallets
                .Include(p => p.Chipin)
                .FirstOrDefaultAsync(m => m.ProductListWalletId == id);
            if (productListWallet == null)
            {
                await Console.Out.WriteLineAsync("ppppppp");

                return NotFound();
            }

            Console.WriteLine("yyyyyyyyyyyyyyychfchfcfhc");

            ViewBag.Products = _context.Products.ToList();
            ViewBag.ProductListItems = _context.ProductListItems.Where(wallet => wallet.ProductListWalletId == id).ToList();
            ViewBag.Wallets = productListWallet;
            ViewBag.ExternalProducts = _context.ExternalProducts.ToList();
            ViewBag.Addresses = _context.Addresses.Where(wallet => wallet.ChipinId.Equals(objectIdClaim.Value)).ToList();

            return View("Address");
        }

        // GET: ProductListWallets/Edit/5
        [Authorize]
        public async Task<IActionResult> EditAddress(int? id, string? token)
        {
            if (id == null || _context.ProductListWallets == null)
            {
                return NotFound();
            }

            var productListWallet = await _context.ProductListWallets.FindAsync(id);
            if (productListWallet == null)
            {
                return NotFound();
            }

            return View(productListWallet);
        }

        // POST: ProductListWallets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditAddress(int id, [Bind("AddressId")] ProductListWallet productListWallet, string? token)
        {
            var plw = await _context.ProductListWallets.FindAsync(id);
            if (plw == null)
            {
                return NotFound();
            }

            plw.AddressId = productListWallet.AddressId;

            _context.Update(plw);
            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();
            return RedirectToAction("DeliveryAddress", "ProductListWallets", new { id, token });
            //return RedirectToAction(nameof(Details), new { id, token });
        }

        // GET: ProductListWallets/Edit/5
        public async Task<IActionResult> ChipinAmount(int? id, string? token)
        {
            _logger.LogInformation($"\n\n\n ID: {id} \n\n\n");
            ViewBag.Token = token;
            ViewBag.Id = id;
            return View();
        }
    }
}
