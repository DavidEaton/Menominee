using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Menominee.Client.MessageHandlers
{
    /// <summary>
    /// Base class AuthorizationMessageHandler takes care of renewing tokens
    /// </summary>
    public class MenonineeApiAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public IConfiguration configuration;

        public MenonineeApiAuthorizationMessageHandler(
            IAccessTokenProvider provider,
            NavigationManager navigation,
            IConfiguration configuration)
            : base(provider, navigation)
        {
            this.configuration = configuration;
            ConfigureHandler(
                authorizedUrls: new[] { configuration["ApiBaseUrl"] });
        }
    }
}
