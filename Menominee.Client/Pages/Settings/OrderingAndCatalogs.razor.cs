using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace Menominee.Client.Pages.Settings
{
    public partial class OrderingAndCatalogs
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
        //    await ResetContent.InvokeAsync(1);
        //}

        //private async Task DiscardChanges()
        //{
        //    await ResetContent.InvokeAsync(0);
        //}


        private bool isAllDataChecked = false;
        private bool isIdentifixChecked = false;
        private bool isMVConnectChecked = false;
        private bool isChecked = false;
    }
}
