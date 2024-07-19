using MailKit.Net.Smtp;
using MimeKit;

namespace Chipin_Rewrite.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }

    public class EmailService// : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /*public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            *//*//var client = new MailgunClient(_configuration["Mailgun:ApiKey"]);
            //var request = new MailgunMessage
            {
                From = _configuration["Mailgun:FromEmail"],
                To = { toEmail },
                Subject = subject,
                HtmlBody = message
            };*//*

            await client.SendMessageAsync(_configuration["Mailgun:Domain"], request);
        }*/
    }
}
