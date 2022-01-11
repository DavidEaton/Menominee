using Blazored.Modal;
using Blazored.Modal.Services;
using MenomineePlayWASM.Client.Components.Shared;
using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Taxes;
using MenomineePlayWASM.Shared.Entities.Payables.Invoices;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MenomineePlayWASM.Client.Components.Payables
{
    public partial class VendorInvoiceTaxes : ComponentBase
    {
        //[Inject]
        //public HttpClient HttpClient { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        //[Inject]
        //private IVendorInvoiceTaxRepository vendorInvoiceTaxRepository { get; set; }

        [Parameter]
        public IList<VendorInvoiceTaxToWrite> Taxes { get; set; }
        //public List<VendorInvoiceTaxToRead> Taxes { get; set; }
        //public List<VendorInvoiceTaxToEdit> TaxesUpdate { get; set; }

        //private SfGrid<VendorInvoiceTaxToWrite> Grid { get; set; }
        //private SfGrid<VendorInvoiceTaxToEdit> Grid { get; set; }
        public TelerikGrid<VendorInvoiceTaxToWrite> Grid { get; set; }

        [CascadingParameter]
        IModalService ModalService { get; set; }

        private long SelectedId = 0;

        protected override void OnParametersSet()
        {
            if (Taxes != null)
            {
                foreach (var tax in Taxes)
                {
                    tax.TaxName = "State Tax";
                }
            }
            base.OnParametersSet();
        }

        private void OnEdit()
        {
            if (SelectedId == 0)
            {
                var parameters = new ModalParameters();
                parameters.Add(nameof(ModalMessage.Message), "Please select a tax to edit.");
                ModalService.Show<ModalMessage>("Edit Tax", parameters);
                return;
            }
        }

        private void OnNew()
        {
            //navigationManager.NavigateTo("/payables/returns/create/");
        }

        private void OnDelete()
        {
            //navigationManager.NavigateTo("/payables/.../");
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedId = (args.Item as VendorInvoiceTaxToWrite).Id;
        }

        //protected override Task OnInitializedAsync()
        //{
        //    foreach (var tax in Taxes)
        //    {
        //        TaxesUpdate.Add(
        //            new VendorInvoiceTaxToEdit
        //            {
        //                Amount = tax.Amount,
        //                Id = tax.Id,
        //                InvoiceId = tax.InvoiceId,
        //                Order = tax.Order,
        //                TaxId = tax.TaxId
        //            });
        //    }
        //    return base.OnInitializedAsync();
        //}
    }
}
