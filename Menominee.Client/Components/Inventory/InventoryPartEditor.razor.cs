using Menominee.Domain.Enums;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryPartEditor : InventoryEditorBase
    {
        protected override async Task OnInitializedAsync()
        {
            await LoadManufacturers();
        }

        protected override async Task OnParametersSetAsync()
        {
            await OnParametersSetCommonAsync(InventoryItemType.Part, "Add Part", "Edit Part");
            await OnManufacturerChangedAsync();

            if (Item.Part is null)
            {
                ResetItemProductCode();
                await LoadItemManufacturerByMiscellaneousStaticManufacturerCode();
                Item.Part = new();
            }
        }
    }
}
