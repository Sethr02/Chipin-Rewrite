using Chipin_Rewrite.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Chipin_Rewrite.Controllers
{
    public class ProductListWalletTransactionDatabaseController : Controller
    {
        private readonly ChipinDbContext _context;

        public ProductListWalletTransactionDatabaseController(ChipinDbContext context)
        {
            _context = context;
        }

        // GET: ProductListItems1
        public async Task<IActionResult> Index()
        {
            var chipinDbContext = _context.ProductListWalletTransactions;
            return View(await chipinDbContext.ToListAsync());
        }

        // GET: ProductListItems1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductListItems == null)
            {
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

            return View(productListItem);
        }

        // GET: ProductListItems1/Create
        public IActionResult Create()
        {
            ViewData["ExternalProductId"] = new SelectList(_context.ExternalProducts, "ExternalProductId", "ExternalProductId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            ViewData["ProductListWalletId"] = new SelectList(_context.ProductListWallets, "ProductListWalletId", "ProductListWalletId");
            return View();
        }

        // POST: ProductListItems1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChipinProductListEntryId,ProductId,Quantity,ProductListWalletId,ExternalProductId")] ProductListItem productListItem)
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
                var exproduct = await _context.ExternalProducts.FindAsync(productListItem.ExternalProductId);
                if (productListItem.ProductId != null)
                {
                    productListWallet.Total += product.Price * productListItem.Quantity;
                }
                else if (productListItem.ExternalProductId != null)
                {
                    productListWallet.Total += exproduct.Price * productListItem.Quantity;
                }



                // Save changes to the ProductListWallet in the database
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExternalProductId"] = new SelectList(_context.ExternalProducts, "ExternalProductId", "ExternalProductId", productListItem.ExternalProductId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", productListItem.ProductId);
            ViewData["ProductListWalletId"] = new SelectList(_context.ProductListWallets, "ProductListWalletId", "ProductListWalletId", productListItem.ProductListWalletId);
            return View(productListItem);
        }

        // GET: ProductListItems1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductListItems == null)
            {
                return NotFound();
            }

            var productListItem = await _context.ProductListItems.FindAsync(id);
            if (productListItem == null)
            {
                return NotFound();
            }
            ViewData["ExternalProductId"] = new SelectList(_context.ExternalProducts, "ExternalProductId", "ExternalProductId", productListItem.ExternalProductId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", productListItem.ProductId);
            ViewData["ProductListWalletId"] = new SelectList(_context.ProductListWallets, "ProductListWalletId", "ProductListWalletId", productListItem.ProductListWalletId);
            return View(productListItem);
        }

        // POST: ProductListItems1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChipinProductListEntryId,ProductId,Quantity,ProductListWalletId,ExternalProductId")] ProductListItem productListItem)
        {
            if (id != productListItem.ChipinProductListEntryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productListItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductListItemExists(productListItem.ChipinProductListEntryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExternalProductId"] = new SelectList(_context.ExternalProducts, "ExternalProductId", "ExternalProductId", productListItem.ExternalProductId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", productListItem.ProductId);
            ViewData["ProductListWalletId"] = new SelectList(_context.ProductListWallets, "ProductListWalletId", "ProductListWalletId", productListItem.ProductListWalletId);
            return View(productListItem);
        }


        // POST: ProductListItems1/Delete/5
        [HttpPost, ActionName("DeleteXXX")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductListWalletTransactions == null)
            {
                return Problem("Entity set 'ChipinDbContext.ProductListWallets'  is null.");
            }
            var productListWalletTransaction = await _context.ProductListWalletTransactions.FindAsync(id);
            if (productListWalletTransaction != null)
            {
                _context.ProductListWalletTransactions.Remove(productListWalletTransaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductListItemExists(int id)
        {
            return (_context.ProductListItems?.Any(e => e.ChipinProductListEntryId == id)).GetValueOrDefault();
        }

        // GET: ProductListItems1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductListWallets == null)
            {
                return NotFound();
            }

            var productListWalletTransactionItem = await _context.ProductListWalletTransactions
                .FirstOrDefaultAsync(m => m.ProductListWalletTransactionId == id);
            if (productListWalletTransactionItem == null)
            {
                return NotFound();
            }

            // Call the DeleteConfirmed method directly to perform deletion
            await DeleteConfirmed(productListWalletTransactionItem.ProductListWalletTransactionId);

            return RedirectToAction(nameof(Index));
        }

    }
}
