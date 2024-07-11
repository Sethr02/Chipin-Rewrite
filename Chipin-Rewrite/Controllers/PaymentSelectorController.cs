using Chipin_Rewrite.Models.Entities;
using Chipin_Rewrite.Utility.Signatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chipin_Rewrite.Controllers
{
    public class PaymentSelectorController : Controller
    {
        private readonly ChipinDbContext _context;
        private readonly ISignatureGenerator _signatureGenerator;
        private readonly ILogger<ProductListWalletsController> _logger;
        public PaymentSelectorController(ChipinDbContext context, ISignatureGenerator signatureGenerator, ILogger<ProductListWalletsController> logger)
        {
            _context = context;
            _signatureGenerator = signatureGenerator;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? id, decimal? amount)
        {
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            // string clientChipinId = objectIdClaim.Value;
            // ViewBag.NavInfo = _context.TokenWallets.Where(xwallet => xwallet.ChipinId.Equals(clientChipinId)).ToList().FirstOrDefault().Amount;


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

            if (amount.HasValue)
            {
                ViewBag.Amount = amount.Value;
            }

            return View(productListWallet);
        }

        public async Task<IActionResult> Success(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("ID parameter is missing.");
            }

            var productListWalletId = Request.Query["id"].FirstOrDefault();
            var chipinAmount = Request.Query["amount"].FirstOrDefault();
            var transactionId = Request.Query["transaction_id"].FirstOrDefault();
            var paymentType = Request.Query["payment_type"].FirstOrDefault();
            var gatewayRef = Request.Query["gateway_reference"].FirstOrDefault();

            if (string.IsNullOrEmpty(productListWalletId) ||
                string.IsNullOrEmpty(chipinAmount) ||
                string.IsNullOrEmpty(transactionId) ||
                string.IsNullOrEmpty(paymentType) ||
                string.IsNullOrEmpty(gatewayRef))
            {
                return BadRequest("One or more required parameters are missing.");
            }

            int parsedProductListWalletId;
            float parsedChipinAmount;
            int parsedTransactionId;

            if (!int.TryParse(productListWalletId, out parsedProductListWalletId) ||
                !float.TryParse(chipinAmount, out parsedChipinAmount) ||
                !int.TryParse(transactionId, out parsedTransactionId))
            {
                return BadRequest("Error parsing parameters.");
            }

            // Get chipin id by finding the matching product list wallet
            var productListWallet = await _context.ProductListWallets.FindAsync(parsedProductListWalletId);
            ViewBag.Products = _context.Products.ToList();
            ViewBag.ProductListItems = _context.ProductListItems.Where(wallet => wallet.ProductListWalletId == id).ToList();
            ViewBag.Wallets = productListWallet;
            ViewBag.ExternalProducts = _context.ExternalProducts.ToList();

            if (productListWallet == null)
            {
                return NotFound("Product List Wallet not found.");
            }
            string chipinId = productListWallet.ChipinId;

            // Check if the transaction already exists
            var existingTransaction = _context.ProductListWalletTransactions
                                              .FirstOrDefault(t => t.ProductListWalletTransactionId == parsedTransactionId);
            if (existingTransaction != null)
            {
                // Transaction already processed, show the existing details
                return View(productListWallet);
            }
            else
            {
                var newTransaction = new ProductListWalletTransaction
                {
                    ProductListWalletTransactionId = parsedTransactionId,
                    Amount = parsedChipinAmount,
                    ChipinId = chipinId,
                    FromInvitedUser = 0,
                    ProductListWalletId = parsedProductListWalletId,
                    TransactionMethod = paymentType,
                };

                _context.ProductListWalletTransactions.Add(newTransaction);
                await _context.SaveChangesAsync();

                // Update product list wallet funded field by amount
                productListWallet.Funded += parsedChipinAmount;

                // Use chipin id of product list wallet and find the token wallet that has that chipin id
                var tokenWallet = _context.TokenWallets.FirstOrDefault(x => x.ChipinId == chipinId);
                if (tokenWallet != null)
                {
                    tokenWallet.Amount += parsedChipinAmount;
                }

                await _context.SaveChangesAsync();

                return View(productListWallet);
            }
        }
    }
}
