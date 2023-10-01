using CSharpFunctionalExtensions;
using Menominee.Client.Services.Taxes;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Payables.Invoices;
using Menominee.Shared.Models.Payables.Invoices.Taxes;
using Menominee.Shared.Models.Taxes;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoiceTaxes : ComponentBase
    {
        [Inject]
        public ISalesTaxDataService SalesTaxDataService { get; set; }

        [Parameter]
        public IList<VendorInvoiceTaxToWrite> Taxes { get; set; }

        [Parameter]
        public Action OnCalculateTotals { get; set; }

        [CascadingParameter]
        public InvoiceTotals InvoiceTotals { get; set; }

        [CascadingParameter]
        public FormMode FormMode { get; set; }

        private bool CanEdit { get; set; } = false;

        public IEnumerable<VendorInvoiceTaxToWrite> SelectedTaxes { get; set; } = Enumerable.Empty<VendorInvoiceTaxToWrite>();
        public VendorInvoiceTaxToWrite SelectedTax { get; set; }

        public TelerikGrid<VendorInvoiceTaxToWrite> Grid { get; set; }

        private IReadOnlyList<SalesTaxToReadInList> SalesTaxes = new List<SalesTaxToReadInList>();

        protected override async Task OnInitializedAsync()
        {
            var result = await SalesTaxDataService.GetAllAsync();

            if (result.IsSuccess)
                SalesTaxes = (IReadOnlyList<SalesTaxToReadInList>)result.Value.OrderByDescending(tax => tax.Order);
        }

        protected override async Task OnParametersSetAsync()
        {
            CanEdit = FormMode is FormMode.Add or FormMode.Edit;
            await AddMissingTaxesToInvoice();
            SortAndSelectTaxes();
        }

        private async Task AddMissingTaxesToInvoice()
        {
            foreach (var salesTax in SalesTaxes)
            {
                if (TaxIsMissing(salesTax.Id))
                {
                    var result = await GetSalesTax(salesTax.Id);
                    if (result.IsSuccess)
                    {
                        Taxes.Add(CreateVendorInvoiceTax(result.Value));
                    }
                }
            }
        }

        private bool TaxIsMissing(long salesTaxId)
        {
            return !Taxes.Any(tax => tax.SalesTax?.Id == salesTaxId);
        }

        private async Task<Result<SalesTaxToRead>> GetSalesTax(long salesTaxId)
        {
            return await SalesTaxDataService.GetAsync(salesTaxId);
        }

        private VendorInvoiceTaxToWrite CreateVendorInvoiceTax(SalesTaxToRead salesTax)
        {
            return new VendorInvoiceTaxToWrite
            {
                SalesTax = salesTax
            };
        }

        private void SortAndSelectTaxes()
        {
            Taxes = Taxes.OrderByDescending(tax => tax.SalesTax.Order).ToList();
            SelectTax(Taxes.FirstOrDefault());
            Grid?.Rebind();
        }

        private void SelectTax(VendorInvoiceTaxToWrite tax)
        {
            SelectedTax = tax;
            SelectedTaxes = new List<VendorInvoiceTaxToWrite> { SelectedTax };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectTax(args.Item as VendorInvoiceTaxToWrite);
        }

        private void OnCalcTaxClick(GridCommandEventArgs args)
        {
            SelectTax(args.Item as VendorInvoiceTaxToWrite);

            SelectedTax.Amount = CalculateTax(SelectedTax.SalesTax.PartTaxRate);
            OnCalculateTotals?.Invoke();
        }

        private void OnTaxAmountChange()
        {
            if (SelectedTax != null)
            {
                OnCalculateTotals?.Invoke();
            }
        }

        private double CalculateTax(double taxRate)
        {
            return Math.Round((InvoiceTotals.TaxableTotal * taxRate) / 100.0, 2);
        }
    }
}
