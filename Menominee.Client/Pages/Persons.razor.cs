using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Services;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Pages
{
    public partial class Persons : ComponentBase
    {
        [Inject]
        public IPersonDataService PersonDataService { get; set; }

        public IReadOnlyList<PersonToReadInList> PersonsList;

        [Inject]
        public LocalStorage LocalStorage { get; set; }

        [Inject]
        IJSRuntime JsInterop { get; set; }
        private PersonToWrite PersonToWrite { get; set; }
        public TelerikGrid<PersonToReadInList> Grid { get; set; }
        private long Id { get; set; }
        private bool Editing { get; set; } = false;
        private bool Adding { get; set; } = false;
        private bool EditingAddress { get; set; } = false;
        private bool isExporting { get; set; }
        private bool ExportAllPages { get; set; }

        private string UniqueStorageKey = new Guid().ToString();

        protected override async Task OnInitializedAsync()
        {
            PersonsList = (await PersonDataService.GetAllPersons()).ToList();
        }

        private void ShowLoadingSymbol()
        {
            isExporting = true;
            StateHasChanged();
            isExporting = false;
        }

        private void AddAddressAddingPerson()
        {
            PersonToWrite.Address = new();
            EditingAddress = true;
        }

        private void CancelEditAddress()
        {
            EditingAddress = false;
        }

        public void EditAddress()
        {
            EditingAddress = true;
        }
        private async Task EditAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as PersonToReadInList).Id;
            Editing = true;
            PersonsList = null;

            var readDto = await PersonDataService.GetPerson(Id);
            PersonToWrite = new PersonToWrite
            {
                Name = new PersonNameToWrite
                {
                    FirstName = readDto.Name.FirstName,
                    MiddleName = readDto.Name.MiddleName,
                    LastName = readDto.Name.LastName
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
            Adding = true;
            PersonsList = null;
            PersonToWrite = new();
            PersonToWrite.Name = new();
        }

        protected async Task HandleAddSubmit()
        {
            if (!string.IsNullOrWhiteSpace(PersonToWrite.Name.FirstMiddleLast))
            {
                await PersonDataService.AddPerson(PersonToWrite);
                await EndAddAsync();
            }
        }

        protected async Task HandleEditSubmit()
        {
            if (!string.IsNullOrWhiteSpace(PersonToWrite.Name.FirstMiddleLast))
            {
                await PersonDataService.UpdatePerson(PersonToWrite, Id);
                await EndEditAsync();
            }
        }

        protected async Task SubmitHandlerAsync()
        {
            if (!string.IsNullOrWhiteSpace(PersonToWrite.Name.FirstMiddleLast))
            {
                if (Adding)
                    await HandleAddSubmit();

                if (Editing)
                    await HandleEditSubmit();
            }
        }

        protected async Task EndEditAsync()
        {
            Editing = false;
            EditingAddress = false;
            PersonsList = (await PersonDataService.GetAllPersons()).ToList();
        }

        protected async Task EndAddAsync()
        {
            Adding = false;
            Editing = false;
            EditingAddress = false;
            PersonsList = (await PersonDataService.GetAllPersons()).ToList();
        }

        protected async Task OnStateInitHandler(GridStateEventArgs<PersonToReadInList> args)
        {
            try
            {
                var state = await LocalStorage.GetItem<GridState<PersonToReadInList>>(UniqueStorageKey);
                if (state != null)
                {
                    args.GridState = state;
                }

            }
            catch (InvalidOperationException e)
            {
                // the JS Interop for the local storage cannot be used during pre-rendering
                // so the code above will throw. Once the app initializes, it will work fine - Telerik docs
            }
        }

        protected async void OnStateChangedHandler(GridStateEventArgs<PersonToReadInList> args)
        {
            await LocalStorage.SetItem(UniqueStorageKey, args.GridState);
        }

        async void ResetState()
        {
            await Grid.SetState(null);
            await LocalStorage.RemoveItem(UniqueStorageKey);
        }
    }


}
