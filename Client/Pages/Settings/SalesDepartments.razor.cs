using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Pages.Settings
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
        public CustomSettings customSettings { get; set; }

        protected override async Task OnInitializedAsync()
        {
            customSettings = await HttpClient.GetFromJsonAsync<CustomSettings>("sample-data/customSettings.json");
        }
    }
}
