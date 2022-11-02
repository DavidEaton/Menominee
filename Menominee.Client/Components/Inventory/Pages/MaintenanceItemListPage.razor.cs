using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Client.Services.Inventory;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using Telerik.DataSource.Extensions;

namespace Menominee.Client.Components.Inventory.Pages
{
    public partial class MaintenanceItemListPage : ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        public IMaintenanceItemDataService DataService { get; set; }

        [Inject]
        public IInventoryItemDataService inventoryItemDataService { get; set; }

        public IReadOnlyList<MaintenanceItemToReadInList> ItemsList;
        public IEnumerable<MaintenanceItemToReadInList> SelectedList { get; set; } = Enumerable.Empty<MaintenanceItemToReadInList>();

        private bool ShowItemSelector = false;
        public InventoryItemToReadInList SelectedInventoryItem { get; set; }

        private MaintenanceItemToReadInList selectedItem;
        public MaintenanceItemToReadInList SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                ItemSelected = selectedItem != null;
            }
        }

        private bool ItemSelected { get; set; } = false;

        public TelerikGrid<MaintenanceItemToReadInList> Grid { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadItemsAsync();
        }

        private async Task LoadItemsAsync()
        {
            ItemsList = (await DataService.GetAllItemsAsync()).ToList();

            if (ItemsList.Count > 0)
            {
                SelectedItem = ItemsList.FirstOrDefault();
                SelectedList = new List<MaintenanceItemToReadInList> { SelectedItem };
            }
            Grid?.Rebind();
        }

        private void OnAdd()
        {
            ShowItemSelector = true;
        }

        private async Task OnDeleteAsync()
        {
            await DataService.DeleteItemAsync(SelectedItem.Id);
            await LoadItemsAsync();
        }

        private async Task OnMoveUpAsync()
        {
            if (SelectedItem != null)
            {
                await SwapItems(SelectedItem, -1);
            }
        }

        private async Task OnMoveDownAsync()
        {
            if (SelectedItem != null)
            {
                await SwapItems(SelectedItem, 1);
            }
        }

        private async Task SwapItems(MaintenanceItemToReadInList item, int direction)
        {
            int srcIndex = ItemsList.IndexOf(item);
            if (direction < 0 && srcIndex == 0) // can't move up if it's the first item
                return;
            if (direction > 0 && srcIndex == ItemsList.Count - 1)   // can't move down if it's the last item
                return;

            long dstId = ItemsList[srcIndex + direction].Id;

            // Increase/decrease the display order on the selected item
            var itemToRead = await DataService.GetItemAsync(item.Id);
            itemToRead.DisplayOrder += direction;
            var itemToWrite = MaintenanceItemHelper.ConvertReadToWriteDto(itemToRead);
            await DataService.UpdateItemAsync(itemToWrite, itemToWrite.Id);

            // Increase/decrease the display order on the destination item
            itemToRead = await DataService.GetItemAsync(dstId);
            itemToRead.DisplayOrder -= direction;
            itemToWrite = MaintenanceItemHelper.ConvertReadToWriteDto(itemToRead);
            await DataService.UpdateItemAsync(itemToWrite, itemToWrite.Id);

            await LoadItemsAsync();

            SelectedItem = ItemsList[srcIndex + direction];
            SelectedList = new List<MaintenanceItemToReadInList> { SelectedItem };
        }

        private void OnDone()
        {
            NavigationManager.NavigateTo("inventory");
        }

        protected void OnSelect(IEnumerable<MaintenanceItemToReadInList> items)
        {
            SelectedItem = items.FirstOrDefault();
            SelectedList = new List<MaintenanceItemToReadInList> { SelectedItem };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedItem = args.Item as MaintenanceItemToReadInList;
            SelectedList = new List<MaintenanceItemToReadInList> { SelectedItem };
        }

        protected async Task SubmitAddItemHandlerAsync()
        {
            if (SelectedInventoryItem != null)
            {
                var inventoryItem = await inventoryItemDataService.GetItemAsync(SelectedInventoryItem.Id);
                MaintenanceItemToWrite ItemToAdd = new();
                ItemToAdd.Item = InventoryItemHelper.ConvertReadToWriteDto(inventoryItem);
                await DataService.AddItemAsync(ItemToAdd);
                ShowItemSelector = false;
                await LoadItemsAsync();
            }
        }

        protected void EndAddItem()
        {
            ShowItemSelector = false;
        }
    }
}
