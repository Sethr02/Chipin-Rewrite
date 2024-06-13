using Microsoft.AspNetCore.Mvc;

namespace Chipin_Rewrite.Controllers
{
    public class SharedListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
