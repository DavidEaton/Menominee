using CustomerVehicleManagement.Shared.Models;
using Menominee.OrganizationDtos.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.OrganizationDtos.Pages.Organization
{
    public partial class OrganizationIndex : ComponentBase
    {
        [Inject]
        public IOrganizationDataService DataService { get; set; }


        [Inject]
        public ILogger<OrganizationIndex> Logger { get; set; }

        public IReadOnlyList<OrganizationInListDto> OrganizationsList;
        public long Id { get; set; }

        private FormMode FormMode { get; set; }
        protected override async Task OnInitializedAsync()
        {
            FormMode = FormMode.Hidden;
            OrganizationsList = (await DataService.GetAllOrganizations()).ToList();
        }

        private void Edit(long id)
        {
            Id = id;
            FormMode = FormMode.Edit;
            OrganizationsList = null;
        }

        private void Add()
        {
            FormMode = FormMode.Add;
            OrganizationsList = null;
        }

        private async Task EditCompletedAsync(bool completed)
        {
            FormMode = FormMode.Hidden;
            //StateHasChanged();
            OrganizationsList = (await DataService.GetAllOrganizations()).ToList();
        }
    }
}