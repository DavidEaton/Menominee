using CustomerVehicleManagement.Shared.Models.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryPartForm
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

        protected override async Task OnInitializedAsync()
        {
            Manufacturers = (await manufacturerDataService.GetAllManufacturersAsync())
                                                          .Where(mfr => mfr.Prefix?.Length > 0 && mfr.Code != "0")
                                                          .ToList();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (parametersSet)
                return;

            parametersSet = true;

            if (Item?.Part == null)
            {
                Item.Part = new();
                Item.ItemType = InventoryItemType.Part;
            }

            if (Item?.ManufacturerId != 0)
            {
                // FIX ME ??? is there a more elegant solution to retaining/restoring the Item.ProductCodeId ???
                var savedPCId = Item.ProductCodeId;
                ProductCodes = (await productCodeDataService.GetAllProductCodesAsync(Item.ManufacturerId)).ToList();
                if (savedPCId > 0 && Item.ProductCodeId == 0 && ProductCodes.Any(pc => pc.Id == savedPCId) == true)
                    Item.ProductCodeId = savedPCId;
            }

            await OnManufacturerChangeAsync();
        }

        private async Task OnManufacturerChangeAsync()
        {
            if (Item?.ManufacturerId != 0)
            {
                // FIX ME ??? is there a more elegant solution to retaining/restoring the Item.ProductCodeId ???
                var savedPCId = Item.ProductCodeId;
                ProductCodes = (await productCodeDataService.GetAllProductCodesAsync(Item.ManufacturerId)).ToList();
                if (savedPCId > 0 && Item.ProductCodeId == 0 && ProductCodes.Any(pc => pc.Id == savedPCId) == true)
                    Item.ProductCodeId = savedPCId;
            }
            
            OnProductCodeChange();
        }

        private void OnProductCodeChange()
        {
            if (Item != null && ProductCodes != null)
            {
                var saleCode = ProductCodes.FirstOrDefault(pc => pc.Id == Item.ProductCodeId)?.SaleCode;
                if (saleCode != null)
                    SaleCode = saleCode.Code + " - " + saleCode.Name;
                else
                    SaleCode = string.Empty;
            }
        }
    }
}
