using Chipin_Rewrite.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Chipin_Rewrite.Controllers
{
    public class AddToListFlowTestController : Controller
    {
        // GET: List
        public IActionResult Index()
        {
            var model = new ListViewModel
            {
                ExistingLists = new List<string> { "Haydan's Birthday", "New List" } // Populate with existing lists
            };
            return View(model);
        }

        // POST: List/AddToList
        [HttpPost]
        public IActionResult AddToList(string existingListName)
        {
            var model = new ListViewModel
            {
                ExistingListName = existingListName
            };
            return View("Step2", model);
        }

        // POST: List/CreateNewList
        [HttpPost]
        public IActionResult CreateNewList(string listName)
        {
            var model = new ListViewModel
            {
                ListName = listName
            };
            return View("Step3", model);
        }

        // POST: List/ChooseDeliveryDate
        [HttpPost]
        public IActionResult ChooseDeliveryDate(string listName, DateTime deliveryDate)
        {
            // Save the list and delivery date to database here

            return RedirectToAction("Confirmation");
        }

        // GET: List/Confirmation
        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
