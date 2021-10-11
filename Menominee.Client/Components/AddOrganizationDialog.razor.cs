using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Services;
using Microsoft.AspNetCore.Components;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System.Threading.Tasks;

namespace Menominee.Client.Components
{
    public partial class AddOrganizationDialog
    {
        private OrganizationAddProperties OrganizationAdd { get; set; }
        private OrganizationToAdd Organization { get; set; }
        public string Message { get; set; }
        public EntityType EntityType { get; set; }
        protected OrganizationNameForm OrganizationNameForm { get; set; }
        protected AddressForm AddressForm { get; set; }

        [Inject]
        public IOrganizationDataService OrganizationDataService { get; set; }
        public bool ShowDialog { get; set; }

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public void Show()
        {
            ResetDialog();
            ShowDialog = true;
            StateHasChanged();
        }

        public void Close()
        {
            ShowDialog = false;
            StateHasChanged();
        }

        private void ResetDialog()
        {
            OrganizationAdd = new OrganizationAddProperties();
        }

        protected async Task HandleValidSubmit()
        {
            Message = string.Empty;

            if (FormIsValid())
            {
                Organization = new OrganizationToAdd
                {
                    Name = OrganizationAdd.Name.Name
                };

                await OrganizationDataService.AddOrganization(Organization);
                ShowDialog = false;

                await CloseEventCallback.InvokeAsync(true);
                StateHasChanged();
            }

            else
            {
                // TODO: Alert user that form is invalid
                Message = "Please complete all required items";
            }
        }

        private bool FormIsValid()
        {
            if (OrganizationAdd.Name != null)
                return true;

            return false;
        }

        public void OrganizationNameForm_OnOrganizationNameChanged()
        {
            OrganizationAdd.Name = OrganizationNameForm.OrganizationName;
            StateHasChanged();
        }

        public void AddressForm_OnAddressChanged()
        {
            OrganizationAdd.Address = AddressForm.EntityAddress;
            StateHasChanged();
        }

        private class OrganizationAddProperties
        {
            internal OrganizationName Name { get; set; }
            internal AddressToAdd Address { get; set; }
            internal virtual PersonToAdd Contact { get; set; }
            internal string Note { get; set; }
        }
    }

}

