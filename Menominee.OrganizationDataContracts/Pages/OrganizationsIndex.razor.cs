using CustomerVehicleManagement.Shared.Models;
using Menominee.OrganizationDataContracts.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using Microsoft.JSInterop;

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

        private bool EditingOrganization { get; set; } = false;
        private bool AddingOrganization { get; set; } = false;
        private bool EditingAddress { get; set; } = false;
        private bool AddingAddress { get; set; } = false;
        private bool isExporting { get; set; }
        private bool AddressDialogVisible => (OrganizationToEdit?.Address != null && (AddingAddress || EditingAddress)) || (OrganizationToAdd?.Address != null && (AddingAddress || EditingAddress));

        private OrganizationToAdd OrganizationToAdd { get; set; }
        private OrganizationToEdit OrganizationToEdit { get; set; }
        private bool ExportAllPages { get; set; }
        private string UniqueStorageKey = new Guid().ToString();

        protected override async Task OnInitializedAsync()
        {
            OrganizationsList = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }

        private async Task ShowLoadingSign()
        {
            isExporting = true;
            StateHasChanged();
            // This won't work for server-side Blazor, the UI will render immediately after the delay and the loading sign will only flicker
            await Task.Delay(3000);
            isExporting = false;
        }

        private void AddAddressAddingOrganization()
        {
            OrganizationToAdd.Address = new();
            AddingAddress = true;
        }

        private void AddAddressEditingOrganization()
        {
            OrganizationToEdit.Address = new();
            EditingAddress = true;
        }

        private void CancelAddAddress()
        {
            AddingAddress = false;

            if (OrganizationToAdd.Address != null)
                OrganizationToAdd.Address = null;
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
            EditingOrganization = true;
            OrganizationsList = null;

            var readDto = await OrganizationDataService.GetOrganization(Id);
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
