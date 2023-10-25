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
        public ISalesTaxDataService? SalesTaxDataService { get; set; }

        [Parameter]
        public IList<VendorInvoiceTaxToWrite>? Taxes { get; set; }

        [Parameter]
        public Action? OnCalculateTotals { get; set; }

        [CascadingParameter]
        public InvoiceTotals? InvoiceTotals { get; set; }

        [CascadingParameter]
        public FormMode FormMode { get; set; }

        private bool CanEdit { get; set; } = false;
        private IEnumerable<VendorInvoiceTaxToWrite> SelectedTaxes { get; set; } = Enumerable.Empty<VendorInvoiceTaxToWrite>();
        private VendorInvoiceTaxToWrite? SelectedTax { get; set; }
        private TelerikGrid<VendorInvoiceTaxToWrite>? Grid { get; set; }
        private IReadOnlyList<SalesTaxToReadInList> SalesTaxes = new List<SalesTaxToReadInList>();

        protected override async Task OnInitializedAsync()
        {
            await GetSalesTaxes();
        }

        protected override async Task OnParametersSetAsync()
        {
            CanEdit = FormMode is FormMode.Add or FormMode.Edit;
            await AddMissingTaxesToInvoice();
            SortAndSelectTaxes();
        }

        private async Task GetSalesTaxes()
        {
            if (SalesTaxDataService is not null)
            {
                var result = await SalesTaxDataService.GetAllAsync();

                if (result.IsSuccess)
                {
                    SalesTaxes = result.Value
                        .OrderBy(tax => tax.Order)
                        .ToList();
                }
            }
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
                        Taxes?.Add(CreateVendorInvoiceTax(result.Value));
                    }
                }
            }
        }

        private bool TaxIsMissing(long salesTaxId)
        {
            return !Taxes?.Any(tax => tax.SalesTax?.Id == salesTaxId) ?? false;
        }

        private async Task<Result<SalesTaxToRead>> GetSalesTax(long salesTaxId)
        {
            return SalesTaxDataService is not null
                ? await SalesTaxDataService.GetAsync(salesTaxId)
                : Result.Failure<SalesTaxToRead>($"Failed to retrieve sales tax {salesTaxId}.");
        }

        private static VendorInvoiceTaxToWrite CreateVendorInvoiceTax(SalesTaxToRead salesTax)
        {
            return new VendorInvoiceTaxToWrite
            {
                SalesTax = salesTax
            };
        }

        private void SortAndSelectTaxes()
        {
            if (Taxes is not null)
            {
                Taxes = Taxes.OrderBy(tax => tax.SalesTax.Order).ToList();
                var tax = Taxes.FirstOrDefault();
                if (tax is not null)
                {
                    SelectTax(tax);
                }
                Grid?.Rebind();
            }
        }

        private void SelectTax(VendorInvoiceTaxToWrite tax)
        {
            SelectedTax = tax;
            SelectedTaxes = new List<VendorInvoiceTaxToWrite> { SelectedTax };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectTax((VendorInvoiceTaxToWrite)args.Item);
        }

        private void OnCalcTaxClick(GridCommandEventArgs args)
        {
            SelectTax((VendorInvoiceTaxToWrite)args.Item);

            if (SelectedTax is not null)
            {
                SelectedTax.Amount = CalculateTax(SelectedTax.SalesTax.PartTaxRate);
                OnCalculateTotals?.Invoke();
            }
        }

        private void OnTaxAmountChange()
        {
            if (SelectedTax is not null)
            {
                OnCalculateTotals?.Invoke();
            }
        }

        private double CalculateTax(double taxRate)
        {
            return InvoiceTotals is not null
                ? Math.Round(InvoiceTotals.TaxableTotal * taxRate / 100.0, 2)
                : 0.0;
        }
    }
}
