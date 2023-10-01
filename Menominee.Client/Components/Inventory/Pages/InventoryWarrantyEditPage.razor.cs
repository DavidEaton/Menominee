using CSharpFunctionalExtensions;
using Menominee.Client.Services.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Inventory.Pages
{
    public partial class InventoryWarrantyEditPage : ComponentBase
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Inject]
        public IInventoryItemDataService DataService { get; set; }

        [Inject]
        public ILogger<InventoryWarrantyEditPage> Logger { get; set; }

        [Parameter]
        public long ItemId { get; set; }

        private InventoryItemToWrite Item;

        protected override async Task OnInitializedAsync()
        {
            if (ItemId == 0)
            {
                InitializeNewItem();
                return;
            }

            var result = await DataService.GetAsync(ItemId);
            HandleInitializationResult(result);
        }

        private void InitializeNewItem()
        {
            Item = new();
        }

        private void HandleInitializationResult(Result<InventoryItemToRead> result)
        {
            if (result.IsSuccess)
            {
                Item = InventoryItemHelper.ConvertReadToWriteDto(result.Value);
            }
            else
            {
                ResetItemAndNotifyUser(result.Error);
            }
        }
        private void ResetItemAndNotifyUser(string errorMessage)
        {
            Logger.LogError("Failed to initialize: {ErrorMessage}", errorMessage);
            //toastService.ShowError("Failed to load the item. Starting with a new item.", "Initialization Failed");

            ItemId = default;
            Item = new();
        }
        private async Task<Result<string>> Save()
        {
            const string addItemSuccessMessage = "Item added successfully";
            const string addItemFailureMessage = "Failed to add new item";
            const string updateItemSuccessMessage = "Item updated successfully";
            const string updateItemFailureMessage = "Failed to update item";
            const string generalFailureMessage = "An error occurred while saving the item";

            try
            {
                Result<string> operationResult;

                if (ItemId == 0)
                {
                    var addItemResult = await DataService.AddAsync(Item);
                    operationResult = addItemResult.IsSuccess
                        ? Result.Success(addItemSuccessMessage)
                        : Result.Failure<string>(addItemFailureMessage);

                    if (addItemResult.IsSuccess)
                    {
                        ItemId = Item.Id;
                    }
                }
                else
                {
                    var updateItemResult = await DataService.UpdateAsync(Item);
                    operationResult = updateItemResult.IsSuccess
                        ? Result.Success(updateItemSuccessMessage)
                        : Result.Failure<string>(updateItemFailureMessage);
                }

                if (operationResult.IsSuccess)
                {
                    // toastService.ShowSuccess(operationResult.Value, "Success");
                }

                return operationResult;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, generalFailureMessage);
                return Result.Failure<string>(generalFailureMessage);
            }
            finally
            {
                EndEdit();
            }
        }



        private void Discard()
        {
            EndEdit();
        }

        protected void EndEdit()
        {
            navigationManager.NavigateTo($"/inventory/items/listing/{ItemId}");
        }
    }
}
