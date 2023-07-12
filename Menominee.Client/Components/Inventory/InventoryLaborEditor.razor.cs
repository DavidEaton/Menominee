using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Shared.Models.ProductCodes;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryLaborEditor
    {
        [Inject]
        public IManufacturerDataService manufacturerDataService { get; set; }

        [Inject]
        public IProductCodeDataService productCodeDataService { get; set; }

        [Parameter]
        public InventoryItemToWrite Item { get; set; }

        [Parameter]
        public string Title { get; set; } = String.Empty;

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        private IReadOnlyList<ProductCodeToReadInList> ProductCodes = null;
        private string SaleCode = string.Empty;
        private bool parametersSet = false;
        private long productCodeId = 0;
        private long manufacturerId = 0;

        protected override async Task OnInitializedAsync()
        {
            manufacturerId = (await manufacturerDataService.GetManufacturerAsync(StaticManufacturerCodes.Miscellaneous))?.Id ?? 0;

            ProductCodes = (await productCodeDataService
                .GetAllProductCodesAsync(manufacturerId))
                .OrderBy(pc => pc.Code)
                .ToList();

            foreach (ItemLaborType item in Enum.GetValues(typeof(ItemLaborType)))
            {
                LaborTypeList.Add(new LaborTypeListItem { Text = EnumExtensions.GetDisplayName(item), Value = item });
            }

            foreach (SkillLevel item in Enum.GetValues(typeof(SkillLevel)))
            {
                SkillLevelList.Add(new SkillLevelListItem { Text = EnumExtensions.GetDisplayName(item), Value = item });
            }

            base.OnInitialized();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (parametersSet)
                return;
            parametersSet = true;

            if (Item?.ProductCode != null)
            {
                productCodeId = Item.ProductCode.Id;
            }

            await OnProductCodeChangeAsync();
            if (Item.Labor == null)
            {
                productCodeId = 0;
                Item.Manufacturer = await manufacturerDataService.GetManufacturerAsync(StaticManufacturerCodes.Miscellaneous);
                Item.ProductCode = new();
                Item.Labor = new();
                Item.ItemType = InventoryItemType.Labor;

                Title = "Add Labor";
            }
            else
            {
                Title = "Edit Labor";
            }

            StateHasChanged();
        }

        private async Task OnProductCodeChangeAsync()
        {
            if (productCodeId > 0 && Item.ProductCode?.Id != productCodeId)
                Item.ProductCode = await productCodeDataService.GetProductCodeAsync(productCodeId);

            if (Item != null && Item.ProductCode != null && Item.ProductCode.SaleCode != null && Item.ProductCode.SaleCode?.Id != 0)
                SaleCode = Item.ProductCode.SaleCode.Code + " - " + Item.ProductCode.SaleCode.Name;
            else
                SaleCode = string.Empty;
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
