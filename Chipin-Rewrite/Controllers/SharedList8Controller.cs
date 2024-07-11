using Chipin_Rewrite.Models.Entities;
using Chipin_Rewrite.Utility.Signatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chipin_Rewrite.Controllers
{
    public class SharedList8Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
