using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Components;
using Menominee.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Pages
{
    public partial class Organizations : ComponentBase
    {
        [Inject]
        public IOrganizationDataService OrganizationsDataService { get; set; }

        [Inject]
        public ILogger<Organizations> Logger { get; set; }

        public IReadOnlyList<OrganizationToReadInList> OrganizationsList;
        public long SelectedId { get; set; }

        protected AddOrganizationDialog AddOrganizationDialog { get; set; }
        protected OrganizationDetail OrganizationDetail { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Logger.LogInformation("Organizations.OnInitializedAsync()");
            OrganizationsList = (await OrganizationsDataService.GetAllOrganizations()).ToList();
        }

        protected void AddOrganization()
        {
            OrganizationDetail?.Close();
            AddOrganizationDialog.Show();
        }

        public async void AddOrganizationDialog_OnDialogClose()
        {
            OrganizationsList = (await OrganizationsDataService.GetAllOrganizations()).ToList();
            StateHasChanged();
        }
        private void SetSelectedId(long id)
        {
            SelectedId = id;
        }
    }

}
