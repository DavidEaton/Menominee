using CustomerVehicleManagement.Shared.Models.Persons;
using Menominee.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Persons
{
    public partial class PersonsGrid : ComponentBase
    {
        [Parameter]
        public IReadOnlyList<PersonToReadInList> Persons { get; set; }

        [Parameter]
        public EventCallback<GridRowClickEventArgs> OnRowClicked { get; set; }

        public PersonToReadInList SelectedPerson { get; set; }

        [Inject]
        public LocalStorage LocalStorage { get; set; }

        [Inject]
        IJSRuntime JsInterop { get; set; }

        public TelerikGrid<PersonToReadInList> Grid { get; set; }

        private async Task GridRowClicked(GridRowClickEventArgs args)
        {
            await OnRowClicked.InvokeAsync(args);
        }
        private bool isExporting { get; set; }

        private bool ExportAllPages { get; set; }

        private string UniqueStorageKey = new Guid().ToString();

        private void ShowLoadingSymbol()
        {
            isExporting = true;
            StateHasChanged();
            isExporting = false;
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
            catch (InvalidOperationException)
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
            await Grid.SetStateAsync(null);
            await LocalStorage.RemoveItem(UniqueStorageKey);
        }
    }
}