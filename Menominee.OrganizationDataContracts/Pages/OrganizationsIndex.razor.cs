using CustomerVehicleManagement.Shared.Models;
using Menominee.OrganizationDataContracts.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        private OrganizationToAdd OrganizationToAdd;
        private OrganizationToEdit OrganizationToEdit;

        protected override async Task OnInitializedAsync()
        {
            OrganizationsList = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }

        private void AddAddress()
        {
            OrganizationToAdd.Address = new();
            AddingAddress = true;
        }

        private void CancelAddAddress()
        {
            OrganizationToAdd.Address = null;
            AddingAddress = false;
        }

        private void CancelEditAddress()
        {
            EditingAddress = false;
        }

        public void EditAddress()
        {
            EditingAddress = true;
        }

        private async Task EditAsync(long id)
        {
            Id = id;
            EditingOrganization = true;
            OrganizationsList = null;

            var readDto = await OrganizationDataService.GetOrganization(id);
            OrganizationToEdit = new OrganizationToEdit
            {
                Name = readDto.Name,
                Note = readDto.Note
            };

            if (readDto.Address != null)
            {
                OrganizationToEdit.Address = new AddressToEdit
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
                    OrganizationToEdit.Emails.Add(new EmailToEdit
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
                    OrganizationToEdit.Phones.Add(new PhoneToEdit
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
            OrganizationToAdd = new();
        }

        protected async Task HandleAddSubmit()
        {
            if (!string.IsNullOrWhiteSpace(OrganizationToAdd.Name))
            {
                await OrganizationDataService.AddOrganization(OrganizationToAdd);
                await EndAddAsync();
            }
        }

        protected async Task HandleUpdateSubmit()
        {
            if (!string.IsNullOrWhiteSpace(OrganizationToEdit.Name))
            {
                await OrganizationDataService.UpdateOrganization(OrganizationToEdit, Id);
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
