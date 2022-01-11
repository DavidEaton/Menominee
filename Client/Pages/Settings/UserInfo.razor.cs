using MenomineePlayWASM.Shared.Entities;
using MenomineePlayWASM.Shared.Entities.Settings;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Pages.Settings
{
    public partial class UserInfo
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

        //[Parameter]
        //public int SelectedContent { get; set; }

        //[Parameter]
        //public EventCallback<int> ResetContent { get; set; }

        //private async Task SaveChanges()
        //{
        //    //SelectedContent = 1;
        //    await ResetContent.InvokeAsync(1);
        //}

        //private async Task DiscardChanges()
        //{
        //    //SelectedContent = 0;
        //    await ResetContent.InvokeAsync(0);
        //}

        public CompanyInformation Company { get; set; }
        public State[] States { get; set; }

        protected override async Task OnInitializedAsync()
        {
            States = await HttpClient.GetFromJsonAsync<State[]>("sample-data/states.json");
            Company = await HttpClient.GetFromJsonAsync<CompanyInformation>("sample-data/companyInfo.json");
        }
    }
}
