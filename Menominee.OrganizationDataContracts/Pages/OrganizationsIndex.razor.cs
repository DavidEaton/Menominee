using CustomerVehicleManagement.Shared.Models;
using Menominee.OrganizationDataContracts.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
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

        public IReadOnlyList<OrganizationInListDto> OrganizationsList;
        public long Id { get; set; }

        private bool EditingOrganization { get; set; } = false;
        private bool AddingOrganization { get; set; } = false;

        private OrganizationAddDto OrganizationToAdd;
        private OrganizationUpdateDto OrganizationToUpdate;

        protected override async Task OnInitializedAsync()
        {
            OrganizationsList = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }

        private async Task EditAsync(long id)
        {
            Id = id;
            EditingOrganization = true;
            OrganizationsList = null;

            var readDto = await OrganizationDataService.GetOrganization(id);
            OrganizationToUpdate = new OrganizationUpdateDto
            {
                Name = readDto.Name,
                Note = readDto.Note
            };

            if (readDto.Address != null)
            {
                OrganizationToUpdate.Address = new AddressUpdateDto
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
                    OrganizationToUpdate.Emails.Add(new EmailUpdateDto
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
                    OrganizationToUpdate.Phones.Add(new PhoneUpdateDto
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
            if (!string.IsNullOrWhiteSpace(OrganizationToUpdate.Name))
            {
                await OrganizationDataService.UpdateOrganization(OrganizationToUpdate, Id);
                await EndEditAsync();
            }
        }

        protected async Task EndEditAsync()
        {
            EditingOrganization = false;
            OrganizationsList = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }

        protected async Task EndAddAsync()
        {
            AddingOrganization = false;
            OrganizationsList = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }
    }

}
