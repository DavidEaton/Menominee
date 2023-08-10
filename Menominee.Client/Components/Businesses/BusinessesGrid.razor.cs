using Menominee.Shared.Models.Businesses;
using Menominee.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Businesses
{
    public partial class BusinessesGrid : ComponentBase
    {
        [Parameter]
        public IReadOnlyList<BusinessToReadInList> Businesses { get; set; }

        [Parameter]
        public EventCallback<GridRowClickEventArgs> OnRowClicked { get; set; }

        public BusinessToReadInList SelectedBusiness { get; set; }

        [Inject]
        public LocalStorage LocalStorage { get; set; }

        [Inject]
        IJSRuntime JsInterop { get; set; }

        public TelerikGrid<BusinessToReadInList> Grid { get; set; }

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

        protected async Task OnStateInitHandler(GridStateEventArgs<BusinessToReadInList> args)
        {
            try
            {
                var state = await LocalStorage.GetItem<GridState<BusinessToReadInList>>(UniqueStorageKey);
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

        protected async void OnStateChangedHandler(GridStateEventArgs<BusinessToReadInList> args)
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
