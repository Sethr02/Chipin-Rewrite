using Chipin_Rewrite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Chipin_Rewrite.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [ChipInAuthorizationFilter]
        public IActionResult Index(string? token)
        {
            if (ControllerContext.ActionDescriptor.Properties.TryGetValue("ViewMode", out var viewmode))
            {
                ViewBag.viewMode = viewmode;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
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