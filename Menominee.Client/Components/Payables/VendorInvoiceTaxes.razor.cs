using Blazored.Modal.Services;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using CustomerVehicleManagement.Shared.Models.Taxes;
using Menominee.Client.Services.Taxes;
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

        //[CascadingParameter]
        //IModalService ModalService { get; set; }

        [CascadingParameter]
        public InvoiceTotals InvoiceTotals { get; set; }

        public IEnumerable<SalesTaxListItem> SelectedTaxes { get; set; } = Enumerable.Empty<SalesTaxListItem>();
        public SalesTaxListItem SelectedTax { get; set; }

        public TelerikGrid<SalesTaxListItem> Grid { get; set; }

        private IReadOnlyList<SalesTaxToReadInList> SalesTaxes = new List<SalesTaxToReadInList>();

        public int SelectedItemIndex
        {
            get => selectedItemIndex;
            set
            {
                selectedItemIndex = value;
            }
        }

        private int selectedItemIndex = -1;

        private List<SalesTaxListItem> salesTaxList { get; set; } = new List<SalesTaxListItem>();

        protected override async Task OnInitializedAsync()
        {
            SalesTaxes = (await SalesTaxDataService.GetAllSalesTaxesAsync()).ToList();
            SalesTaxes.OrderByDescending(tax => tax.Order);

            foreach (SalesTaxToReadInList tax in SalesTaxes)
            {
                salesTaxList.Add(new SalesTaxListItem { Id = tax.Id, Description = tax.Description });
            }
        }

        protected override void OnParametersSet()
        {
            if (salesTaxList?.Count > 0)
            {
                if (Taxes?.Count > 0)
                {
                    foreach (VendorInvoiceTaxToWrite invoiceTax in Taxes)
                    {
                        var salesTax = salesTaxList.FirstOrDefault(x => x.Id == invoiceTax.SalesTax.Id);
                        if (salesTax != null)
                        {
                            salesTax.Amount = invoiceTax.Amount;
                        }
                    }
                }

                SelectedTax = salesTaxList.FirstOrDefault();
                SelectedTaxes = new List<SalesTaxListItem> { SelectedTax };
                SelectedItemIndex = SelectedTaxes.IndexOf(SelectedTax);
                Grid?.Rebind();
            }
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedTax = args.Item as SalesTaxListItem;
            SelectedTaxes = new List<SalesTaxListItem> { SelectedTax };
            SelectedItemIndex = SelectedTaxes.IndexOf(SelectedTax);
        }

        public class SalesTaxListItem
        {
            public long Id { get; set; }
            public string Description { get; set; }
            public double Amount { get; set; }
        }
    }
}
