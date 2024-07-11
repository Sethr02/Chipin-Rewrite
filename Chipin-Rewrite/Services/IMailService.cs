using Chipin_Rewrite.Configuration;

namespace Chipin_Rewrite.Services
{
    public interface IMailService
    {
        bool SendMail(MailData Mail_Data);

    }
}
