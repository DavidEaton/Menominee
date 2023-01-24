using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using CustomerVehicleManagement.Shared.Models.Taxes;
using Menominee.Client.Services.Taxes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            SalesTaxes = (await SalesTaxDataService.GetAllSalesTaxesAsync()).ToList();
            SalesTaxes.OrderByDescending(tax => tax.Order);
        }

        protected override async Task OnParametersSetAsync()
        {
            CanEdit = FormMode == FormMode.Add || FormMode == FormMode.Edit;

            foreach (var salesTax in SalesTaxes)
            {
                var invoiceTax = Taxes.FirstOrDefault(x => x.SalesTax?.Id == salesTax.Id);
                if (invoiceTax == null)
                {
                    var tax = (await SalesTaxDataService.GetSalesTaxAsync(salesTax.Id));
                    Taxes.Add(new VendorInvoiceTaxToWrite()
                    {
                        SalesTax = tax
                    });
                }
            }
            Taxes.OrderByDescending(tax => tax.SalesTax.Order);
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
