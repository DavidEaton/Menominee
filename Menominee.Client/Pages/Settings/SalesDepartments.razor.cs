using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace Menominee.Client.Pages.Settings
{
    public partial class SalesDepartments
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        private void SaveChanges()
        {
            navigationManager.NavigateTo("/settings");
        }

        private void DiscardChanges()
        {
            navigationManager.NavigateTo("/settings");
        }


        // Placeholder content
        //public CustomSettings customSettings { get; set; }

        //protected override async Task OnInitializedAsync()
        //{
        //    customSettings = await HttpClient.GetFromJsonAsync<CustomSettings>("sample-data/customSettings.json");
        //}
    }
}
