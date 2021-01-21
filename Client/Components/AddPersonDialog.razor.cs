using Client.Models;
using Client.Services;
using CustomerVehicleManagement.Domain.ValueObjects;
using Microsoft.AspNetCore.Components;
using SharedKernel.Enums;
using System;
using System.Threading.Tasks;

namespace Client.Components
{
    public partial class AddPersonDialog
    {
        private PersonAddProperties PersonAdd { get; set; }
        private PersonAddDto Person { get; set; }
        public EntityType EntityType { get; set; }

        protected PersonNameForm PersonNameForm { get; set; }

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
            await PersonDataService.AddPerson(Person);
            ShowDialog = false;

            await CloseEventCallback.InvokeAsync(true);
            StateHasChanged();
        }

        public void PersonNameForm_OnPersonNameCreated()
        {
            Person = new PersonAddDto(PersonNameForm.PersonName, Gender.Male);
            var moops = Person;
            //StateHasChanged();
        }

        private class PersonAddProperties
        {
            internal PersonName Name { get; set; }
            public Gender Gender { get; set; }
            public DateTime? Birthday { get; set; }
            internal Address Address { get; set; }
        }
    }
}
