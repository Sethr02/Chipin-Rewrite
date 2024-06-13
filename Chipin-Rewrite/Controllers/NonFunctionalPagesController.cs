using Microsoft.AspNetCore.Mvc;

namespace Chipin_Rewrite.Controllers
{
    public class NonFunctionalPagesController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return await Error404();
        }

        public async Task<IActionResult> Error404()
        {
            ViewBag.Title = "404";
            ViewBag.ErrorType = "ERROR";
            ViewBag.ErrorCode = "404";
            ViewBag.ErrorMessage = "Client side issue: Bad Request response";
            ViewBag.ReturnAction = "Index";
            ViewBag.ReturnController = "UserTables";
            ViewBag.btnText = "Return to Home";
            return View("Index");
        }
        public async Task<IActionResult> Error500()
        {
            ViewBag.Title = "500";
            ViewBag.ErrorType = "ERROR";
            ViewBag.ErrorCode = "500";
            ViewBag.ErrorMessage = "Internal Server Error: Server encountered an unexpected condition that prevented it from fulfilling the request. Please try again later";
            ViewBag.ReturnAction = "Index";
            ViewBag.ReturnController = "UserTables";
            ViewBag.btnText = "Return to Home";
            return View("Index");
        }

        public async Task<IActionResult> ErrorNoBillingAddress()
        {
            ViewBag.Title = "ERROR";
            ViewBag.ErrorType = "ERROR";
            ViewBag.ErrorCode = "418";
            ViewBag.ErrorMessage = "No Billing Address selected. Please select a billing address before continuing";
            ViewBag.ReturnAction = "Create";
            ViewBag.ReturnController = "BillingAddresses";
            ViewBag.btnText = "Create Billing Address";
            return View("Index");
        }
        public async Task<IActionResult> ErrorNoShippingAddress()
        {
            ViewBag.Title = "ERROR";
            ViewBag.ErrorType = "ERROR";
            ViewBag.ErrorCode = "418";
            ViewBag.ErrorMessage = "No Shipping Address selected. Please select a shipping address before continuing";
            ViewBag.ReturnAction = "Create";
            ViewBag.ReturnController = "ShippingAddresses";
            ViewBag.btnText = "Return to Home";
            return View("Index");
        }

        public async Task<IActionResult> ErrorProduct(string? name)
        {
            ViewBag.Title = "ERROR";
            ViewBag.ErrorType = "ERROR";
            ViewBag.ErrorCode = "418";
            ViewBag.ErrorMessage = "Product(" + name + ") not found: Please remove from list.";
            ViewBag.ReturnAction = "Index";
            ViewBag.ReturnController = "ProductListWallets";
            ViewBag.btnText = "Return to Lists";
            return View("Index");
        }

        public async Task<IActionResult> ErrorNoStock(string? name)
        {
            ViewBag.Title = "ERROR";
            ViewBag.ErrorType = "ERROR";
            ViewBag.ErrorCode = "418";
            ViewBag.ErrorMessage = "Product(" + name + ") out of stock: Please remove item from list.";
            ViewBag.ReturnAction = "Index";
            ViewBag.ReturnController = "ProductListWallets";
            ViewBag.btnText = "Return to Lists";
            return View("Index");
        }

        public async Task<IActionResult> ErrorInsufficientFunds()
        {
            ViewBag.Title = "ERROR";
            ViewBag.ErrorType = "ERROR";
            ViewBag.ErrorCode = "418";
            ViewBag.ErrorMessage = "Insufficient funds: Please load funds to proceed";
            ViewBag.ReturnAction = "Index";
            ViewBag.ReturnController = "ProductListWallets";
            ViewBag.btnText = "Return to Lists";
            return View("Index");
        }

        public async Task<IActionResult> PurchaseSuccess()
        {
            ViewBag.Title = "SUCCESS";
            ViewBag.ErrorType = "";
            ViewBag.ErrorCode = "Purchase successful";
            ViewBag.ErrorMessage = "Your list has been fully paid for ";
            ViewBag.ReturnAction = "Index";
            ViewBag.ReturnController = "ProductListWallets";
            ViewBag.btnText = "Return to Lists";
            return View("Index");
        }

        public async Task<IActionResult> PurchaseFailure()
        {
            ViewBag.Title = "ERROR";
            ViewBag.ErrorType = "ERROR";
            ViewBag.ErrorCode = "418";
            ViewBag.ErrorMessage = "Purchase failed: Please try again later";
            ViewBag.ReturnAction = "Index";
            ViewBag.ReturnController = "ProductListWallets";
            ViewBag.btnText = "Return to Lists";
            return View("Index");
        }

        public async Task<IActionResult> VerifiedEmail()
        {
            ViewBag.Title = "SUCCESS";
            ViewBag.ErrorType = "Success";
            ViewBag.ErrorCode = "Your email has been verified";
            ViewBag.ErrorMessage = "Email verification successful";
            ViewBag.ReturnAction = "Index";
            ViewBag.ReturnController = "UserTables";
            return View("Index");
        }

        public async Task<IActionResult> PasswordReset()
        {
            ViewBag.Title = "SUCCESS";
            ViewBag.ErrorType = "Success";
            ViewBag.ErrorCode = "Your password has been reset";
            ViewBag.ErrorMessage = "Password reset successful";
            ViewBag.ReturnAction = "Index";
            ViewBag.ReturnController = "UserTables";
            return View("Index");
        }

        public async Task<IActionResult> ForgotPassword()
        {
            ViewBag.Title = "SUCCESS";
            ViewBag.ErrorType = "Success";
            ViewBag.ErrorCode = "An email has been sent";
            ViewBag.ErrorMessage = "Please click the link in the email to reset your password";
            ViewBag.ReturnAction = "Index";
            ViewBag.ReturnController = "UserTables";
            return View("Index");
        }

        public async Task<IActionResult> VerifyEmailRequest()
        {

            ViewBag.Title = "SUCCESS";
            ViewBag.ErrorCode = "Success";
            ViewBag.ErrorMessage = "Confirmation email sent. Kindly click the link within the email to verify your email address.";
            ViewBag.ReturnAction = "Index";
            ViewBag.ReturnController = "UserTables";
            ViewBag.btnText = "Return";
            return View("Index");
        }


        public async Task<IActionResult> onFirstLogin(string? token)
        {
            ViewBag.Token = token;
            ViewBag.Title = "Login";
            ViewBag.ErrorCode = "Login Successful";
            ViewBag.ErrorMessage = "Kindly proceed to add a billing address to start listing!";
            ViewBag.ReturnAction = "billingredirect";
            ViewBag.ReturnController = "NonFunctionalPages";
            ViewBag.btnText = "Proceed";
            return View("Index");
        }

        public async Task<IActionResult> billingredirect(string? token)
        {

            return RedirectToAction("Create", "BillingAddresses", new { token = token, onLogin = true });

        }








    }
}
