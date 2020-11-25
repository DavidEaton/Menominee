using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ClientApp.Shared
{
    public partial class NavMenu : ComponentBase
    {
        private string RegisterUrl = string.Empty;

        [Inject]
        public NavigationManager Navigation { get; set; }

        //[Inject]
        //public SignOutSessionStateManager SignOutManager { get; set; }

        [Inject]
        public IConfiguration Configuration { get; set; }

        protected override async Task OnInitializedAsync()
        {
            RegisterUrl = $"{Configuration.GetValue<string>($"OidcConfiguration:RegisterUrl")}";
        }

        private async Task BeginSignOut(MouseEventArgs args)
        {
            //await SignOutManager.SetSignOutState();
            Navigation.NavigateTo("authentication/logout");
        }
    }
}
