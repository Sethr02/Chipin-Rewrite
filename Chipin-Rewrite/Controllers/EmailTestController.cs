using Chipin_Rewrite.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chipin_Rewrite.Controllers
{
    public class EmailTestController : Controller
    {
        private readonly OdooGoogleEmailService _emailService;

        public EmailTestController(OdooGoogleEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        [Route("sendtestemail")]
        public async Task<IActionResult> SendEmail()
        {
            await _emailService.SendEmailAsync("Sethramsamy1@gmail.com", "dcdcdcd", "ccdccdcdcd");
            return Ok("Email Sent");
        }
    }
}
