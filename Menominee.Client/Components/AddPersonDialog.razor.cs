using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Services;
using Microsoft.AspNetCore.Components;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Threading.Tasks;

namespace Menominee.Client.Components
{
    public partial class AddPersonDialog
    {
        private PersonAddProperties PersonAdd { get; set; }
        private PersonToAdd Person { get; set; }
        public string Message { get; set; }
        public EntityType EntityType { get; set; }

        protected PersonNameForm PersonNameForm { get; set; }
        protected AddressForm AddressForm { get; set; }

        [Inject]
        public IPersonDataService PersonDataService { get; set; }
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
            PersonAdd = new PersonAddProperties();
        }

        protected async Task HandleValidSubmit()
        {
            Message = string.Empty;

            if (FormIsValid())
            {
                Person = new PersonToAdd
                {
                    Name =
                    new PersonNameToAdd
                    {
                        FirstName = PersonAdd.Name.FirstName,
                        MiddleName = PersonAdd.Name.MiddleName,
                        LastName = PersonAdd.Name.LastName
                    },

                    Gender = PersonAdd.Gender,
                    Birthday = PersonAdd.Birthday,
                    Address = PersonAdd.Address
                };

                await PersonDataService.AddPerson(Person);
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
            if (PersonAdd.Name != null)
                return true;

            return false;
        }

        public void PersonNameForm_OnPersonNameChanged()
        {
            PersonAdd.Name = PersonNameForm.PersonName;
            //StateHasChanged();
        }

        public void AddressForm_OnAddressChanged()
        {
            PersonAdd.Address = AddressForm.EntityAddress;
        }

        private class PersonAddProperties
        {
            internal PersonName Name { get; set; }
            public Gender Gender { get; set; }
            public DateTime? Birthday { get; set; }
            internal AddressToAdd Address { get; set; }
        }
    }
}
