using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Components;
using Menominee.Client.Services;
using Microsoft.AspNetCore.Components;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Pages
{
    public partial class OrganizationDetail : ComponentBase
    {
        [Inject]
        public IOrganizationDataService OrganizationDataService { get; set; }

        [Parameter]
        public long Id { get; set; }
        protected OrganizationNameForm OrganizationNameForm { get; set; }
        protected AddressForm AddressForm { get; set; }
        public OrganizationUpdateDto OrganizationUpdateDto { get; set; } = new();
        public OrganizationReadDto Organization { get; set; } = new();

        // Screen state
        protected FormMode FormMode = FormMode.Read;
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;
        //protected bool EditName = false;
        //protected bool EditAddress = false;
        //protected bool EditNotes = false;

        protected override async Task OnParametersSetAsync()
        {
            if (Id > 0)
            {
                Organization = await OrganizationDataService.GetOrganizationDetails(Id);
                OrganizationUpdateDto = new();
            }
        }
        public void OrganizationNameForm_OnOrganizationNameChanged()
        {
            OrganizationUpdateDto.Name = OrganizationNameForm.OrganizationName.Name;
            StateHasChanged();
        }

        public void SaveEdits()
        {
            FormMode = FormMode.Read;
        }

        protected void ToggleFormMode()
        {
            switch (FormMode)
            {
                case FormMode.Edit:
                    FormMode = FormMode.Read;
                    break;
                case FormMode.Read:
                    FormMode = FormMode.Edit;
                    break;
                default:
                    break;
            }
        }

        protected async Task HandleValidSubmit()
        {
            Saved = false;

            if (Organization.Id == 0) // new Organization
            {
                var phones = (IList<PhoneCreateDto>)Organization.Phones
                    .Select(phone =>
                            new PhoneCreateDto {
                                Number = phone.Number,
                                PhoneType = Enum.Parse<PhoneType>(phone.PhoneType),
                                IsPrimary = phone.IsPrimary
                            });

                var emails = (IList<EmailCreateDto>)Organization.Emails
                    .Select(email =>
                            new EmailCreateDto {
                                Address = email.Address,
                                IsPrimary = email.IsPrimary
                            });

                var organization = new OrganizationAddDto
                {
                    Name = Organization.Name,
                    Address = new AddressAddDto
                    {
                        AddressLine = Organization.Address.AddressLine,
                        City = Organization.Address.City,
                        State = Organization.Address.State,
                        PostalCode = Organization.Address.PostalCode
                    },
                    Note = Organization.Note
                };

                //organization.Contact = Organization.Contact;
                //organization.Phones = Organization.Phones;
                //organization.Emails = Organization.Emails;

                var addedOrganization = await OrganizationDataService.AddOrganization(organization);

                if (addedOrganization != null)
                {
                    StatusClass = "alert-success";
                    Message = "New person added successfully.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong adding the new person. Error logged.";
                    Saved = false;
                }
            }
            //else // existing
            //{
            //    await OrganizationDataService.UpdateOrganization(Organization);
            //    StatusClass = "alert-success";
            //    Message = "Organization updated successfully.";
            //    Saved = true;
            //}
        }

        //private void ToggleEditName() => EditName = !EditName;
        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "Please resolve validation errors.";
        }

        private void DeleteOrganization()
        {
            Console.WriteLine("DeleteOrganization() called");
        }

        public void Close()
        {
            Saved = true;
            Message = string.Empty;
            StatusClass = string.Empty;
            Id = -1;
            StateHasChanged();
        }
    }

    public enum FormMode
    {
        Read,
        Add,
        Edit
    }
}
