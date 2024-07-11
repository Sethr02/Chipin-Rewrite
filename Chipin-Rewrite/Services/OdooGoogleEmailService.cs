using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using MailKit.Net.Smtp;
using MimeKit;

namespace Chipin_Rewrite.Services
{
    public class OdooGoogleEmailService
    {
        private readonly string[] Scopes = { "https://www.googleapis.com/auth/gmail.send" };

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var credential = await GetGoogleCredentialAsync();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Seth", "sethramsamy100@gmail.com"));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("sethramsamy100@gmail.com", credential.Token.AccessToken);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        private async Task<UserCredential> GetGoogleCredentialAsync()
        {
            var credPath = Path.Combine(Directory.GetCurrentDirectory(), "Credentials", "client_secrets.json");
            using (var stream = new FileStream(credPath, FileMode.Open, FileAccess.Read))
            {
                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore("token.json", true));

                return credential;
            }
        }
    }
}
