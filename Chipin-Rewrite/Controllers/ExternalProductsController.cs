using Chipin_Rewrite.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chipin_Rewrite.Controllers
{
    public class ExternalProductsController : Controller
    {
        private readonly ChipinDbContext _context;

        public ExternalProductsController(ChipinDbContext context)
        {
            _context = context;
        }

        // GET: ExternalProducts
        public async Task<IActionResult> Index()
        {
            return _context.ExternalProducts != null ?
                        View(await _context.ExternalProducts.ToListAsync()) :
                        Problem("Entity set 'ChipinDbContext.ExternalProducts'  is null.");
        }

        // GET: ExternalProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ExternalProducts == null)
            {
                return NotFound();
            }

            var externalProduct = await _context.ExternalProducts
                .FirstOrDefaultAsync(m => m.ExternalProductId == id);
            if (externalProduct == null)
            {
                return NotFound();
            }

            return View(externalProduct);
        }

        // GET: ExternalProducts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExternalProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExternalProductId,ProductName,ProductDescription,Price,PageUrl,Store,CustString1,CustString2,CustString3,CustInt1,CustInt2,CustInt3,Image,Image1,Image2,Image3,ReturnUrl")] ExternalProduct externalProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(externalProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(externalProduct);
        }

        // GET: ExternalProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ExternalProducts == null)
            {
                return NotFound();
            }

            var externalProduct = await _context.ExternalProducts.FindAsync(id);
            if (externalProduct == null)
            {
                return NotFound();
            }
            return View(externalProduct);
        }

        // POST: ExternalProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExternalProductId,ProductName,ProductDescription,Price,PageUrl,Store,CustString1,CustString2,CustString3,CustInt1,CustInt2,CustInt3,Image,Image1,Image2,Image3,ReturnUrl")] ExternalProduct externalProduct)
        {
            if (id != externalProduct.ExternalProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(externalProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExternalProductExists(externalProduct.ExternalProductId))
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
            return View(externalProduct);
        }

        // GET: ExternalProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ExternalProducts == null)
            {
                return NotFound();
            }

            var externalProduct = await _context.ExternalProducts
                .FirstOrDefaultAsync(m => m.ExternalProductId == id);
            if (externalProduct == null)
            {
                return NotFound();
            }

            return View(externalProduct);
        }

        // POST: ExternalProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ExternalProducts == null)
            {
                return Problem("Entity set 'ChipinDbContext.ExternalProducts'  is null.");
            }
            var externalProduct = await _context.ExternalProducts.FindAsync(id);
            if (externalProduct != null)
            {
                _context.ExternalProducts.Remove(externalProduct);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExternalProductExists(int id)
        {
            return (_context.ExternalProducts?.Any(e => e.ExternalProductId == id)).GetValueOrDefault();
        }
    }
}
