﻿using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryTireEditor
    {
        [Inject]
        public IManufacturerDataService manufacturerDataService { get; set; }

        [Inject]
        public IProductCodeDataService productCodeDataService { get; set; }

        [Parameter]
        public InventoryItemToWrite Item { get; set; }

        [Parameter]
        public string Title { get; set; } = "Edit Tire";

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        private IReadOnlyList<ManufacturerToReadInList> Manufacturers = null;
        private IReadOnlyList<ProductCodeToReadInList> ProductCodes = null;
        private string SaleCode = string.Empty;
        private bool parametersSet = false;
        private long manufacturerId = 0;
        private long productCodeId = 0;

        protected override async Task OnInitializedAsync()
        {
            Manufacturers = (await manufacturerDataService.GetAllManufacturersAsync())
                                                          .Where(mfr => mfr.Prefix?.Length > 0
                                                                     && mfr.Code != StaticManufacturerCodes.Custom
                                                                     && mfr.Code != StaticManufacturerCodes.Package)
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

            if (Item.Tire == null)
            {
                Item.Tire = new();
                Item.ItemType = InventoryItemType.Tire.ToString();

                Title = "Add Tire";
            }

            //if (Item?.ManufacturerId != 0)
            //{
            //    // FIX ME ??? is there a more elegant solution to retaining/restoring the Item.ProductCodeId ???
            //    var savedPCId = Item.ProductCodeId;
            //    ProductCodes = (await productCodeDataService.GetAllProductCodesAsync(Item.ManufacturerId)).ToList();
            //    if (savedPCId > 0 && Item.ProductCodeId == 0 && ProductCodes.Any(pc => pc.Id == savedPCId) == true)
            //        Item.ProductCodeId = savedPCId;
            //}

            
            await OnManufacturerChangeAsync();
        }

        private async Task OnManufacturerChangeAsync()
        {
            //if (Item?.ManufacturerId != 0)
            //{
            //    // FIX ME ??? is there a more elegant solution to retaining/restoring the Item.ProductCodeId ???
            //    var savedPCId = Item.ProductCodeId;
            //    ProductCodes = (await productCodeDataService.GetAllProductCodesAsync(Item.ManufacturerId)).ToList();
            //    if (savedPCId > 0 && Item.ProductCodeId == 0 && ProductCodes.Any(pc => pc.Id == savedPCId) == true)
            //        Item.ProductCodeId = savedPCId;
            //}
            //else
            //{
            //    ProductCodes = new List<ProductCodeToReadInList>();
            //}

            //OnProductCodeChange();
            if (manufacturerId > 0 && Item.Manufacturer?.Id != manufacturerId)
                Item.Manufacturer = await manufacturerDataService.GetManufacturerAsync(manufacturerId);

            if (Item?.Manufacturer is not null)
            {
                long savedProductCodeId = productCodeId;
                ProductCodes = (await productCodeDataService.GetAllProductCodesAsync(manufacturerId)).ToList();
                if (savedProductCodeId > 0 && Item.ProductCode?.Id == 0 && ProductCodes.Any(pc => pc.Id == savedProductCodeId) == true)
                    Item.ProductCode = (await productCodeDataService.GetProductCodeAsync(savedProductCodeId));
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

            if (Item != null && Item.ProductCode != null)
                SaleCode = Item.ProductCode?.SaleCode.Code + " - " + Item.ProductCode?.Name;
            else
                SaleCode = string.Empty;
        }
    }
}
