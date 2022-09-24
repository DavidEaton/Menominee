using CustomerVehicleManagement.Shared.Models.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Client.Services.SaleCodes;
using Menominee.Client.Shared;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoiceItemEditor
    {
        [Inject]
        public IManufacturerDataService manufacturerDataService { get; set; }

        [Inject]
        public IProductCodeDataService productCodeDataService { get; set; }

        [Inject]
        public ISaleCodeDataService saleCodeDataService { get; set; }

        [Parameter]
        public VendorInvoiceLineItemToWrite LineItem { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        [Parameter]
        public FormMode Mode
        {
            get => formMode;
            set
            {
                formMode = value;
                Title = FormTitle.BuildTitle(formMode, "Item");
            }
        }

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        private bool ItemSelectDialogVisible { get; set; } = false;
        public InventoryItemToReadInList SelectedInventoryItem { get; set; }
        private FormMode formMode;
        private IReadOnlyList<ManufacturerToReadInList> Manufacturers = null;
        private IList<InvoiceItemType> ItemTypes { get; set; } = new List<InvoiceItemType>();
        private IReadOnlyList<ProductCodeToReadInList> ProductCodes { get; set; } = new List<ProductCodeToReadInList>();
        private IReadOnlyList<SaleCodeToReadInList> SaleCodes { get; set; } = new List<SaleCodeToReadInList>();
        private long saleCodeId = 0;
        private bool parametersSet = false;
        private long manufacturerId = 0;
        public string Title { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Manufacturers = (await manufacturerDataService.GetAllManufacturersAsync())
                                                          .Where(mfr => mfr.Prefix?.Length > 0
                                                          // TODO: Move where clause to api method
                                                                     && mfr.Code != StaticManufacturerCodes.Package)
                                                          .OrderBy(mfr => mfr.Prefix)
                                                          .ToList();

            ProductCodes = (await productCodeDataService.GetAllProductCodesAsync(manufacturerId)).ToList();

            SaleCodes = (await saleCodeDataService.GetAllSaleCodesAsync())
                                                          .OrderBy(saleCode => saleCode.Code)
                                                          .ToList();

            foreach (VendorInvoiceItemType itemType in Enum.GetValues(typeof(VendorInvoiceItemType)))
            {
                ItemTypes.Add(new InvoiceItemType { Text = EnumExtensions.GetDisplayName(itemType), Value = itemType });
            }

            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            //if (parametersSet)
            //    return;
            //parametersSet = true;

            if (LineItem?.Item?.Manufacturer is not null)
                manufacturerId = LineItem.Item.Manufacturer.Id;

            if (LineItem is not null && LineItem.Item is not null && LineItem.Item.SaleCode is not null)
                if (LineItem.Item.SaleCode.Id != 0)
                    saleCodeId = LineItem.Item.SaleCode.Id;

            //OnManufacturerChange();
        }

        private void OnManufacturerChange()
        {
            if (manufacturerId > 0 && LineItem?.Item?.Manufacturer?.Id != manufacturerId)
                LineItem.Item.Manufacturer = ManufacturerHelper.ConvertReadInListToReadDto(
                    Manufacturers.FirstOrDefault(manufacturer => manufacturer.Id == manufacturerId));
        }

        private void OnSaleCodeChange()
        {
            if (saleCodeId > 0 && LineItem.Item.SaleCode?.Id != saleCodeId)
                LineItem.Item.SaleCode = SaleCodeHelper.ConvertTorReadInListToReadDto(
                    SaleCodes.FirstOrDefault(saleCode => saleCode.Id == saleCodeId));
        }

        private void SelectInventoryItem()
        {
            ItemSelectDialogVisible = true;
        }

        private async Task AddItemAsync()
        {
            ItemSelectDialogVisible = false;

            if (SelectedInventoryItem is not null)
            {
                manufacturerId = SelectedInventoryItem.ManufacturerId;
                var productCode = await productCodeDataService.GetProductCodeAsync(SelectedInventoryItem.ProductCodeId);

                LineItem.Item.Manufacturer = await manufacturerDataService.GetManufacturerAsync(manufacturerId);

                if (productCode?.SaleCode?.Code.Length > 0)
                    LineItem.Item.SaleCode = productCode.SaleCode;

                LineItem.Item.PartNumber = SelectedInventoryItem.ItemNumber;
                LineItem.Item.Description = SelectedInventoryItem.Description;
            }
        }

        private void CancelAddItem()
        {
            ItemSelectDialogVisible = false;
        }

        public class InvoiceItemType
        {
            public string Text { get; set; }
            public VendorInvoiceItemType Value { get; set; }
        }
    }
}
