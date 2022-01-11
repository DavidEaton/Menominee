using MenomineePlayWASM.Shared.Entities.Settings;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MenomineePlayWASM.Shared.Entities;
using System.Net.Http;
using System.Net.Http.Json;

namespace MenomineePlayWASM.Client.Pages.Settings
{
    public partial class CompanyInfo
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        //[Parameter]
        //public EventCallback<int> ResetContent { get; set; }

        //private async Task SaveChanges()
        //{
        //    SelectedContent = 0;
        //    await ResetContent.InvokeAsync();
        //}

        //private async Task DiscardChanges()
        //{
        //    SelectedContent = 0;
        //    await ResetContent.InvokeAsync();
        //}

        private void SaveChanges()
        {
            navigationManager.NavigateTo("/settings");
        }

        private void DiscardChanges()
        {
            navigationManager.NavigateTo("/settings");
        }

        public CompanyInformation Company { get; set; }

        public State[] States { get; set; }

        protected override async Task OnInitializedAsync()
        {
            States = await HttpClient.GetFromJsonAsync<State[]>("sample-data/states.json");
            Company = await HttpClient.GetFromJsonAsync<CompanyInformation>("sample-data/companyInfo.json");
        }
    }
}
