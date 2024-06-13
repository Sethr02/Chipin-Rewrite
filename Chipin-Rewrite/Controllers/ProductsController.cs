using Chipin_Rewrite.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chipin_Rewrite.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ChipinDbContext _context;

        public ProductsController(ChipinDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Products1
        public async Task<IActionResult> Index(string? token)
        {

            List<ProductListWallet> wallets = new List<ProductListWallet>();
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (objectIdClaim != null)
            {
                wallets = _context.ProductListWallets.Where(wallet => wallet.ChipinId.Equals(objectIdClaim.Value)).ToList();
            }




            ViewBag.Wallets = wallets;

            return _context.Products != null ?
                          View(await _context.Products.ToListAsync()) :
                          Problem("Entity set 'ChipinDbContext.Products'  is null.");
        }




        [AllowAnonymous]
        // GET: Products1/Details/5
        public async Task<IActionResult> Details(int? id, string? token)
        {

            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            List<ProductListWallet> wallets = new List<ProductListWallet>();
            if (objectIdClaim != null)
            {

                wallets = _context.ProductListWallets.Where(wallet => wallet.ChipinId.Equals(objectIdClaim.Value)).ToList();
                ViewBag.ProductListItems = _context.ProductListItems
                                             .Where(item => item.ProductListWallet.ChipinId.Equals(objectIdClaim.Value))
                                             .ToList();
            }



            ViewBag.Products = new List<Product> { product };
            ViewBag.Wallets = wallets;
            ViewData["ProductListWalletId"] = _context.ProductListWallets.ToList();
            return View();
        }





        // GET: Products1/Details/5
        public async Task<IActionResult> ExternalDetails(int? id, string? token)
        {

            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.ExternalProducts
                .FirstOrDefaultAsync(m => m.ExternalProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            List<ProductListWallet> wallets = new List<ProductListWallet>();


            wallets = _context.ProductListWallets.Where(wallet => wallet.ChipinId.Equals(objectIdClaim.Value)).ToList();
            ViewBag.ProductListItems = _context.ProductListItems
                                         .Where(item => item.ProductListWallet.ChipinId.Equals(objectIdClaim.Value))
                                         .ToList();




            ViewBag.Products = new List<ExternalProduct> { product };
            ViewBag.Wallets = wallets;

            return View();
        }







        // GET: Products1/Create
        public IActionResult Create(string? token)
        {

            return View();
        }

        // POST: Products1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]

        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,Store,ProductImage,Quantity,Price,ProductImage1,ProductImage2,ProductImage3")] Product product, string? token)
        {



            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { token });
            }
            return View(product);



        }



        // GET: Products1/Edit/5
        public async Task<IActionResult> Edit(int? id, string? token)
        {





            if (id == null || _context.Products == null)
            {
                await Console.Out.WriteLineAsync("psppsspsp1");
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                await Console.Out.WriteLineAsync("psppsspsp2");
                return NotFound();
            }
            ViewBag.id = id;
            return View(product);

        }


        // POST: Products1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,Store,ProductImage,Quantity,Price,ProductImage1,ProductImage2,ProductImage3")] Product product, string? token)
        {




            if (id != product.ProductId)
            {
                await Console.Out.WriteLineAsync("psppsspsp3 " + id + " " + product.ProductId);
                await Console.Out.WriteLineAsync();
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        await Console.Out.WriteLineAsync("psppsspsp4");
                        return NotFound();
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync("psppsspsp5");
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { token });
            }
            await Console.Out.WriteLineAsync("psppsspsp6");
            return View(product);
        }


        // GET: Products1/Delete/5
        public async Task<IActionResult> Delete(int? id, string? token)
        {


            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return await DeleteConfirmed((int)id.Value, token);



        }


        // POST: Products1/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, string? token)
        {

            if (_context.Products == null)
            {
                return Problem("Entity set 'ChipinDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { token });
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }









        // POST: ProductListItems1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToList([Bind("ChipinProductListEntryId,ProductId,Quantity,ProductListWalletId")] ProductListItem productListItem, string? token)
        {

            if (ModelState.IsValid)
            {
                // Check if a similar product list item exists for the same ProductId and ProductListWalletId
                var existingItem = await _context.ProductListItems
                    .FirstOrDefaultAsync(p => p.ProductId == productListItem.ProductId && p.ProductListWalletId == productListItem.ProductListWalletId);

                if (existingItem != null)
                {
                    // If the item already exists, update the quantity
                    existingItem.Quantity += productListItem.Quantity;
                }
                else
                {
                    // If the item does not exist, add it to the context
                    _context.Add(productListItem);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Update the relevant ProductListWallet total and other properties
                var productListWallet = await _context.ProductListWallets.FindAsync(productListItem.ProductListWalletId);
                var product = await _context.Products.FindAsync(productListItem.ProductId);

                productListWallet.Total += product.Price * productListItem.Quantity;
                // Update other properties accordingly

                // Save changes to the ProductListWallet in the database
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = product.ProductId, token = token });
            }
            return RedirectToAction("Index", "UserTables");
        }



        // POST: ProductListItems1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExToList([Bind("ChipinProductListEntryId,ExternalProductId,Quantity,ProductListWalletId")] ProductListItem productListItem, string? token)
        {

            if (ModelState.IsValid)
            {
                // Check if a similar product list item exists for the same ProductId and ProductListWalletId
                var existingItem = await _context.ProductListItems
                    .FirstOrDefaultAsync(p => p.ExternalProductId == productListItem.ExternalProductId && p.ProductListWalletId == productListItem.ProductListWalletId);

                if (existingItem != null)
                {
                    // If the item already exists, update the quantity
                    existingItem.Quantity += 1;
                }
                else
                {
                    // If the item does not exist, add it to the context
                    _context.Add(productListItem);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Update the relevant ProductListWallet total and other properties
                var productListWallet = await _context.ProductListWallets.FindAsync(productListItem.ProductListWalletId);
                var product = await _context.ExternalProducts.FindAsync(productListItem.ExternalProductId);

                productListWallet.Total += product.Price;
                // Update other properties accordingly

                // Save changes to the ProductListWallet in the database
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "ProductListWallets", new { id = productListItem.ProductListWalletId, token = token });

            }
            return RedirectToAction("Index", "UserTables");
        }






        [HttpPost]

        public async Task<IActionResult> Link(string? path, int? id, string? token, [Bind("ChipinProductListEntryId,ProductId,ExternalProductId,Quantity,ProductListWalletId")] ProductListItem? productListItem)
        {

            await Console.Out.WriteLineAsync(path + " " + id + " " + token);
            if (path.Equals("Index"))
            {
                await Index(token);
                return View(path);
            }
            else if (path.Equals("Create"))
            {
                return View(path);
            }
            else if (path.Equals("Edit"))
            {
                await Edit(id, token);
            }
            else if (path.Equals("Details"))
            {
                await Details(id, token);
            }
            else if (path.Equals("Delete"))
            {
                await Delete(id, token);
                return RedirectToAction(nameof(Index), new { token });

            }
            else if (path.Equals("AddToList"))
            {
                await AddToList(productListItem, token);
                return RedirectToAction(nameof(Index), new { token });
            }
            else if (path.Equals("AddExToList"))
            {
                return await AddExToList(productListItem, token);
            }
            return View(path); ;
        }




    }
}
