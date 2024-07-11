using AspNetCoreRateLimit;
using Chipin_Rewrite.Configuration;
using Chipin_Rewrite.Models.Entities;
using Chipin_Rewrite.Services;
using Chipin_Rewrite.Utility.CallPay;
using Chipin_Rewrite.Utility.Encryption;
using Chipin_Rewrite.Utility.Payfast;
using Chipin_Rewrite.Utility.Signatures;
using Chipin_Rewrite.Utility.ThirdPartyReturns;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.Bind("AzureAdB2C", options);
        /* options.Events = new OpenIdConnectEvents
         {

             OnRedirectToIdentityProvider = context =>
             {
                 // Store the original URL in the state parameter
                 context.ProtocolMessage.State = context.Properties.RedirectUri;
                 return Task.CompletedTask;
             },
             OnAuthorizationCodeReceived = context =>
             {
                 // Retrieve the original URL from the state parameter
                 string originalUrl = context.ProtocolMessage.State;
                 string redirectUri = $"/UserTables/userSignInSignUp?redirectUrl={originalUrl}";


                 context.TokenEndpointRequest.RedirectUri = redirectUri;
                 return Task.CompletedTask;
             },
             OnSignedOutCallbackRedirect = context =>
             {
                 context.Response.Redirect("/");
                 context.HandleResponse();
                 return Task.CompletedTask;
             }
         };*/
        /*options.Events.OnTicketReceived = context =>
        {
            // This sets the return URL to a specific page
            context.ReturnUri = "/UserTables/userSignInSignUp";

            return Task.CompletedTask;
        };
        options.Events.OnSignedOutCallbackRedirect = context =>
        {
            context.Response.Redirect("/");
            context.HandleResponse();

            return Task.CompletedTask;
        };*/
        options.Events = new OpenIdConnectEvents
        {
            OnRedirectToIdentityProvider = context =>
            {
                // Store the original URL in the AuthenticationProperties
                context.Properties.Items["original_redirect_uri"] = context.Properties.RedirectUri;
                return Task.CompletedTask;
            },
            OnTicketReceived = context =>
            {
                // Retrieve the original URL from the AuthenticationProperties
                if (context.Properties.Items.TryGetValue("original_redirect_uri", out var originalUrl))
                {
                    string redirectUri = originalUrl;
                    context.ReturnUri = redirectUri;
                }
                else
                {
                    context.ReturnUri = "/UserTables/userSignInSignUp";
                }

                return Task.CompletedTask;
            },
            OnSignedOutCallbackRedirect = context =>
            {
                context.Response.Redirect("/");
                context.HandleResponse();
                return Task.CompletedTask;
            }
        };

    });

builder.Services.AddLogging();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<OdooGoogleEmailService>();
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();
builder.Services.AddDbContext<ChipinDbContext>(ServiceLifetime.Scoped);
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IExternalProductReturns, ExternalProductReturns>();
builder.Services.AddScoped<ISignatureGenerator, SignatureGenerator>();
builder.Services.AddScoped<IPayFastService, PayFastService>();
builder.Services.AddScoped<ICallPayService, CallPayService>();
builder.Services.AddDistributedMemoryCache();

// Configure rate limiting
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Limit = 5, // requests per second
            Period = "s" // "s" stands for seconds
        }
    };
});

builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://yourfrontendurl.com")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});

builder.Services.AddHttpContextAccessor(); // Add this line

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home_/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home_}/{action=Index}/");
app.MapRazorPages();
app.Run();
