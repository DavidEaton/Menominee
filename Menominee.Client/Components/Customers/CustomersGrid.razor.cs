﻿using Menominee.Client.Services;
using Menominee.Shared.Models.Customers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Customers
{
    public partial class CustomersGrid : ComponentBase
    {
        [Parameter]
        public IReadOnlyList<CustomerToReadInList> CustomersList { get; set; }
        public TelerikGrid<CustomerToReadInList> Grid { get; set; }

        [Parameter]
        public EventCallback<GridRowClickEventArgs> OnRowClicked { get; set; }

        [Parameter]
        public EventCallback OnAddAsync { get; set; }

        [Inject]
        public LocalStorage LocalStorage { get; set; }

        [Inject]
        IJSRuntime JsInterop { get; set; }

        private string UniqueStorageKey = new Guid().ToString();

        private async Task RowClicked(GridRowClickEventArgs args)
        {
            await OnRowClicked.InvokeAsync(args);
        }

        private async Task AddAsync()
        {
            await OnAddAsync.InvokeAsync();
        }

        protected async void OnStateChangedHandler(GridStateEventArgs<CustomerToReadInList> args)
        {
            await LocalStorage.SetItem(UniqueStorageKey, args.GridState);
        }

        async void ResetState()
        {
            await Grid.SetStateAsync(null);
            await LocalStorage.RemoveItem(UniqueStorageKey);
        }

        protected async Task OnStateInitHandler(GridStateEventArgs<CustomerToReadInList> args)
        {
            try
            {
                var state = await LocalStorage.GetItem<GridState<CustomerToReadInList>>(UniqueStorageKey);
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

    }
}
