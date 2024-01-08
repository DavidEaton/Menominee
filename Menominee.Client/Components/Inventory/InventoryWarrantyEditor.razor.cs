using Menominee.Domain.Enums;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryWarrantyEditor : InventoryEditorBase
    {
        [Inject]
        public Logger<InventoryWarrantyEditor> Logger { get; set; }

        private List<WarrantyPeriodListItem> periodTypeList { get; set; } = new List<WarrantyPeriodListItem>();

        protected override async Task OnInitializedAsync()
        {
            await LoadItemManufacturerByMiscellaneousStaticManufacturerCode();
            await LoadProductCodesByManufacturer();

            foreach (InventoryItemWarrantyPeriodType item in Enum.GetValues(typeof(InventoryItemWarrantyPeriodType)))
            {
                periodTypeList.Add(new WarrantyPeriodListItem { Text = EnumExtensions.GetDisplayName(item), Value = item });
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await OnParametersSetCommonAsync(InventoryItemType.Warranty, "Add Warranty", "Edit Warranty");
            await OnProductCodeChangedAsync();

            if (Item.Warranty is null)
            {
                ResetItemProductCode();
                Item.Warranty = new() { PeriodType = InventoryItemWarrantyPeriodType.Years, Duration = 1 };
            }
        }

        public class WarrantyPeriodListItem
        {
            public string Text { get; set; }
            public InventoryItemWarrantyPeriodType Value { get; set; }
        }
    }
}
