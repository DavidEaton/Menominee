using CustomerVehicleManagement.Shared.Models;
using Menominee.OrganizationDtos.Services;
using Microsoft.AspNetCore.Components;
using SharedKernel.Enums;
using System;
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

        public AddressAddDto AddressToAdd { get; set; }
        protected bool AddingAddress { get; set; } = false;
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
                    State = readDto.Address.State,
                    PostalCode = readDto.Address.PostalCode
                };
            }

            if (readDto?.Emails.Count > 0)
            {
                foreach (var email in readDto.Emails)
                {
                    Organization.Emails.Add(new EmailUpdateDto
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
                    Organization.Phones.Add(new PhoneUpdateDto(phone.Number,
                                                               Enum.Parse<PhoneType>(phone.PhoneType),
                                                               phone.IsPrimary));
                }
            }
        }

        protected async Task HandleSubmit()
        {
            Message = string.Empty;

            if (AddingAddress)
            {
                Organization.Address = new AddressUpdateDto
                {
                    AddressLine = AddressToAdd.AddressLine,
                    City = AddressToAdd.City,
                    PostalCode = AddressToAdd.PostalCode,
                    State = AddressToAdd.State
                };
            }

            if (FormIsValid())
            {
                await OrganizationDataService.UpdateOrganization(Organization, Id);
                AddingAddress = false;
                Close();
            }

            else
            {
                Message = "Please complete all required items";
            }
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

        public void AddAddress()
        {
            AddingAddress = true;
            AddressToAdd = new AddressAddDto();
        }

        public void AddEmail()
        {
            Organization.Emails.Add(new EmailUpdateDto());
        }
    }
}
