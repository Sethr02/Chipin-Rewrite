using Chipin_Rewrite.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chipin_Rewrite.Controllers
{
    [AllowAnonymous]
    public class BillingAddressesController : Controller
    {
        private readonly ChipinDbContext _context;

        public BillingAddressesController(ChipinDbContext context)
        {
            _context = context;
        }


        // GET: BillingAddresses
        public async Task<IActionResult> Index(string? token)
        {


            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            List<BillingAddress> billAddress = new List<BillingAddress>();
            if (true)
            {

                billAddress = _context.BillingAddresses.Where(wallet => wallet.ChipinId.Equals("24")).ToList();

            }
            ViewBag.BillingAddresses = billAddress;
            return View();
        }




        // GET: BillingAddresses/Create
        public IActionResult Create(string? token, bool? onLogin)
        {
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (onLogin != null)
            {
                ViewBag.onLogin = onLogin;
            }
            else
            {
                ViewBag.onLogin = false;
            }

            List<BillingAddress> modelx = new List<BillingAddress>();
            if (objectIdClaim != null)
            {
                modelx = _context.BillingAddresses.Where(wallet => wallet.ChipinId.Equals(objectIdClaim.Value)).ToList();
            }
            return View();
        }




        // POST: BillingAddresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<IActionResult> Create([Bind("BillingAddressId,AdressName,Address1,Address2,City,State,Country,PostCode,IsDefault,FirstName,LastName,PhoneNumber,Email")] BillingAddress billingAddress, string? token)
        {
            Console.WriteLine("Create Billing Address");

            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (objectIdClaim != null)
            {
                billingAddress.ChipinId = objectIdClaim.Value;
                billingAddress.IsDefault = true;
                _context.Add(billingAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { token });
            }


            return View(billingAddress);
        }

        // GET: BillingAddresses/Edit/5
        public async Task<IActionResult> Edit(int? id, string? token)
        {

            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (id == null || _context.BillingAddresses == null)
            {
                return NotFound();
            }





            var billingAddress = await _context.BillingAddresses.FindAsync(id);

            if (billingAddress == null)
            {
                return NotFound();
            }

            if (objectIdClaim != null)
            {
                //TempData["chipinId"] = val.ToString();

                if (billingAddress.ChipinId.Equals(objectIdClaim.Value))
                {
                    ViewBag.id = id;
                    return View(billingAddress);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return RedirectToAction("Index", "UserTables");
            }

        }

        // POST: BillingAddresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<IActionResult> Edit(int id, [Bind("AdressName,Address1,Address2,City,State,Country,ChipinId,PostCode,IsDefault,FirstName,LastName,PhoneNumber,Email")] BillingAddress billingAddress, string? token)
        {


            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!BillingAddressExists(id))
            {
                return NotFound();
            }




            if (objectIdClaim != null)
            {
                billingAddress.BillingAddressId = id;
                billingAddress.ChipinId = objectIdClaim.Value;
                _context.Update(billingAddress);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { token });



            return View(billingAddress);
        }



        // GET: BillingAddresses/Delete/5
        public async Task<IActionResult> Delete(int? id, string? token)
        {


            if (id == null || _context.BillingAddresses == null)
            {
                return NotFound();
            }

            var billingAddress = await _context.BillingAddresses
                .Include(b => b.Chipin)
                .FirstOrDefaultAsync(m => m.BillingAddressId == id);
            if (billingAddress == null)
            {
                return NotFound();
            }

            return await DeleteConfirmed(id.Value, token);
        }

        // POST: BillingAddresses/Delete/5
        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id, string? token)
        {




            if (_context.BillingAddresses == null)
            {
                return Problem("Entity set 'ChipinDbContext.BillingAddresses'  is null.");
            }
            var billingAddress = await _context.BillingAddresses.FindAsync(id);
            if (billingAddress != null)
            {
                _context.BillingAddresses.Remove(billingAddress);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { token });


        }

        private bool BillingAddressExists(int id)
        {
            return (_context.BillingAddresses?.Any(e => e.BillingAddressId == id)).GetValueOrDefault();
        }

        [HttpPost]

        public async Task<IActionResult> Link(string? path, int? id, string? token)
        {
            await Console.Out.WriteLineAsync(path + " " + id + " " + token);
            if (path.Equals("Index"))
            {
                await Index(token);

            }
            else if (path.Equals("Create"))
            {
                return View(path);
            }

            else if (path.Equals("Edit"))
            {
                await Edit(id, token);
            }


            return View(path); ;
        }


    }
}
