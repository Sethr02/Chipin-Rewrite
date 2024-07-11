using Chipin_Rewrite.Configuration;
using Chipin_Rewrite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chipin_Rewrite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        IMailService Mail_Service = null;
        //injecting the IMailService into the constructor
        public MailController(IMailService _MailService)
        {
            Mail_Service = _MailService;
        }
        [HttpPost]
        public bool SendMail(MailData Mail_Data)
        {
            return Mail_Service.SendMail(Mail_Data);
        }
    }
}
