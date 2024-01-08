using Menominee.Domain.Enums;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryLaborEditor : InventoryEditorBase
    {
        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

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
        }

        protected override async Task OnParametersSetAsync()
        {
            await OnParametersSetCommonAsync(InventoryItemType.Labor, "Add Labor", "Edit Labor");
            await OnProductCodeChangedAsync();

            if (Item.Labor is null)
            {
                ResetItemProductCode();
                await LoadItemManufacturerByMiscellaneousStaticManufacturerCode();
                Item.Labor = new();
            }
        }

        private List<LaborTypeListItem> LaborTypeList { get; set; } = new List<LaborTypeListItem>();
        private List<SkillLevelListItem> SkillLevelList { get; set; } = new List<SkillLevelListItem>();

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
    }
}
