using Chipin_Rewrite.Models.Entities;
using Chipin_Rewrite.Utility.Signatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace Chipin_Rewrite.Controllers
{
    public class SharedListController : Controller
    {
        private readonly ChipinDbContext _context;
        private readonly ISignatureGenerator _signatureGenerator;
        private readonly ILogger<ProductListWalletsController> _logger;
        public SharedListController(ChipinDbContext context, ISignatureGenerator signatureGenerator, ILogger<ProductListWalletsController> logger)
        {
            _context = context;
            _signatureGenerator = signatureGenerator;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? id)
        {
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            /*varstring clientChipinId = objectIdClaim.Value;
            ViewBag.NavInfo = _context.TokenWallets.Where(xwallet => xwallet.ChipinId.Equals(clientChipinId)).ToList().FirstOrDefault().Amount;*/


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

        public async Task<IActionResult> ViewGiftList(int? id)
        {
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

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

            // Fetch ProductListItems with related ExternalProducts
            var productListItems = await _context.ProductListItems
                .Where(wallet => wallet.ProductListWalletId == id)
                .Include(item => item.ExternalProduct)
                .ToListAsync();

            ViewBag.Products = _context.Products.ToList();
            ViewBag.ProductListItems = productListItems;
            ViewBag.Wallets = productListWallet;
            ViewBag.ExternalProducts = productListItems.Select(item => item.ExternalProduct).ToList();

            // Check if user is logged in and that the chipin belongs to them
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
    }
}
