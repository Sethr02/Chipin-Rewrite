using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace Chipin_Rewrite.Services
{
    public class OdooEmailService
    {
        private readonly string _odooUrl;
        private readonly string _dbName;
        private readonly string _username;
        private readonly string _password;
        private readonly int _userId;

        public OdooEmailService(string odooUrl, string dbName, string username, string password)
        {
            _odooUrl = odooUrl;
            _dbName = dbName;
            _username = username;
            _password = password;
            _userId = Authenticate().Result;
        }

        private async Task<int> Authenticate()
        {
            using (var client = new HttpClient())
            {
                var request = new
                {
                    jsonrpc = "2.0",
                    method = "call",
                    @params = new
                    {
                        db = _dbName,
                        login = _username,
                        password = _password
                    },
                    id = Guid.NewGuid().ToString()
                };

                var jsonRequest = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync($"{_odooUrl}/web/session/authenticate", content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    var responseObject = JsonConvert.DeserializeObject<JObject>(responseString);
                    if (responseObject["result"] != null)
                    {
                        return responseObject["result"]["uid"].Value<int>();
                    }
                    else
                    {
                        var error = responseObject["error"];
                        if (error != null)
                        {
                            throw new Exception($"Odoo authentication error: {error["message"]}");
                        }
                        else
                        {
                            throw new Exception("Unknown error during Odoo authentication.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    Console.WriteLine($"Request: {jsonRequest}");
                    throw new Exception("Failed to authenticate with Odoo", ex);
                }
            }
        }

        public async Task SendEmail(string to, string subject, string body)
        {
            using (var client = new HttpClient())
            {
                var request = new
                {
                    jsonrpc = "2.0",
                    method = "call",
                    @params = new
                    {
                        model = "mail.mail",
                        method = "create",
                        args = new[]
                        {
                        new
                        {
                            email_to = to,
                            subject = subject,
                            body_html = body,
                            email_from = _username
                        }
                    },
                        kwargs = new { }
                    },
                    id = Guid.NewGuid().ToString()
                };

                var jsonRequest = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{_odooUrl}/jsonrpc", content);
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<JObject>(responseString);

                if (responseObject["result"] == null)
                {
                    throw new Exception("Failed to send email through Odoo");
                }
            }
        }
    }
}
