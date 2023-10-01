using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryInspectionEditor : InventoryEditorBase
    {
        [Inject]
        public Logger<InventoryDonationEditor> Logger { get; set; }

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        private List<LaborTypeListItem> LaborTypeList { get; set; } = new List<LaborTypeListItem>();
        private List<SkillLevelListItem> SkillLevelList { get; set; } = new List<SkillLevelListItem>();
        private List<InspectionTypeListItem> InspectionTypeList { get; set; } = new List<InspectionTypeListItem>();

        protected override async Task OnInitializedAsync()
        {
            await LoadItemManufacturerByMiscellaneousStaticManufacturerCode();
            await LoadProductCodesByManufacturer();

            foreach (ItemLaborType item in Enum.GetValues(typeof(ItemLaborType)))
            {
                LaborTypeList.Add(new LaborTypeListItem { Text = EnumExtensions.GetDisplayName(item), Value = item });
            }

            foreach (SkillLevel item in Enum.GetValues(typeof(SkillLevel)))
            {
                SkillLevelList.Add(new SkillLevelListItem { Text = EnumExtensions.GetDisplayName(item), Value = item });
            }

            foreach (InventoryItemInspectionType item in Enum.GetValues(typeof(InventoryItemInspectionType)))
            {
                InspectionTypeList.Add(new InspectionTypeListItem { Text = EnumExtensions.GetDisplayName(item), Value = item });
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await OnParametersSetCommonAsync(InventoryItemType.Inspection, "Add Inspection", "Edit Inspection");

            if (Item.Inspection is null)
            {
                ResetItemProductCode();
                await LoadItemManufacturerByMiscellaneousStaticManufacturerCode();
                Item.Inspection = new();
            }
        }

        public class LaborTypeListItem
        {
            public string Text { get; set; }
            public ItemLaborType Value { get; set; }
        }

        public class SkillLevelListItem
        {
            public string Text { get; set; }
            public SkillLevel Value { get; set; }
        }

        public class InspectionTypeListItem
        {
            public string Text { get; set; }
            public InventoryItemInspectionType Value { get; set; }
        }
    }
}
