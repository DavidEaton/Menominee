using CSharpFunctionalExtensions;
using Menominee.Client.Services.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Shared.Models.Inventory.MaintenanceItems;
using Microsoft.AspNetCore.Components;
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
        public ILogger<MaintenanceItemListPage> Logger { get; set; }

        [Inject]
        public IInventoryItemDataService InventoryItemDataService { get; set; }

        public IReadOnlyList<MaintenanceItemToReadInList> ItemsList;
        public IEnumerable<MaintenanceItemToReadInList> SelectedList { get; set; } = Enumerable.Empty<MaintenanceItemToReadInList>();

#pragma warning disable IDE0052 // Remove unread private members... OnAddItem() uses it!!!!!!
        private bool ShowItemSelector = false;
#pragma warning restore IDE0052 // Remove unread private members
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
            ItemsList = (await DataService.GetAllAsync()).Value.ToList();

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
            await DataService.DeleteAsync(SelectedItem.Id);
            await LoadItemsAsync();
        }

        private async Task OnMoveUpAsync()
        {
            if (SelectedItem is not null)
            {
                await SwapItems(SelectedItem, -1);
            }
        }

        private async Task OnMoveDownAsync()
        {
            if (SelectedItem is not null)
            {
                await SwapItems(SelectedItem, 1);
            }
        }

        private async Task SwapItems(MaintenanceItemToReadInList item, int direction)
        {
            var srcIndex = ItemsList.IndexOf(item);
            if (CanMove(srcIndex, direction))
            {
                var dstId = ItemsList[srcIndex + direction].Id;

                var sourceUpdateResult = await UpdateDisplayOrder(item.Id, direction);
                var destinationUpdateResult = await UpdateDisplayOrder(dstId, -direction);

                if (sourceUpdateResult.IsSuccess && destinationUpdateResult.IsSuccess)
                {
                    await RefreshItemsList(srcIndex + direction);
                }
            }
        }

        private bool CanMove(int index, int direction)
        {
            return !(direction < 0 && index == 0) && !(direction > 0 && index == ItemsList.Count - 1);
        }

        private async Task<Result> UpdateDisplayOrder(long itemId, int direction)
        {
            var itemResult = await DataService.GetAsync(itemId);
            if (itemResult.IsFailure)
            {
                Logger.LogError("Failed to get item with id {Id}", itemId);
                return Result.Failure("Failed to get item.");
            }

            var itemToRead = itemResult.Value;
            itemToRead.DisplayOrder += direction;
            var itemToWrite = MaintenanceItemHelper.ConvertReadToWriteDto(itemToRead);

            var updateResult = await DataService.UpdateAsync(itemToWrite);
            if (updateResult.IsFailure)
            {
                Logger.LogError("Failed to update item with id {Id}", itemId);
            }

            return updateResult;
        }

        private async Task RefreshItemsList(int newIndex)
        {
            await LoadItemsAsync();
            SelectedItem = ItemsList[newIndex];
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
            if (SelectedInventoryItem is not null)
            {
                var result = await InventoryItemDataService.GetAsync(SelectedInventoryItem.Id);

                if (result.IsFailure)
                {
                    Logger.LogError("Failed to get inventory item with id {Id}", SelectedInventoryItem.Id);
                    //toastService.ShowError("Failed to add selected item", "Operation Failed");
                    return;
                }

                if (result.IsSuccess)
                {
                    var inventoryItem = result.Value;
                    var ItemToAdd = new MaintenanceItemToWrite
                    {
                        Item = InventoryItemHelper.ConvertReadToWriteDto(inventoryItem)
                    };
                    await DataService.AddAsync(ItemToAdd);
                    ShowItemSelector = false;
                    await LoadItemsAsync();
                }
            }
        }

        protected void EndAddItem()
        {
            ShowItemSelector = false;
        }
    }
}
