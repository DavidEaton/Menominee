using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Shared.Models.ProductCodes;
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
    public partial class InventoryGiftCertificateEditor
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

            ProductCodes = (await productCodeDataService
                .GetAllProductCodesAsync(manufacturerId))
                .OrderBy(pc => pc.Code)
                .ToList();
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

            if (Item.Id == 0)//if (Item.GiftCertificate == null)
            {
                productCodeId = 0;
                //Item.ManufacturerId = manufacturerId;
                Item.Manufacturer = await manufacturerDataService.GetManufacturerAsync(StaticManufacturerCodes.Miscellaneous);
                Item.ProductCode = new();
                //Item.GiftCertificate = new();
                Item.ItemType = InventoryItemType.GiftCertificate;

                Title = "Add Gift Certificate";
            }
            else
            {
                Title = "Edit Gift Certificate";
            }

            StateHasChanged();
        }

        private async Task OnProductCodeChangeAsync()
        {
            if (productCodeId > 0 && Item.ProductCode?.Id != productCodeId)
                Item.ProductCode = await productCodeDataService.GetProductCodeAsync(productCodeId);

            if (Item != null && Item.ProductCode != null)
                SaleCode = Item.ProductCode.SaleCode.Code + " - " + Item.ProductCode.SaleCode.Name;
            else
                SaleCode = string.Empty;
        }
    }
}
