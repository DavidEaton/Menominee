﻿using CustomerVehicleManagement.Shared.Models.Inventory;
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
    public partial class InventoryDonationEditor
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

            if (Item.Id == 0)//if (Item.Donation == null)
            {
                productCodeId = 0;
                //Item.ManufacturerId = manufacturerId;
                Item.Manufacturer = ManufacturerHelper.ConvertReadToWriteDto(await manufacturerDataService.GetManufacturerAsync(StaticManufacturerCodes.Miscellaneous));
                Item.ProductCode = new();
                //Item.Donation = new();
                Item.ItemType = InventoryItemType.Donation;

                Title = "Add Donation";
            }
            else
            {
                Title = "Edit Donation";
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
    }
}

