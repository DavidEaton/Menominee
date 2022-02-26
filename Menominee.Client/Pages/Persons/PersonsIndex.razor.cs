using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Components.Address;
using Menominee.Client.Services;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Pages.Persons
{
    public partial class PersonsIndex : ComponentBase
    {
        [Inject]
        public IPersonDataService PersonDataService { get; set; }

        public IReadOnlyList<PersonToReadInList> Persons;

        public PersonToWrite PersonToWrite { get; set; }
        public TelerikGrid<PersonToReadInList> Grid { get; set; }
        private long Id { get; set; }

        private FormMode PersonFormMode = FormMode.Unknown;

        private FormMode AddressFormMode = FormMode.Unknown;

        private AddressEditor addressEditor;

        protected override async Task OnInitializedAsync()
        {
            Persons = (await PersonDataService.GetAllPersons()).ToList();
        }

        private void AddAddress()
        {
            PersonToWrite.Address = new();
            AddressFormMode = FormMode.Add;
        }

        public void EditAddress()
        {
            AddressFormMode = FormMode.Edit;
        }

        private void CancelEditAddress()
        {
            if (AddressFormMode == FormMode.Edit)
            {
                addressEditor.Cancel();
                AddressFormMode = FormMode.Unknown;
            }

            if (AddressFormMode == FormMode.Add && PersonToWrite.Address is not null)
            {
                AddressFormMode = FormMode.Unknown;
                StateHasChanged();  //TODO: When Person is new and Address is new, how to cancel?
                                    //Dialog does not close, console error: null reference exception
                PersonToWrite.Address = null;
            }
        }

        private async Task EditAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as PersonToReadInList).Id;
            PersonFormMode = FormMode.Edit;
            Persons = null;

            var readDto = await PersonDataService.GetPerson(Id);
            PersonToWrite = new PersonToWrite
            {
                Name = new PersonNameToWrite
                {
                    FirstName = readDto.FirstName,
                    MiddleName = readDto.MiddleName,
                    LastName = readDto.LastName
                },
                Gender = readDto.Gender,
                Birthday = readDto.Birthday
            };

            if (readDto.Address != null)
            {
                PersonToWrite.Address = new AddressToWrite
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
                    PersonToWrite.Emails.Add(new EmailToWrite
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
                    PersonToWrite.Phones.Add(new PhoneToWrite
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
            PersonFormMode = FormMode.Add;
            Persons = null;
            PersonToWrite = new();
            PersonToWrite.Name = new();
        }

        protected async Task HandleAddSubmit()
        {
            if (!string.IsNullOrWhiteSpace(PersonToWrite.Name.FirstMiddleLast))
            {
                await PersonDataService.AddPerson(PersonToWrite);
                await EndAddEditAsync();
            }
        }

        protected async Task HandleEditSubmit()
        {
            if (!string.IsNullOrWhiteSpace(PersonToWrite.Name.FirstMiddleLast))
            {
                await PersonDataService.UpdatePerson(PersonToWrite, Id);
                await EndAddEditAsync();
            }
        }

        protected async Task SubmitHandlerAsync()
        {
            if (!string.IsNullOrWhiteSpace(PersonToWrite.Name.FirstMiddleLast))
            {
                if (PersonFormMode == FormMode.Add)
                    await HandleAddSubmit();

                if (PersonFormMode == FormMode.Edit)
                    await HandleEditSubmit();
            }
        }

        protected async Task EndAddEditAsync()
        {
            PersonFormMode = FormMode.Unknown;
            AddressFormMode = FormMode.Unknown;
            Persons = (await PersonDataService.GetAllPersons()).ToList();
        }
    }
}
