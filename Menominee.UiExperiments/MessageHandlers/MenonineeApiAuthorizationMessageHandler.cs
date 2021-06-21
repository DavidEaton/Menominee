using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;

namespace Menominee.UiExperiments.MessageHandlers
{
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
            ConfigureHandler(authorizedUrls: new[] { configuration["ApiBaseUrl"] });
        }
    }
}
