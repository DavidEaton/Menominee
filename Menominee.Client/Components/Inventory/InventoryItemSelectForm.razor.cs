using CustomerVehicleManagement.Shared.Models.Inventory;
using Menominee.Client.Services.Inventory;
using Menominee.Client.Services.Manufacturers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryItemSelectForm : ComponentBase
    {
        [Inject]
        public IInventoryItemDataService DataService { get; set; }

        [Inject]
        public IManufacturerDataService MfrDataService { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        [Parameter]
        public EventCallback OnSelect { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Parameter]
        public InventoryItemToReadInList SelectedItem { get; set; }

        [Parameter]
        public EventCallback<InventoryItemToReadInList> SelectedItemChanged { get; set; }

        public IReadOnlyList<InventoryItemToReadInList> ItemsList;
        public IEnumerable<InventoryItemToReadInList> SelectedList { get; set; } = Enumerable.Empty<InventoryItemToReadInList>();

        private bool CanSelect { get; set; } = false;

        //private long selectedId = 0;
        //public long SelectedId
        //{
        //    get => selectedId;
        //    set
        //    {
        //        selectedId = value;
        //        CanSelect = selectedId > 0;
        //    }
        //}

        protected override async Task OnInitializedAsync()
        {
            ItemsList = (await DataService.GetAllItems()).ToList();

            if (ItemsList.Count > 0)
            {
                SelectedItem = ItemsList.FirstOrDefault();
                await SelectedItemChanged.InvokeAsync(SelectedItem);
                //SelectedId = SelectedItem.Id;
                SelectedList = new List<InventoryItemToReadInList> { SelectedItem };
            }
            else
            {
                //SelectedId = 0;
            }

            CanSelect = ItemsList.Count > 0;
        }

        protected async Task OnSelectItemAsync(IEnumerable<InventoryItemToReadInList> items)
        {
            SelectedItem = items.FirstOrDefault();
            SelectedList = new List<InventoryItemToReadInList> { SelectedItem };
            //SelectedId = SelectedItem.Id;
            await SelectedItemChanged.InvokeAsync(SelectedItem);
        }

        private async Task OnRowSelectedAsync(GridRowClickEventArgs args)
        {
            //SelectedId = (args.Item as InventoryItemToReadInList).Id;
            SelectedItem = args.Item as InventoryItemToReadInList;
            await SelectedItemChanged.InvokeAsync(SelectedItem);
        }
    }
}
