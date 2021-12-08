using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Menominee.OrganizationDataContracts.Services;
using Microsoft.AspNetCore.Components;
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
        private bool EditingAddress { get; set; } = false;
        private bool AddingAddress { get; set; } = false;
        private bool AddressDialogVisible => organizationToWrite?.Address != null && (AddingAddress || EditingAddress);

        private OrganizationToWrite organizationToWrite { get; set; }
        protected override async Task OnInitializedAsync()
        {
            OrganizationsList = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }

        private async Task HandleSelectedOrganizationAsync(GridRowClickEventArgs args)
        {
            EditingOrganization = true;
            OrganizationsList = null;

            var readDto = await OrganizationDataService.GetOrganization((args.Item as OrganizationToReadInList).Id);
            organizationToWrite = new OrganizationToWrite
            {
                Name = readDto.Name,
                Note = readDto.Note
            };

            if (readDto.Address != null)
            {
                organizationToWrite.Address = new AddressToWrite
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
                    organizationToWrite.Emails.Add(new EmailToWrite
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
                    organizationToWrite.Phones.Add(new PhoneToWrite
                    {
                        Number = phone.Number,
                        PhoneType = Enum.Parse<PhoneType>(phone.PhoneType),
                        IsPrimary = phone.IsPrimary
                    });
                }
            }
        }


        private void AddAddressAddingOrganization()
        {
            organizationToWrite.Address = new();
            AddingAddress = true;
        }

        private void AddAddressEditingOrganization()
        {
            organizationToWrite.Address = new();
            EditingAddress = true;
        }

        private void CancelAddAddress()
        {
            AddingAddress = false;

            if (organizationToWrite.Address != null)
                organizationToWrite.Address = null;
        }

        private void CancelEditAddress()
        {
            EditingAddress = false;
        }

        public void EditAddress()
        {
            EditingAddress = true;
        }

        private void Add()
        {
            AddingOrganization = true;
            OrganizationsList = null;
            organizationToWrite = new();
        }

        protected async Task HandleAddSubmit()
        {
            await OrganizationDataService.AddOrganization(organizationToWrite);
            await EndAddAsync();
        }

        protected async Task HandleUpdateSubmit()
        {
            if (!string.IsNullOrWhiteSpace(organizationToWrite.Name))
            {
                await OrganizationDataService.UpdateOrganization(organizationToWrite, Id);
                await EndEditAsync();
            }
        }

        protected async Task EndEditAsync()
        {
            EditingOrganization = false;
            OrganizationsList = (await OrganizationDataService.GetAllOrganizations()).ToList();
            EditingAddress = false;
        }

        protected async Task EndAddAsync()
        {
            AddingOrganization = false;
            OrganizationsList = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }
    }
}