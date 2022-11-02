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

        //public IEnumerable<SalesTaxListItem> SelectedTaxes { get; set; } = Enumerable.Empty<SalesTaxListItem>();
        //public SalesTaxListItem SelectedTax { get; set; }
        public IEnumerable<VendorInvoiceTaxToWrite> SelectedTaxes { get; set; } = Enumerable.Empty<VendorInvoiceTaxToWrite>();
        public VendorInvoiceTaxToWrite SelectedTax { get; set; }

        //public TelerikGrid<SalesTaxListItem> Grid { get; set; }
        public TelerikGrid<VendorInvoiceTaxToWrite> Grid { get; set; }

        private IReadOnlyList<SalesTaxToReadInList> SalesTaxes = new List<SalesTaxToReadInList>();

        //public int SelectedItemIndex
        //{
        //    get => selectedItemIndex;
        //    set
        //    {
        //        selectedItemIndex = value;
        //    }
        //}

        //private int selectedItemIndex = -1;

        //private List<SalesTaxListItem> salesTaxList { get; set; } = new List<SalesTaxListItem>();

        protected override async Task OnInitializedAsync()
        {
            SalesTaxes = (await SalesTaxDataService.GetAllSalesTaxesAsync()).ToList();
            SalesTaxes.OrderByDescending(tax => tax.Order);

            //foreach (SalesTaxToReadInList tax in SalesTaxes)
            //{
            //    salesTaxList.Add(new SalesTaxListItem { Id = tax.Id, Description = tax.Description });
            //}
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

            //if (salesTaxList?.Count > 0)
            //{
            //    if (Taxes?.Count > 0)
            //    {
            //        foreach (VendorInvoiceTaxToWrite invoiceTax in Taxes)
            //        {
            //            var salesTax = salesTaxList.FirstOrDefault(x => x.Id == invoiceTax.SalesTax.Id);
            //            if (salesTax != null)
            //            {
            //                salesTax.Amount = invoiceTax.Amount;
            //            }
            //        }
            //    }

            //    SelectTax(salesTaxList.FirstOrDefault());
            //    Grid?.Rebind();
            //}
        }

        //private void SelectTax(SalesTaxListItem salesTax)
        //{
        //    SelectedTax = salesTax;
        //    SelectedTaxes = new List<SalesTaxListItem> { SelectedTax };
        //    SelectedItemIndex = SelectedTaxes.IndexOf(SelectedTax);
        //}

        private void SelectTax(VendorInvoiceTaxToWrite tax)
        {
            SelectedTax = tax;
            SelectedTaxes = new List<VendorInvoiceTaxToWrite> { SelectedTax };
            //SelectedItemIndex = Taxes.IndexOf(SelectedTax);
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectTax(args.Item as VendorInvoiceTaxToWrite);
            //SelectTax(args.Item as SalesTaxListItem);
            //SelectedTax = args.Item as SalesTaxListItem;
            //SelectedTaxes = new List<SalesTaxListItem> { SelectedTax };
            //SelectedItemIndex = SelectedTaxes.IndexOf(SelectedTax);
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
            return (InvoiceTotals.TaxableTotal * taxRate) / 100.0;
        }

        //public class SalesTaxListItem
        //{
        //    public long Id { get; set; }
        //    public string Description { get; set; }
        //    public double Amount { get; set; }
        //}
    }
}
