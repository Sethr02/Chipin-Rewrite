using Chipin_Rewrite.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace Chipin_Rewrite.Controllers
{

    [Authorize]

    public class UserTablesController : Controller
    {
        private readonly ChipinDbContext _context;


        public UserTablesController(ChipinDbContext context)
        {
            _context = context;

        }


        // GET: UserTables
        public async Task<IActionResult> Index(string data, string cart)
        {

            // Console.WriteLine(data);
            if (!string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(cart))
            {
                string decodedData = System.Net.WebUtility.UrlDecode(data);
                string decodedCart = System.Net.WebUtility.UrlDecode(cart);

                ViewBag.returnUrl = decodedData;
                ViewBag.cart = decodedCart;

                Console.WriteLine(decodedData);
                Console.WriteLine(decodedCart);
            }
            else
            {
                ViewBag.returnUrl = "";
                ViewBag.cart = "";
                Console.WriteLine("No data");
            }

            ViewBag.DevUserTable = _context.UserTables.ToList<UserTable>();
            return _context.UserTables != null ?
                          View() :
                          Problem("Entity set 'ChipinDbContext.UserTables'  is null.");
        }

        // GET: UserTables1/Details/5
        public async Task<IActionResult> Details(string? token)
        {

            var objectIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);

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


            return View();
        }

        // GET: UserTables1/Create
        public IActionResult Create(string data)
        {
            Console.WriteLine(data);
            return View();
        }

        // POST: UserTables1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChipinId,ChipinName,ChipinCreatedDate,TokenWalletId,UserEmail")] UserTable userTable)
        {
            //see if email is in database already and set a bool to true if it is


            if (ModelState.IsValid)
            {
                _context.Add(userTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userTable);
        }



        // GET: UserTables1/Edit/5
        public async Task<IActionResult> Edit(string? token)
        {
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            UserTable userTable = new UserTable();
            if (objectIdClaim != null)
            {
                if (_context.UserTables == null)
                {
                    return NotFound();
                }
                userTable = await _context.UserTables.FindAsync(objectIdClaim.Value);
            }

            if (userTable == null)
            {
                return NotFound();
            }
            return View(userTable);

        }


        // POST: UserTables1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("ChipinName,UserEmail")] UserTable userTable, string? token)
        {

            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);



            if (objectIdClaim != null)
            {
                var ut = await _context.UserTables.FindAsync(objectIdClaim.Value);
                ut.ChipinName = userTable.ChipinName;
                ut.UserEmail = userTable.UserEmail;

                _context.Update(ut);
            }

            await _context.SaveChangesAsync();

            return View(userTable);
        }

        // GET: UserTables1/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.UserTables == null)
            {
                return NotFound();
            }

            var userTable = await _context.UserTables
                .FirstOrDefaultAsync(m => m.ChipinId == id);
            if (userTable == null)
            {
                return NotFound();
            }

            return View(userTable);
        }

        // POST: UserTables1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.UserTables == null)
            {
                return Problem("Entity set 'ChipinDbContext.UserTables'  is null.");
            }
            var userTable = await _context.UserTables.FindAsync(id);
            if (userTable != null)
            {
                _context.UserTables.Remove(userTable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTableExists(string id)
        {
            return (_context.UserTables?.Any(e => e.ChipinId == id)).GetValueOrDefault();
        }








        [HttpGet]
        public IActionResult userSignInSignUp(string? redirectUrl)
        {

            //_logger.LogInformation($"UserTablesController.userSignInSignUp() called. \n RedirectUrl is {redirectUrl}");
            foreach (var cl in User.Claims)
            {
                Console.WriteLine($"Claim Type: {cl.Type}, Claim Value: {cl.Value} ");
            }
            Console.WriteLine("_________________________________________");

            // Retrieve the user's Azure AD object ID from the claims
            var objectIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);




            if (objectIdClaim != null)
            {
                var objectId = objectIdClaim.Value;

                // Retrieve the user's email claim from the claims
                var emailClaim = User.FindFirst("emails");
                var username = User.FindFirst("name").Value;

                DateTime dtnow = DateTime.Now;
                var someSalt = GenerateSalt();



                if (emailClaim != null)
                {
                    var email = emailClaim.Value;
                    Console.WriteLine("Email: " + email + " name: " + username);



                    Console.WriteLine("Azure AD Object ID: " + objectId + " email: " + email);

                    // Check if the user already exists in the database based on their email
                    var existingUser = _context.UserTables.FirstOrDefault(u => u.UserEmail == email);
                    var tokenWId = GenerateTokenWalletId(email, dtnow, someSalt);

                    if (existingUser == null)
                    {


                        // User does not exist, so add them to the database
                        var newUser = new UserTable
                        {
                            ChipinId = objectId,
                            ChipinName = username, // Provide a default value or customize as needed
                            ChipinCreatedDate = dtnow,
                            UserEmail = email,
                            TokenWalletId = tokenWId,
                            Salt = someSalt
                            // Add other properties as needed
                        };

                        _context.UserTables.Add(newUser);
                        _context.SaveChanges(); // Save changes to the database

                        //Create a tokenWallet and add it to the database
                        var tokenWallet = new TokenWallet
                        {
                            ChipinId = objectId,
                            Amount = 0
                        };
                        _context.TokenWallets.Add(tokenWallet);
                        _context.SaveChanges(); // Save changes to the database


                        Console.WriteLine("User added to the database.");
                        if (redirectUrl != null)
                        {
                            return Redirect(redirectUrl);
                        }


                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Console.WriteLine("User already exists in the database.");
                        if (redirectUrl != null)
                        {
                            return Redirect(redirectUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }


                }
                else
                {
                    // Handle the case where the email claim is not present
                    return BadRequest("Email claim not found");
                }
            }
            else
            {
                // Handle the case where the object ID claim is not present
                return BadRequest("Azure AD Object ID claim not found");
            }
        }



        private readonly Random _random = new Random();
        private string GenerateSalt()
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] chars = new char[16];
            for (int i = 0; i < 16; i++)
            {
                chars[i] = validChars[_random.Next(validChars.Length)];
            }
            return new string(chars);
        }
        private int GenerateTokenWalletId(string email, DateTime registrationDate, string salt)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                // Combine the email, registrationDate, and salt as input to the hash function
                var inputBytes = Encoding.UTF8.GetBytes(email + registrationDate.ToString() + salt);

                // Calculate the hash and take the first 8 bytes (64 bits)
                var hashBytes = sha256.ComputeHash(inputBytes);
                var hashInt = BitConverter.ToInt64(hashBytes, 0);

                // Use the absolute value to avoid negative values
                var tokenWalletId = Math.Abs(hashInt);

                // Truncate the value to fit into INT11 range (0 to 2,147,483,647)
                tokenWalletId %= 10000000000; // 10-digit integer

                return (int)tokenWalletId;
            }
        }












        //// ...

        //[HttpPost]
        //public async Task<IActionResult> SignUp([Bind("UserEmail, UserPass")] UserTable u)
        //{
        //    // Base URL of the authentication API
        //    string apiBaseUrl = "http://localhost:5000"; // Replace with your API base URL

        //    // Create the login request payload
        //    var loginData = new
        //    {
        //        Username = u.UserEmail,
        //        Password = u.UserPass
        //    };

        //    //check if email is in database already and set bool to true if it is
        //    var user = await _context.UserTables.FirstOrDefaultAsync(m => m.UserEmail == u.UserEmail);
        //    if (user != null)
        //    {
        //        ModelState.AddModelError("", "This email has already linked to another account.");
        //        return View("Create");
        //    }

        //    // Serialize the loginData to JSON
        //    var jsonPayload = JsonSerializer.Serialize(loginData);

        //    // Create a HttpClient instance
        //    using (var httpClient = new HttpClient())
        //    {
        //        // Set the Content-Type header to application/json
        //        httpClient.DefaultRequestHeaders.Accept.Clear();
        //        httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //        // Create the HttpContent from the JSON payload
        //        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        //        // Send the POST request to the specified URL
        //        var response = await httpClient.PostAsync($"{apiBaseUrl}/Users/register", content);

        //        // Check if the response indicates successful registration (you should customize this based on your API)
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Read the response content as a string
        //            string responseContent = await response.Content.ReadAsStringAsync();

        //            // Deserialize the JSON response to an object
        //            var responseObject = JsonSerializer.Deserialize<RegistrationResponse>(responseContent);

        //            // Check if the user was registered successfully
        //            if (responseObject != null && responseObject.message == "User registered successfully")
        //            {
        //                // Redirect to another page or perform the desired action upon successful registration
        //                // For example, redirect to the user's profile page:
        //                Console.WriteLine("User registered successfully : " + responseObject.token);
        //                Email email = new Email("rademanalfred@gmail.com");
        //                email.SendMode2(responseObject.token);
        //                return RedirectToAction("VerifyEmailRequest", "NonFunctionalPages");
        //            }
        //            else
        //            {
        //                // Registration failed or the response does not match the expected format, show an error message or perform other actions
        //                ModelState.AddModelError("", "Registration failed. Please try again.");
        //                return View("Index");
        //            }
        //        }
        //        else
        //        {
        //            // Registration failed, show an error message or perform other actions
        //            ModelState.AddModelError("", "Registration failed. Please try again.");
        //            return View("Create");
        //        }
        //    }
        //}
        //// Create a class to represent the response from the registration API
        //public class RegistrationResponse
        //{
        //    public string message { get; set; }
        //    public myUser user { get; set; }

        //    public string token { get; set; }
        //}

        //[HttpGet]
        //// the url of this is https://localhost:7273/UserTables/VerifyEmail/{id}
        //public IActionResult VerifyEmail(string id)
        //{
        //    Console.WriteLine("id: " + id);
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    // create a HttpClient instance and post a VerifyToken to localhost:5000/users/verify
        //    using (var httpClient = new HttpClient())
        //    {
        //        // Set the Content-Type header to application/json
        //        httpClient.DefaultRequestHeaders.Accept.Clear();
        //        httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //        VerifyToken verifyToken = new VerifyToken();
        //        verifyToken.Token = id;
        //        var content = new StringContent(JsonSerializer.Serialize(verifyToken), Encoding.UTF8, "application/json");
        //        var response = httpClient.PostAsync("http://localhost:5000/users/verify", content).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            Console.WriteLine("response: good " + response);
        //        }
        //        else
        //        {
        //            Console.WriteLine("response: bad " + response);
        //        }

        //    }



        //    return RedirectToAction("VerifiedEmail", "NonFunctionalPages");
        //}



        //[HttpPost]
        //public async Task<IActionResult> MyLogin([Bind("UserEmail, UserPass")] UserTable u, string data, string cart)
        //{
        //    Console.WriteLine("data: " + data);
        //    ViewBag.returnUrl = data;
        //    ViewBag.cart = cart;
        //    // Base URL of the authentication API
        //    string apiBaseUrl = "http://localhost:5000"; // Replace with your API base URL

        //    // Create the login request payload
        //    var loginData = new
        //    {
        //        Username = u.UserEmail,
        //        Password = u.UserPass
        //    };

        //    // Serialize the loginData to JSON
        //    var jsonPayload = JsonSerializer.Serialize(loginData);

        //    // Create a HttpClient instance
        //    using (var httpClient = new HttpClient())
        //    {
        //        // Set the Content-Type header to application/json
        //        httpClient.DefaultRequestHeaders.Accept.Clear();
        //        httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //        // Create the HttpContent from the JSON payload
        //        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        //        // Send the POST request to the authentication API to get the token
        //        var response = await httpClient.PostAsync($"{apiBaseUrl}/Users/authenticate", content);

        //        // Check if the response indicates successful authentication
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Read the response content as a string
        //            string responseContent = await response.Content.ReadAsStringAsync();

        //            // Deserialize the JSON response to an object
        //            var responseObject = JsonSerializer.Deserialize<AuthenticationResponse>(responseContent);

        //            // Check if the token is not null or empty
        //            if (!string.IsNullOrEmpty(responseObject?.token))
        //            {
        //                // Store the token in local storage or session (choose an appropriate method for your frontend)
        //                // For example, in JavaScript:
        //                // localStorage.setItem('token', responseObject.token);

        //                // Perform token validation to ensure it is still valid
        //                bool isTokenValid = await ValidateToken(apiBaseUrl, responseObject.token);

        //                if (isTokenValid)
        //                {
        //                    // Redirect to the desired page or perform the desired action upon successful authentication
        //                    // For example, redirect to the user's dashboard:

        //                    //Check in with Jayden about token
        //                    /*int chipinId = 5;
        //                    ReturnChipinId returnChipin = new ReturnChipinId();
        //                    returnChipin.ChipinId = chipinId;
        //                    returnChipin.Signature = "TestSignature";
        //                    var jsonReturnChipin = new StringContent(JsonSerializer.Serialize(returnChipin), Encoding.UTF8, "application/json");*/
        //                    //var ProductData = httpClient.PostAsync(data, jsonReturnChipin);


        //                    return RedirectToAction("LoginSuccess", new { token = responseObject.token, data = ViewBag.returnUrl, cart = ViewBag.cart });

        //                }
        //                else
        //                {
        //                    // Token is no longer valid, handle accordingly (e.g., redirect to login page or show an error message)
        //                    ModelState.AddModelError("", "Token is no longer valid. Please log in again.");
        //                    return View("Index");
        //                }
        //            }
        //            else
        //            {
        //                // Authentication failed or the response does not match the expected format, show an error message or perform other actions
        //                ModelState.AddModelError("", "Authentication failed. Please check your username and password.");
        //                return View("Index");
        //            }
        //        }
        //        else
        //        {
        //            // Authentication request failed, show an error message or perform other actions
        //            ModelState.AddModelError("", "Authentication request failed. Please try again later.");
        //            return View("Index");
        //        }
        //    }
        //}

        //// Create a class to represent the response from the authentication API
        //public class AuthenticationResponse
        //{
        //    public string token { get; set; }
        //    public myUser user { get; set; }
        //}

        //// Create a class to represent the user information returned by the API
        //public class myUser
        //{
        //    public int chipinId { get; set; }
        //    public string chipinName { get; set; }
        //    public DateTime chipinCreatedDate { get; set; }
        //    public int? tokenWalletId { get; set; }
        //    public string userEmail { get; set; }
        //}


        //// Method to validate the token
        //private async Task<bool> ValidateToken(string apiBaseUrl, string token)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        // Set the Authorization header with the token
        //        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        //        // Send the GET request to validate the token
        //        var response = await httpClient.GetAsync($"{apiBaseUrl}/Users/validate_token");

        //        // Check if the response indicates the token is valid
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Read the response content as a string
        //            string responseContent = await response.Content.ReadAsStringAsync();

        //            // Deserialize the JSON response to an object
        //            var responseObject = JsonSerializer.Deserialize<TokenValidationResponse>(responseContent);

        //            // Check if the token is valid based on the response message
        //            return responseObject?.message == "Token is valid";
        //        }

        //        // Token validation failed or the response does not match the expected format
        //        return false;
        //    }
        //}

        //// Create a class to represent the response from the token validation API
        //public class TokenValidationResponse
        //{
        //    public string message { get; set; }
        //}












        //public async Task<IActionResult> LoginSuccess(string token, string? data, string? cart)
        //{
        //    // Redirect to the "Index" action in the "ProductListWallets" controller with the "token" parameter

        //    //Get and print the viewmode
        //    if (ControllerContext.ActionDescriptor.Properties.TryGetValue("ViewMode", out var viewmode))
        //    {
        //        ViewBag.viewMode = viewmode;

        //    }

        //    //If no BillingAddress, redirect to Create BillingAddress


        //    /*if (ControllerContext.ActionDescriptor.Properties.TryGetValue("chipinId", out var chipinId))
        //    {
        //        var ba = _context.BillingAddresses.Where(b => b.ChipinId == int.Parse(chipinId.ToString())).FirstOrDefault();
        //        Console.WriteLine("ba: " + ba);
        //        if (ba == null)
        //        {

        //            return RedirectToAction("Create", "BillingAddresses", new { token = token});
        //        }
        //    }*/

        //    if (string.IsNullOrEmpty(data))
        //    {


        //        if (ControllerContext.ActionDescriptor.Properties.TryGetValue("chipinId", out var chipinId))
        //        {
        //            var ba = _context.BillingAddresses.Where(b => b.ChipinId == chipinId.ToString()).FirstOrDefault();
        //            Console.WriteLine("ba: " + ba);
        //            if (ba == null)
        //            {

        //                return RedirectToAction("onFirstLogin", "NonFunctionalPages", new { token = token });


        //            }
        //        }


        //        return RedirectToAction("Index", "ProductListWallets", new { token = token });

        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "ExternalAdditions", new { token = token, data = data, cart = cart });
        //    }

        //}



        //public async Task<IActionResult> ForgotPassword()
        //{
        //    // Redirect to the "Index" action in the "ProductListWallets" controller with the "token" parameter
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> ForgotPassword(string email)
        //{

        //    //Get the chipinId from the email

        //    string chipinId = _context.UserTables.Where(u => u.UserEmail == email).Select(u => u.ChipinId).FirstOrDefault();
        //    if (chipinId.Equals("0"))
        //    {
        //        return RedirectToAction("Error404", "NonFunctionalPages");
        //    }

        //    ResetPasswordLinkRequest resetPasswordLinkRequest = new ResetPasswordLinkRequest();
        //    resetPasswordLinkRequest.ChipinId = chipinId;
        //    resetPasswordLinkRequest.Email = email;


        //    //Send a post request to localhost:5000/Users/resetPasswordLink and store the response in a variable

        //    using (var httpClient = new HttpClient())
        //    {

        //        var result = await httpClient.PostAsync("http://localhost:5000/Users/resetPasswordLink", new StringContent(JsonSerializer.Serialize(resetPasswordLinkRequest), Encoding.UTF8, "application/json"));

        //        //get the response content. it contains a single string
        //        string link = await result.Content.ReadAsStringAsync();
        //        if (link == "Error")
        //        {
        //            return RedirectToAction("Error404", "NonFunctionalPages");
        //        }
        //        else
        //        {


        //            Email sendLink = new Email(email);
        //            bool sent = sendLink.SendMode1(link);
        //            if (sent)
        //            {
        //                return RedirectToAction("ForgotPassword", "NonFunctionalPages");
        //            }
        //            else
        //            {
        //                return RedirectToAction("Error404", "NonFunctionalPages");
        //            }

        //        }


        //    }

        //    return View();
        //}

        //[HttpGet]
        //public async Task<IActionResult> ResetPassword(string hash, int id)
        //{
        //    if (hash == null || id == 0)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else if (hash == "1")
        //    {
        //        return View("Index", "Home");
        //    }
        //    else
        //    {
        //        ViewBag.hash = hash;
        //        ViewBag.id = id;
        //        // Redirect to the "Index" action in the "ProductListWallets" controller with the "token" parameter
        //        return View(); // Password Reset Page
        //    }

        //}

        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(string hash, int id, string password, string confirmPassword)
        //{
        //    if (hash == null || id == 0)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else if (hash == "1")
        //    {
        //        return View("Index", "Home");
        //    }
        //    else
        //    {

        //        if (password != confirmPassword)
        //        {
        //            return RedirectToAction("ResetPassword", "Users", new { hash = hash, id = id });
        //        }

        //        using (var client = new HttpClient())
        //        {
        //            ResetPassword resetPassword = new ResetPassword();
        //            resetPassword.ChipinId = id;
        //            resetPassword.Password = password;
        //            resetPassword.Token = hash;

        //            var result = await client.PostAsync("http://localhost:5000/Users/resetPassword", new StringContent(JsonSerializer.Serialize(resetPassword), Encoding.UTF8, "application/json"));
        //            string response = await result.Content.ReadAsStringAsync();
        //            if (response == "Success")
        //            {

        //                return RedirectToAction("PasswordReset", "NonFunctionalPages");
        //            }
        //            else
        //            {
        //                return RedirectToAction("ResetPassword", "Users", new { hash = hash, id = id });
        //            }
        //        }

        //        // Redirect to the "Index" action in the "ProductListWallets" controller with the "token" parameter
        //        return View();
        //    }
        //}

    }

}
