using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace ClientApp.Services
{
    public class ApiAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public IConfiguration configuration;

        public ApiAuthorizationMessageHandler(IAccessTokenProvider provider,
                                                       NavigationManager navigation,
                                                       IConfiguration configuration)
            : base(provider, navigation)
        {
            this.configuration = configuration;
            ConfigureHandler(authorizedUrls: new[] { configuration["ApiBaseUrl"] });
        }
    }
}
