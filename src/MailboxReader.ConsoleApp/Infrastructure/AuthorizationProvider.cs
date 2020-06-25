using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace MailboxReader.ConsoleApp.Infrastructure
{
    internal class AuthorizationProvider
    {
        private readonly AppRegistrationSettings _appRegistrationSettings;

        public AuthorizationProvider(IOptions<AppRegistrationSettings> appRegistrationSettings)
        {
            _appRegistrationSettings = appRegistrationSettings.Value;
        }
        
        public IAuthenticationProvider Create()
        {
            var clientId = _appRegistrationSettings.ApplicationId;
            var clientSecret = _appRegistrationSettings.ApplicationSecret;
            var redirectUri = _appRegistrationSettings.RedirectUri;
            var authority = $"https://login.microsoftonline.com/{_appRegistrationSettings.TenantId}/v2.0";

            List<string> scopes = new List<string>
            {
                "https://graph.microsoft.com/.default"
            };

            var cca = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithAuthority(authority)
                .WithRedirectUri(redirectUri)
                .WithClientSecret(clientSecret)
                .Build();
            return new MsalAuthenticationProvider(cca, scopes.ToArray());
        }
    }
}