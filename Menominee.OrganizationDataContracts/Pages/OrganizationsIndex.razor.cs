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

        //Edit
        private async Task HandleSelectedOrganizationAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as OrganizationToReadInList).Id;
            EditingOrganization = true;
            OrganizationsList = null;

            var readDto = await OrganizationDataService.GetOrganization(Id);
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

        private void Add()
        {
            AddingOrganization = true;
            OrganizationsList = null;
            organizationToWrite = new();
        }

        protected async Task Save()
        {
            if (AddingOrganization)
                await OrganizationDataService.AddOrganization(organizationToWrite);

            if (EditingOrganization)
                await OrganizationDataService.UpdateOrganization(organizationToWrite, Id);

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
            organizationToWrite.Address = new();
            AddingAddress = true;
        }

        private void SaveAddress()
        {
            AddingAddress = false;
            EditingAddress = false;
        }

        private void CancelEditAddress()
        {
            if (organizationToWrite.Address != null && AddingAddress)
                organizationToWrite.Address = null;

            AddingAddress = false;
            EditingAddress = false;
        }
    }
}