using Chipin_Rewrite.Models;
using Chipin_Rewrite.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Chipin_Rewrite.Controllers
{
    public class Home_Controller : Controller
    {
        private readonly ChipinDbContext _context;
        private readonly ILogger<Home_Controller> _logger;

        public Home_Controller(ChipinDbContext context, ILogger<Home_Controller> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            var objectIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);

            if (ControllerContext.ActionDescriptor.Properties.TryGetValue("ViewMode", out var viewmode))
            {
                ViewBag.viewMode = viewmode;
            }

            List<UserTable> modelx = new List<UserTable>();
            if (objectIdClaim != null)
            {
                modelx = _context.UserTables.Where(wallet => wallet.ChipinId.Equals(objectIdClaim.Value)).ToList();

            }
            else
                if (objectIdClaim == null || _context.UserTables == null)
            {
                return NotFound();
            }

            ViewBag.UserTable = modelx;

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
        public IActionResult FAQs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
