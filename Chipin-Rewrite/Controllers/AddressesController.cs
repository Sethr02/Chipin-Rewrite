using Chipin_Rewrite.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chipin_Rewrite.Controllers
{
    [Authorize]
    public class AddressesController : Controller
    {
        private readonly ChipinDbContext _context;

        public AddressesController(ChipinDbContext context)
        {
            _context = context;
        }


        // GET: Addresses
        public async Task<IActionResult> Index(string? token)
        {



            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            List<Address> address = new List<Address>();
            if (objectIdClaim != null)
            {
                address = _context.Addresses.Where(wallet => wallet.ChipinId.Equals(objectIdClaim.Value)).ToList();
            }


            ViewBag.Addresses = address;
            return View();
        }



        // GET: Addresses/Create
        public IActionResult Create(string? token, int? fromList)
        {
            Console.WriteLine("fromList " + fromList);

            if (fromList != null)
            {
                ViewBag.fromList = fromList;
            }
            else
            {
                ViewBag.fromList = null;
            }

            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);


            List<Address> address = new List<Address>();
            if (objectIdClaim != null)
            {

                address = _context.Addresses.Where(wallet => wallet.ChipinId.Equals(objectIdClaim.Value)).ToList();


            }

            return View();
        }


        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<IActionResult> Create([Bind("AddressId,AdressName,Address1,Address2,City,State,Country,PostCode,IsDefault,FirstName,LastName,PhoneNumber,Email")] Address address, string? token, int? fromList)
        {
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (objectIdClaim != null)
            {
                address.ChipinId = objectIdClaim.Value;
                if ((bool)address.IsDefault)
                {
                    List<Address> modelx = _context.Addresses.Where(wallet => wallet.ChipinId.Equals(objectIdClaim.Value)).ToList();
                    foreach (var item in modelx)
                    {
                        item.IsDefault = false;
                    }
                }
                _context.Add(address);
                await _context.SaveChangesAsync();
                if (fromList != null)
                {
                    return RedirectToAction("Details", "ProductListWallets", new { id = fromList, token = token });
                }
                return RedirectToAction("Index", new { token });
            }


            return View(address);
        }


        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(int? id, string? token)
        {
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (id == null || _context.BillingAddresses == null)
            {
                return NotFound();
            }
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            if (objectIdClaim != null)
            {
                if (address.ChipinId.Equals(objectIdClaim.Value))
                {
                    ViewBag.id = id;
                    return View(address);
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



        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<IActionResult> Edit(int id, [Bind("AdressName,Address1,Address2,City,State,Country,PostCode,IsDefault,ProductListWalletId,FirstName,LastName,PhoneNumber,Email")] Address address, string? token)
        {
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (!AddressExists(id))
            {
                return NotFound();
            }


            try
            {
                if (objectIdClaim != null)
                {


                    List<Address> modelx = _context.Addresses.Where(wallet => wallet.ChipinId.Equals(objectIdClaim.Value)).ToList();

                    if ((bool)address.IsDefault)
                    {

                        foreach (var item in modelx)
                        {

                            if ((bool)item.IsDefault)
                            {
                                await Console.Out.WriteLineAsync("ddd " + item.AdressName);
                                // Update properties of the default address
                                item.IsDefault = false;
                                _context.Update(item);
                                await _context.SaveChangesAsync();
                            }
                            await Console.Out.WriteLineAsync("ggg " + item.AddressId + " " + id);
                            if (item.AddressId == id)
                            {
                                await Console.Out.WriteLineAsync("ff " + address.AdressName);
                                item.AdressName = address.AdressName;
                                item.Address1 = address.Address1;
                                item.Address2 = address.Address2;
                                item.City = address.City;
                                item.State = address.State;
                                item.Country = address.Country;
                                item.PostCode = address.PostCode;
                                item.IsDefault = address.IsDefault;
                                item.FirstName = address.FirstName;
                                item.LastName = address.LastName;
                                item.PhoneNumber = address.PhoneNumber;
                                item.Email = address.Email;
                                _context.Update(item);
                                await _context.SaveChangesAsync();
                            }
                        }

                    }
                    else
                    {
                        // Retrieve the existing address from the database
                        var existingAddress = await _context.Addresses.FindAsync(id);

                        if (existingAddress == null)
                        {
                            return NotFound();
                        }

                        // Update the properties of the existing address
                        existingAddress.AdressName = address.AdressName;
                        existingAddress.Address1 = address.Address1;
                        existingAddress.Address2 = address.Address2;
                        existingAddress.City = address.City;
                        existingAddress.State = address.State;
                        existingAddress.Country = address.Country;
                        existingAddress.PostCode = address.PostCode;
                        existingAddress.IsDefault = address.IsDefault;
                        existingAddress.FirstName = address.FirstName;
                        existingAddress.LastName = address.LastName;
                        existingAddress.PhoneNumber = address.PhoneNumber;
                        existingAddress.Email = address.Email;

                        // Update the existing address and save changes
                        _context.Update(existingAddress);
                        await _context.SaveChangesAsync();
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;

                }
            }

            return RedirectToAction("Index", new { token });





        }






        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(int? id, string? token)
        {

            if (id == null || _context.Addresses == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .Include(a => a.Chipin)
                // .Include(a => a.ProductListWallet)
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }
            return await DeleteConfirmed(id.Value, token);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id, string? token)
        {
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);


            if (_context.Addresses == null)
            {
                return Problem("Entity set 'ChipinDbContext.Addresses'  is null.");
            }
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { token });
        }

        private bool AddressExists(int id)
        {
            return (_context.Addresses?.Any(e => e.AddressId == id)).GetValueOrDefault();
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
