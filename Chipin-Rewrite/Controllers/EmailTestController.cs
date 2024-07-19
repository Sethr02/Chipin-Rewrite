using Chipin_Rewrite.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chipin_Rewrite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailTestController : Controller
    {
        private readonly IEmailService _emailService;

        public EmailTestController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (emailRequest == null || string.IsNullOrEmpty(emailRequest.ToEmail) || string.IsNullOrEmpty(emailRequest.Subject) || string.IsNullOrEmpty(emailRequest.Message))
            {
                return BadRequest("Invalid email request.");
            }

            await _emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Message);
            return Ok("Email sent successfully.");
        }

    }

    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
