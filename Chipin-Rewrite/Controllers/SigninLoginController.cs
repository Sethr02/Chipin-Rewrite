using Microsoft.AspNetCore.Mvc;

namespace Chipin_Rewrite.Controllers
{
    public class SigninLoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
