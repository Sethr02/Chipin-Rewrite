namespace Chipin_Rewrite.Utility.Email
{
    using System;
    using System.Net;
    using System.Net.Mail;

    public class Email
    {
        // ... other methods and constructors ...
        private readonly string recipientEmail;

        public Email(string recipientEmail)
        {
            this.recipientEmail = recipientEmail;
        }

        public bool ListCreated(string listName, string listUrl)
        {
            string subject = "Your Chipin list has been created!";
            string message = $"Your Chipin list, {listName}, has been created! You can view it at {listUrl}";
            return SendEmail(subject, message);
        }

        public bool ListFullyFunded(string listName, string listUrl)
        {
            string subject = "Your Chipin list has been funded!";
            string message = $"Your Chipin list, {listName}, has been funded! You can view it at {listUrl}";
            return SendEmail(subject, message);
        }

        public bool ListPartiallyFunded(string listName, string listUrl, string fromName, string fromMessage, string fromEmail)
        {
            string subject = "Your Chipin list has been partially funded!";
            string message = $"Your Chipin list, {listName}, has partially been funded by {fromName}. You can view it at {listUrl} \n\n Their message to you: \n {fromMessage}";
            return SendEmail(subject, message);
        }

        public bool ListFundedConfirmation(string listName, string listUrl)
        {
            string subject = "You have contributed to a list!";
            string message = $"You have successfully funded the Chipin list {listName}. You can view it ar {listUrl}";
            return SendEmail(subject, message);
        }
        private bool SendEmail(string subject, string message)
        {
            string senderEmail = "support@markitevolution.com";
            string senderPassword = "D3ll$0n!cME!";
            string smtpServer = "smtp.markitevolution.com";
            int smtpPort = 587; // Use the appropriate SMTP port

            SmtpClient smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = false,
            };

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine($"Email sent to {recipientEmail}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
