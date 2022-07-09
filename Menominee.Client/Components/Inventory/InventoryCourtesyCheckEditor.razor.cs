using CustomerVehicleManagement.Shared.Models.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryCourtesyCheckEditor
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
            manufacturerId = (await manufacturerDataService.GetManufacturerAsync(StaticManufacturerCodes.Miscellaneous)).Id;
            ProductCodes = (await productCodeDataService.GetAllProductCodesAsync(manufacturerId)).ToList();
            ProductCodes.OrderBy(pc => pc.Code);

            foreach (ItemLaborType item in Enum.GetValues(typeof(ItemLaborType)))
            {
                laborTypeList.Add(new LaborTypeListItem { Text = EnumExtensions.GetDisplayName(item), Value = item });
            }

            foreach (SkillLevel item in Enum.GetValues(typeof(SkillLevel)))
            {
                skillLevelList.Add(new SkillLevelListItem { Text = EnumExtensions.GetDisplayName(item), Value = item });
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
            if (Item.CourtesyCheck == null)
            {
                productCodeId = 0;
                //Item.ManufacturerId = manufacturerId;
                Item.Manufacturer = ManufacturerHelper.ConvertReadToWriteDto(await manufacturerDataService.GetManufacturerAsync(StaticManufacturerCodes.Miscellaneous));
                Item.ProductCode = new();
                Item.CourtesyCheck = new();
                Item.ItemType = InventoryItemType.CourtesyCheck;

                Title = "Add Courtesy Check";
            }
            else
            {
                Title = "Edit Courtesy Check";
            }

            StateHasChanged();
        }

        private async Task OnProductCodeChangeAsync()
        {
            if (productCodeId > 0 && Item.ProductCode?.Id != productCodeId)
            {
                Item.ProductCode = ProductCodeHelper.ConvertReadToWriteDto(await productCodeDataService.GetProductCodeAsync(productCodeId));
            }

            if (Item != null && Item.ProductCode != null)
                SaleCode = Item.ProductCode.SaleCode.Code + " - " + Item.ProductCode.SaleCode.Name;
            else
                SaleCode = string.Empty;
        }

        private List<LaborTypeListItem> laborTypeList { get; set; } = new List<LaborTypeListItem>();
        private List<SkillLevelListItem> skillLevelList { get; set; } = new List<SkillLevelListItem>();

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
