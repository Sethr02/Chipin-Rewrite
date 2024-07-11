using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace Chipin_Rewrite.Controllers
{
    public class ShareByEmailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(string recipientName, string recipientEmail, string message)
        {
            /*try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

                var fromAddress = new MailAddress(userEmail, "Your Name");
                var toAddress = new MailAddress(recipientEmail, recipientName);
                const string fromPassword = "your-email-password";
                const string subject = "Invitation to ChipIn";
                string body = message;

                var smtp = new SmtpClient
                {
                    Host = "smtp.example.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var mailMessage = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(mailMessage);
                }

                return View();
            }
            catch (Exception ex)
            {
                // Handle the error
                ViewBag.ErrorMessage = "There was an error sending the email: " + ex.Message;
                return View("Error");
            }*/
            return View();
        }
    }
}
