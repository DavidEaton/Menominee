using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryTireEditor : InventoryEditorBase
    {
        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadManufacturers();
        }

        protected override async Task OnParametersSetAsync()
        {
            await OnParametersSetCommonAsync(InventoryItemType.Tire, "Add Tire", "Edit Tire");

            //if (Item?.ManufacturerId != 0)
            //{
            //    // FIX ME ??? is there a more elegant solution to retaining/restoring the Item.ProductCodeId ???
            //    var savedPCId = Item.ProductCodeId;
            //    ProductCodes = (await productCodeDataService.GetAllProductCodesAsync(Item.ManufacturerId)).ToList();
            //    if (savedPCId > 0 && Item.ProductCodeId == 0 && ProductCodes.Any(pc => pc.Id == savedPCId) == true)
            //        Item.ProductCodeId = savedPCId;
            //}

            // Okay here's my attempt:
            await SetProductCodeIdIfNecessaryAsync();

            if (Item.Tire is null)
            {
                ResetItemProductCode();
                Item.Tire = new();
            }

            await OnManufacturerChangedAsync();
        }

        private async Task SetProductCodeIdIfNecessaryAsync()
        {
            if (ItemManufacturerIsNew())
            {
                await LoadProductCodesByManufacturer();
                RestoreProductCodeIdIfNecessary();
            }
        }

        private bool ItemManufacturerIsNew() => Item?.ManufacturerId == 0;

        private void RestoreProductCodeIdIfNecessary()
        {
            var loadedProductCodeId = Item?.ProductCodeId ?? 0;
            if (ShouldRestoreProductCodeId(loadedProductCodeId))
                Item.ProductCodeId = loadedProductCodeId;
        }

        /* ShouldRestoreProductCodeId
        loadedProductCodeId > 0: checks whether the loadedProductCodeId,
        which is presumably the original ProductCodeId for the item,
        is greater than zero. If this condition is not met, it means
        that no valid ProductCodeId was loaded initially,
        so restoring is not applicable.

        Item.ProductCodeId == 0: checks whether the current ProductCodeId
        on the item is zero. If it is zero, then it suggests that the 
        ProductCodeId may need to be restored from the loaded value.

        ProductCodes.Any(pc => pc.Id == loadedProductCodeId): checks if 
        there exists a product code in the ProductCodes list that matches 
        the loadedProductCodeId. If such a product code exists, then it's 
        safe to restore the ProductCodeId.
        */
        private bool ShouldRestoreProductCodeId(long loadedProductCodeId) =>
            loadedProductCodeId > 0
            && Item.ProductCodeId == 0
            && ProductCodes.Any(pc => pc.Id == loadedProductCodeId);
    }
}
