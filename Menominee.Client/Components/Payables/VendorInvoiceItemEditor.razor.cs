using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Client.Services.SaleCodes;
using Menominee.Client.Shared;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Inventory.InventoryItems;
using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.Payables.Invoices.LineItems;
using Menominee.Shared.Models.SaleCodes;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoiceItemEditor
    {
        [Inject]
        public IManufacturerDataService ManufacturerDataService { get; set; }

        [Inject]
        public IProductCodeDataService ProductCodeDataService { get; set; }

        [Inject]
        public ISaleCodeDataService SaleCodeDataService { get; set; }

        [Parameter]
        public VendorInvoiceLineItemToWrite LineItem { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        private bool ItemSelectDialogVisible { get; set; } = false;
        public InventoryItemToReadInList SelectedInventoryItem { get; set; }
        private bool CanEdit { get; set; } = false;
        private IReadOnlyList<ManufacturerToReadInList> Manufacturers = new List<ManufacturerToReadInList>();
        private IList<InvoiceItemType> ItemTypes { get; set; } = new List<InvoiceItemType>();
        private IReadOnlyList<SaleCodeToReadInList> SaleCodes { get; set; } = new List<SaleCodeToReadInList>();

        private long saleCodeId = 0;
        private long manufacturerId = 0;
        public string Title { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var resultTask = await ManufacturerDataService.GetAllAsync();
            if (resultTask.IsSuccess)
            {
                Manufacturers = resultTask.Value
                                             .Where(mfr => mfr.Prefix?.Length > 0
                                                        && mfr.Id != StaticManufacturerCodes.Custom
                                                        && mfr.Id != StaticManufacturerCodes.Package)
                                             .OrderBy(mfr => mfr.Prefix)
                                             .ToList();
            }

            if (resultTask.IsFailure)
                Manufacturers = new List<ManufacturerToReadInList>();

            SaleCodes = (await SaleCodeDataService.GetAllAsync())
                                                          .OrderBy(saleCode => saleCode.Code)
                                                          .ToList();

            foreach (VendorInvoiceLineItemType itemType in Enum.GetValues(typeof(VendorInvoiceLineItemType)))
            {
                ItemTypes.Add(new InvoiceItemType { Text = EnumExtensions.GetDisplayName(itemType), Value = itemType });
            }

            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            if (LineItem?.Item?.Manufacturer is not null)
                manufacturerId = LineItem.Item.Manufacturer.Id;

            if (LineItem is not null && LineItem.Item is not null && LineItem.Item.SaleCode is not null)
                if (LineItem.Item.SaleCode.Id != 0)
                    saleCodeId = LineItem.Item.SaleCode.Id;

            Title = FormTitle.BuildTitle(FormMode, "Item");
            CanEdit = FormMode == FormMode.Add || FormMode == FormMode.Edit;
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
                LineItem.Item.SaleCode = SaleCodeHelper.ConvertReadInListToReadDto(
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
                var productCode = await ProductCodeDataService.GetAsync(SelectedInventoryItem.ProductCode.Id);

                var manufacturerResult = await ManufacturerDataService.GetAsync(StaticManufacturerCodes.Miscellaneous);
                if (manufacturerResult.IsSuccess)
                    LineItem.Item.Manufacturer = manufacturerResult.Value;

                if (productCode.Value.SaleCode?.Code.Length > 0)
                    LineItem.Item.SaleCode = productCode.Value.SaleCode;

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
            public VendorInvoiceLineItemType Value { get; set; }
        }
    }
}
