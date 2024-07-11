using Chipin_Rewrite.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;
using Newtonsoft.Json;
using System.Text;

namespace Chipin_Rewrite.Controllers
{
    public class CallPaySuccessTest : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
