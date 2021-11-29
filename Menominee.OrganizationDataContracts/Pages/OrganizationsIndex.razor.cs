using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Menominee.OrganizationDataContracts.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
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

        [Inject]
        public LocalStorage LocalStorage { get; set; }
        [Inject]
        IJSRuntime JsInterop { get; set; }

        public IReadOnlyList<OrganizationToReadInList> OrganizationsList;
        public TelerikGrid<OrganizationToReadInList> Grid { get; set; }
        public long Id { get; set; }

        private bool Editing { get; set; } = false;
        private bool Adding { get; set; } = false;
        private bool EditingAddress { get; set; } = false;
        private bool isExporting { get; set; }
        private OrganizationToWrite organizationToWrite { get; set; }
        private bool ExportAllPages { get; set; }

        private string UniqueStorageKey = new Guid().ToString();

        protected override async Task OnInitializedAsync()
        {
            OrganizationsList = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }

        private void ShowLoadingSymbol()
        {
            isExporting = true;
            StateHasChanged();
            isExporting = false;
        }

        private void AddAddressAddingOrganization()
        {
            organizationToWrite.Address = new();
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
            Id = (args.Item as OrganizationToReadInList).Id;
            Editing = true;
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
            Adding = true;
            OrganizationsList = null;
            organizationToWrite = new();
        }

        protected async Task HandleAddSubmit()
        {
            if (!string.IsNullOrWhiteSpace(organizationToWrite.Name))
            {
                await OrganizationDataService.AddOrganization(organizationToWrite);
                await EndAddAsync();
            }
        }

        protected async Task HandleEditSubmit()
        {
            if (!string.IsNullOrWhiteSpace(organizationToWrite.Name))
            {
                await OrganizationDataService.UpdateOrganization(organizationToWrite, Id);
                await EndEditAsync();
            }
        }

        protected async Task SubmitHandlerAsync()
        {
            if (!string.IsNullOrWhiteSpace(organizationToWrite.Name))
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
            OrganizationsList = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }

        protected async Task EndAddAsync()
        {
            Adding = false;
            Editing = false;
            EditingAddress = false;
            OrganizationsList = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }

        protected async Task OnStateInitHandler(GridStateEventArgs<OrganizationToReadInList> args)
        {
            try
            {
                var state = await LocalStorage.GetItem<GridState<OrganizationToReadInList>>(UniqueStorageKey);
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

        protected async void OnStateChangedHandler(GridStateEventArgs<OrganizationToReadInList> args)
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
