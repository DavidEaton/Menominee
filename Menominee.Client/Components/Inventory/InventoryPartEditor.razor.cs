using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.ProductCodes;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryPartEditor
    {
        [Inject]
        public IManufacturerDataService manufacturerDataService { get; set; }

        [Inject]
        public IProductCodeDataService productCodeDataService { get; set; }

        [Parameter]
        public InventoryItemToWrite Item { get; set; }

        [Parameter]
        public string Title { get; set; } = "Edit Part";

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        private IReadOnlyList<ManufacturerToReadInList> Manufacturers = null;
        private IReadOnlyList<ProductCodeToReadInList> ProductCodes = new List<ProductCodeToReadInList>();
        private string SaleCode = string.Empty;
        private bool parametersSet = false;
        private long manufacturerId = 0;
        private long productCodeId = 0;

        protected override async Task OnInitializedAsync()
        {
            Manufacturers = (await manufacturerDataService.GetAllManufacturersAsync())
                                                          .Where(mfr => mfr.Prefix?.Length > 0
                                                                     && mfr.Id != StaticManufacturerCodes.Custom
                                                                     && mfr.Id != StaticManufacturerCodes.Package)
                                                          .OrderBy(mfr => mfr.Prefix)
                                                          .ToList();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (parametersSet)
                return;
            parametersSet = true;

            if (Item?.Manufacturer != null)
            {
                manufacturerId = Item.Manufacturer.Id;
            }
            if (Item?.ProductCode != null)
            {
                productCodeId = Item.ProductCode.Id;
            }

            if (Item?.Part == null)
            {
                Item.Part = new();
                Item.ItemType = InventoryItemType.Part;

                Title = "Add Part";
            }

            await OnManufacturerChangeAsync();
        }

        private async Task OnManufacturerChangeAsync()
        {
            if (manufacturerId > 0 && Item.Manufacturer?.Id != manufacturerId)
                Item.Manufacturer = await manufacturerDataService.GetManufacturerAsync(manufacturerId);

            if (Item?.Manufacturer is not null)
            {
                long savedProductCodeId = productCodeId;
                ProductCodes = (await productCodeDataService.GetAllProductCodesAsync(manufacturerId)).ToList();
                if (savedProductCodeId > 0 && Item.ProductCode?.Id == 0 && ProductCodes.Any(pc => pc.Id == savedProductCodeId) == true)
                    Item.ProductCode = await productCodeDataService.GetProductCodeAsync(savedProductCodeId);
                productCodeId = savedProductCodeId;
            }
            else
            {
                productCodeId = 0;
                ProductCodes = new List<ProductCodeToReadInList>();
                Item.ProductCode = new();
            }

            await OnProductCodeChangeAsync();
        }

        private async Task OnProductCodeChangeAsync()
        {
            if (productCodeId > 0 && Item.ProductCode?.Id != productCodeId)
                Item.ProductCode = await productCodeDataService.GetProductCodeAsync(productCodeId);

            if (Item != null && Item.ProductCode != null && Item.ProductCode.SaleCode != null && Item.ProductCode.SaleCode.Id != 0)
                SaleCode = Item.ProductCode.SaleCode.Code + " - " + Item.ProductCode.SaleCode.Name;
            else
                SaleCode = string.Empty;
        }
    }
}
