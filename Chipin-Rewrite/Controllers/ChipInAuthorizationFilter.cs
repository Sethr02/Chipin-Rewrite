using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace Chipin_Rewrite.Controllers
{
    public class ChipInAuthorizationFilter : Attribute, IAsyncActionFilter
    {
        private readonly HttpClient _httpClient;

        public ChipInAuthorizationFilter()
        {
            _httpClient = new HttpClient();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("token", out var mytoken))
            {


                context.ActionDescriptor.Properties["ViewMode"] = "LoggedOut";
                context.ActionDescriptor.Properties["Token"] = mytoken;
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var decodedToken = tokenHandler.ReadJwtToken((string)mytoken);

                    var username = decodedToken.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
                    var userId = decodedToken.Claims.FirstOrDefault(c => c.Type == "chipin_id")?.Value;

                    context.ActionDescriptor.Properties["chipinId"] = userId;




                    // Send GET request to validate the token
                    var validationUrl = "http://localhost:5000/Users/validate_token";

                    // Add the token to the Authorization header
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)mytoken);

                    var response = await _httpClient.GetAsync(validationUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        context.ActionDescriptor.Properties["isTokenValid"] = true;
                        context.ActionDescriptor.Properties["ViewMode"] = "LoggedIn";
                    }
                    else
                    {
                        context.ActionDescriptor.Properties["isTokenValid"] = false;
                        context.ActionDescriptor.Properties["ViewMode"] = "LoggedOut";
                    }
                }
                catch (Exception ex)
                {
                    //  context.ActionDescriptor.Properties["chipinId"] = -1;
                    context.ActionDescriptor.Properties["ViewMode"] = "LoggedOut";
                }

                // Call the next delegate in the pipeline to continue with the action execution
                await next();
            }
            else
            {
                context.ActionDescriptor.Properties["ViewMode"] = "LoggedOut";
                await next();
            }


        }
    }
}
