using CustomerVehicleManagement.Shared.Models;
using Menominee.OrganizationDtos.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Menominee.OrganizationDtos.Pages.Organization
{
    public partial class OrganizationEdit : ComponentBase
    {
        [Inject]
        public IOrganizationDataService OrganizationDataService { get; set; }

        [Parameter]
        public long Id { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        protected OrganizationUpdateDto Organization { get; set; }
        public string Message { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var readDto = await OrganizationDataService.GetOrganization(Id);

            Organization = new OrganizationUpdateDto
            {
                Name = readDto.Name,
                Note = readDto.Note
            };

            if (readDto.Address != null)
            {
                Organization.Address = new AddressUpdateDto
                {
                    AddressLine = readDto.Address.AddressLine,
                    City = readDto.Address.City,
                    State = (readDto.Address.State),
                    PostalCode = readDto.Address.PostalCode
                };
            }
        }

        protected async Task HandleValidSubmit()
        {
            Message = string.Empty;

            if (FormIsValid())
            {
                await OrganizationDataService.UpdateOrganization(Organization, Id);
                Close();
            }

            else
            {
                Message = "Please complete all required items";
            }
        }

        protected async Task HandleInvalidSubmit()
        {
            Message = "Please complete all required items";
        }

        public void Close()
        {
            StateHasChanged();
            NavigationManager.NavigateTo("/organizations");
        }

        private bool FormIsValid()
        {
            if (Organization.Name != null)
                return true;

            return false;
        }

    }
}
