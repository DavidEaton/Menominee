using CSharpFunctionalExtensions;
using Menominee.Client.Components.Address;
using Menominee.Client.Services;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Persons;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace Menominee.Client.Pages.Persons
{
    public partial class PersonsIndex : ComponentBase
    {
        [Inject]
        public IPersonDataService PersonDataService { get; set; }

        [Inject]
        public ILogger<PersonsIndex> Logger { get; set; }

        public IReadOnlyList<PersonToReadInList> Persons;

        public PersonToWrite Person { get; set; }
        public TelerikGrid<PersonToReadInList> Grid { get; set; }
        private long Id { get; set; }

        private FormMode PersonFormMode = FormMode.Unknown;

        private FormMode AddressFormMode = FormMode.Unknown;

        private AddressEditor addressEditor;

        protected override async Task OnInitializedAsync()
        {
            await PersonDataService.GetAllAsync()
                .Match(
                    success => Persons = success,
                    failure => Logger.LogError(failure)
            );

        }

        private void AddAddress()
        {
            Person.Address = new();
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

            if (AddressFormMode == FormMode.Add && Person.Address is not null)
            {
                AddressFormMode = FormMode.Unknown;
                StateHasChanged();  //TODO: When Person is new and Address is new, how to cancel?
                                    //Dialog does not close, console error: null reference exception
                Person.Address = null;
            }
        }

        private async Task EditAsync(GridRowClickEventArgs args)
        {
            var id = (args.Item as PersonToReadInList)?.Id ?? Id;
            var formMode = args.Item switch
            {
                PersonToReadInList => FormMode.Edit,
                _ => PersonFormMode
            };

            var personResult = await PersonDataService.GetAsync(Id)
                .OnFailure(error =>
                {
                    Logger.LogError(error);
                    // TODO: Replace exception with toast message
                    throw new Exception(error);
                });

            var person = personResult.Value;

            Person = new PersonToWrite
            {
                Name = new PersonNameToWrite
                {
                    FirstName = person.Name.FirstName,
                    MiddleName = person.Name.MiddleName,
                    LastName = person.Name.LastName
                },
                Gender = person.Gender,
                Birthday = person.Birthday
            };

            if (person.Address != null)
            {
                Person.Address = new AddressToWrite
                {
                    AddressLine1 = person.Address.AddressLine1,
                    City = person.Address.City,
                    State = person.Address.State,
                    PostalCode = person.Address.PostalCode,
                    AddressLine2 = person.Address.AddressLine2
                };
            }

            if (person?.Emails.Count > 0)
            {
                foreach (var email in person.Emails)
                {
                    Person.Emails.Add(new EmailToWrite
                    {
                        Address = email.Address,
                        IsPrimary = email.IsPrimary
                    });
                }
            }

            if (person?.Phones.Count > 0)
            {
                foreach (var phone in person.Phones)
                {
                    Person.Phones.Add(new PhoneToWrite
                    {
                        Number = phone.Number,
                        PhoneType = phone.PhoneType,
                        IsPrimary = phone.IsPrimary
                    });
                }
            }
        }

        private void Add()
        {
            PersonFormMode = FormMode.Add;
            Persons = null;
            Person = new();
            Person.Name = new();
        }

        protected async Task HandleAddSubmit()
        {
            if (!string.IsNullOrWhiteSpace(Person.Name.FirstName))
            {
                await PersonDataService.AddAsync(Person);
                await EndAddEditAsync();
            }
        }

        protected async Task HandleEditSubmit()
        {
            if (!string.IsNullOrWhiteSpace(Person.Name.FirstName))
            {
                await PersonDataService.UpdateAsync(Person);
                await EndAddEditAsync();
            }
        }

        protected async Task SubmitHandlerAsync()
        {
            if (!string.IsNullOrWhiteSpace(Person.Name.FirstName))
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
            await PersonDataService.GetAllAsync()
                .Match(
                    success => Persons = success,
                    failure => Logger.LogError(failure));
        }
    }
}
