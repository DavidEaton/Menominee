using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Menominee.Client.Pages.Settings
{
    public partial class CustomSettings
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        //public CustomSettings customSettings { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //customSettings = await HttpClient.GetFromJsonAsync<CustomSettings>("sample-data/customSettings.json");
        }

        //private async Task SaveChanges()
        private void SaveChanges()
        {
            navigationManager.NavigateTo("/settings");
        }

        private void DiscardChanges()
        {
            navigationManager.NavigateTo("/settings");
        }
    }
}
