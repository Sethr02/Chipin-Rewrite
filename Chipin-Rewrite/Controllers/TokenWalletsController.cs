using Chipin_Rewrite.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Chipin_Rewrite.Controllers
{
    public class TokenWalletsController : Controller
    {
        private readonly ChipinDbContext _context;

        public TokenWalletsController(ChipinDbContext context)
        {
            _context = context;
        }

        // GET: TokenWallets
        public async Task<IActionResult> Index()
        {
            var chipinDbContext = _context.TokenWallets.Include(t => t.Chipin);
            return View(await chipinDbContext.ToListAsync());
        }

        // GET: TokenWallets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TokenWallets == null)
            {
                return NotFound();
            }

            var tokenWallet = await _context.TokenWallets
                .Include(t => t.Chipin)
                .FirstOrDefaultAsync(m => m.TokenWalletId == id);
            if (tokenWallet == null)
            {
                return NotFound();
            }

            return View(tokenWallet);
        }
        public IActionResult Chipin(int? id)
        {
            ViewBag.ProductListWalletTransactionId = id;
            return View();
        }

        // GET: TokenWallets/Create
        public IActionResult Create()
        {
            ViewData["ChipinId"] = new SelectList(_context.UserTables, "ChipinId", "ChipinId");
            return View();
        }

        // POST: TokenWallets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TokenWalletId,Amount,ChipinId")] TokenWallet tokenWallet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tokenWallet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChipinId"] = new SelectList(_context.UserTables, "ChipinId", "ChipinId", tokenWallet.ChipinId);
            return View(tokenWallet);
        }

        // GET: TokenWallets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TokenWallets == null)
            {
                return NotFound();
            }

            var tokenWallet = await _context.TokenWallets.FindAsync(id);
            if (tokenWallet == null)
            {
                return NotFound();
            }
            ViewData["ChipinId"] = new SelectList(_context.UserTables, "ChipinId", "ChipinId", tokenWallet.ChipinId);
            return View(tokenWallet);
        }

        // POST: TokenWallets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TokenWalletId,Amount,ChipinId")] TokenWallet tokenWallet)
        {
            if (id != tokenWallet.TokenWalletId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tokenWallet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TokenWalletExists(tokenWallet.TokenWalletId))
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
            ViewData["ChipinId"] = new SelectList(_context.UserTables, "ChipinId", "ChipinId", tokenWallet.ChipinId);
            return View(tokenWallet);
        }

        // GET: TokenWallets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TokenWallets == null)
            {
                return NotFound();
            }

            var tokenWallet = await _context.TokenWallets
                .Include(t => t.Chipin)
                .FirstOrDefaultAsync(m => m.TokenWalletId == id);
            if (tokenWallet == null)
            {
                return NotFound();
            }

            return View(tokenWallet);
        }

        // POST: TokenWallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TokenWallets == null)
            {
                return Problem("Entity set 'ChipinDbContext.TokenWallets'  is null.");
            }
            var tokenWallet = await _context.TokenWallets.FindAsync(id);
            if (tokenWallet != null)
            {
                _context.TokenWallets.Remove(tokenWallet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TokenWalletExists(int id)
        {
            return (_context.TokenWallets?.Any(e => e.TokenWalletId == id)).GetValueOrDefault();
        }
    }
}
