using Chipin_Rewrite.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Chipin_Rewrite.Controllers
{
    public class ShareListsController : Controller
    {
        private readonly ChipinDbContext _context;
        private readonly ILogger<ShareListsController> _logger;

        public ShareListsController(ChipinDbContext context, ILogger<ShareListsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ShareLists
        public async Task<IActionResult> Index()
        {
            var chipinDbContext = _context.ShareLists.Include(s => s.ProductListWalletTransaction);
            return View(await chipinDbContext.ToListAsync());
        }

        // GET: ShareLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShareLists == null)
            {
                return NotFound();
            }

            var shareList = await _context.ShareLists
                .Include(s => s.ProductListWalletTransaction)
                .FirstOrDefaultAsync(m => m.ShareId == id);
            if (shareList == null)
            {
                return NotFound();
            }

            return View(shareList);
        }

        // GET: ShareLists/Create
        public IActionResult Create(int listId)
        {






            var id = listId;
            ViewBag.TransactionId = id; // ProductListWalletTransactionId is the gateway_reference which is the transaction_id in the db
            return View();
        }

        // POST: ShareLists/Create
        // Protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShareName,ShareEmail,ShareMessage")] ShareList shareList, string transactionId)
        {
            if (ModelState.IsValid)
            {
                var productlistwallettransaction = _context.ProductListWalletTransactions.Where(x => x.TransactionId == transactionId).FirstOrDefault();
                shareList.ProductListWalletTransactionId = productlistwallettransaction.ProductListWalletTransactionId;
                shareList.ShareDate = DateTime.Now;
                _context.Add(shareList);
                await _context.SaveChangesAsync();
                //find the productlistwallet from the productlistwallettransaction
                var productlistwallet = _context.ProductListWallets.Where(x => x.ProductListWalletId == productlistwallettransaction.ProductListWalletId).FirstOrDefault();


                return RedirectToAction("Details", "ProductListWallets", new { id = productlistwallet.ProductListWalletId });
            }
            ViewData["ProductListWalletTransactionId"] = new SelectList(_context.ProductListWalletTransactions, "ProductListWalletTransactionId", "ProductListWalletTransactionId", shareList.ProductListWalletTransactionId);
            return RedirectToAction("Index", "NonFunctionalPages");
        }

        // GET: ShareLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShareLists == null)
            {
                return NotFound();
            }

            var shareList = await _context.ShareLists.FindAsync(id);
            if (shareList == null)
            {
                return NotFound();
            }
            ViewData["ProductListWalletTransactionId"] = new SelectList(_context.ProductListWalletTransactions, "ProductListWalletTransactionId", "ProductListWalletTransactionId", shareList.ProductListWalletTransactionId);
            return View(shareList);
        }

        // POST: ShareLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShareId,ShareName,ShareEmail,ShareMessage,ShareDate,ProductListWalletTransactionId")] ShareList shareList)
        {
            if (id != shareList.ShareId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shareList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShareListExists(shareList.ShareId))
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
            ViewData["ProductListWalletTransactionId"] = new SelectList(_context.ProductListWalletTransactions, "ProductListWalletTransactionId", "ProductListWalletTransactionId", shareList.ProductListWalletTransactionId);
            return View(shareList);
        }

        // GET: ShareLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShareLists == null)
            {
                return NotFound();
            }

            var shareList = await _context.ShareLists
                .Include(s => s.ProductListWalletTransaction)
                .FirstOrDefaultAsync(m => m.ShareId == id);
            if (shareList == null)
            {
                return NotFound();
            }

            return View(shareList);
        }

        // POST: ShareLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShareLists == null)
            {
                return Problem("Entity set 'ChipinDbContext.ShareLists'  is null.");
            }
            var shareList = await _context.ShareLists.FindAsync(id);
            if (shareList != null)
            {
                _context.ShareLists.Remove(shareList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShareListExists(int id)
        {
            return (_context.ShareLists?.Any(e => e.ShareId == id)).GetValueOrDefault();
        }
    }
}
