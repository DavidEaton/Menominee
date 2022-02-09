using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Menominee.OrganizationDataContracts.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.OrganizationDataContracts.Pages
{
    public partial class OrganizationsIndex : ComponentBase
    {
        [Inject]
        public IOrganizationDataService OrganizationDataService { get; set; }

        [Inject]
        public ILogger<OrganizationsIndex> Logger { get; set; }


        public IReadOnlyList<OrganizationToReadInList> OrganizationsList;
        public long Id { get; set; }

        private bool EditingOrganization { get; set; } = false;
        private bool AddingOrganization { get; set; } = false;
        private bool EnableEditor { get; set; } = false;

        private OrganizationToWrite organization { get; set; }
        protected override async Task OnInitializedAsync()
        {
            OrganizationsList = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }

        //Edit
        private async Task HandleSelectedOrganizationAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as OrganizationToReadInList).Id;
            EditingOrganization = true;
            OrganizationsList = null;

            var readDto = await OrganizationDataService.GetOrganization(Id);
            organization = new OrganizationToWrite
            {
                Name = readDto.Name,
                Note = readDto.Note
            };

            if (readDto.Address != null)
            {
                organization.Address = new AddressToWrite
                {
                    AddressLine = readDto.Address.AddressLine,
                    City = readDto.Address.City,
                    State = readDto.Address.State,
                    PostalCode = readDto.Address.PostalCode
                };
            }

            if (readDto?.Emails.Count > 0)
            {
                foreach (var email in readDto.Emails)
                {
                    organization.Emails.Add(new EmailToWrite
                    {
                        Address = email.Address,
                        IsPrimary = email.IsPrimary
                    });
                }
            }

            if (readDto?.Phones.Count > 0)
            {
                foreach (var phone in readDto.Phones)
                {
                    organization.Phones.Add(new PhoneToWrite
                    {
                        Number = phone.Number,
                        PhoneType = Enum.Parse<PhoneType>(phone.PhoneType),
                        IsPrimary = phone.IsPrimary
                    });
                }
            }
        }

        private void Add()
        {
            AddingOrganization = true;
            OrganizationsList = null;
            organization = new();
        }

        protected async Task SubmitForm()
        {
            if (AddingOrganization)
                await OrganizationDataService.AddOrganization(organization);

            if (EditingOrganization)
                await OrganizationDataService.UpdateOrganization(organization, Id);

            await CloseEditorAsync();
        }

        private async Task CloseEditorAsync()
        {
            EditingOrganization = false;
            AddingOrganization = false;
            OrganizationsList = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }

        private void AddAddress()
        {
            organization.Address = new();
            EnableEditor = true;
            //Modal.Show<AddressEditor>("Welcome to Blazored Modal");
        }

        private void SaveAddress()
        {
            EnableEditor = false;
        }
        private void CancelAddress()
        {
            //if (organizationToWrite.Address != null && AddingAddress)
            //    organizationToWrite.Address = null;
            EnableEditor = false;
        }

        private void OrganizationEditorUpdated()
        {
            // Organization Validation WILL occur without this call:
            //editContext.Validate();
        }
        private void EditorUpdated()
        {
            // Organization Address Validation will not occur without this call:
            var editContext = new EditContext(organization);
            editContext.Validate();
        }
    }
}