using Blazored.Toast.Services;
using CustomerVehicleManagement.Shared.Models;
using Menominee.OrganizationDtos.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Menominee.OrganizationDtos.Pages.Organization
{
    public partial class OrganizationAdd : ComponentBase
    {
        [Inject]
        public IOrganizationDataService OrganizationsDataService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IToastService ToastService { get; set; }

        OrganizationAddDto Organization = new();
        public string Message { get; set; } = "Please complete all required items";

        protected async Task HandleValidSubmit()
        {
            if (FormIsValid())
            {
                await OrganizationsDataService.AddOrganization(Organization);
                Close();
            }

            else
            {
                ToastService.ShowError(Message);
            }
        }

        protected async Task HandleInvalidSubmit()
        {
            ToastService.ShowError(Message);
        }

        private bool FormIsValid()
        {
            if (Organization.Name != null)
                return true;

            return false;
        }
        public void Close()
        {
            StateHasChanged();
            NavigationManager.NavigateTo("/organizations");
        }
    }
}
