using CustomerVehicleManagement.Shared.Models;
using Menominee.OrganizationDtos.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.OrganizationDtos.Pages.Organization
{
    public partial class OrganizationIndex : ComponentBase
    {
        [Inject]
        public IOrganizationDataService OrganizationsDataService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        public ILogger<OrganizationIndex> Logger { get; set; }

        public IReadOnlyList<OrganizationInListDto> OrganizationsList;

        protected override async Task OnInitializedAsync()
        {
            OrganizationsList = (await OrganizationsDataService.GetAllOrganizations()).ToList();
        }

        private void Edit(long id)
        {
            NavigationManager.NavigateTo($"/organization/edit/{id}");
        }
    }
}