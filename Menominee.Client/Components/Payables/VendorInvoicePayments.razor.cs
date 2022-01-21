using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoicePayments : ComponentBase
    {
        //[Inject]
        //public HttpClient HttpClient { get; set; }
        VendorInvoicePaymentToWrite moops;
       [Inject]
        private NavigationManager navigationManager { get; set; }

        //[Inject]
        //private IVendorInvoicePaymentRepository vendorInvoicePaymentRepository { get; set; }

        [Parameter]
        public IList<VendorInvoicePaymentToWrite> Payments { get; set; }
        //public List<VendorInvoicePaymentToEdit> PaymentsUpdate { get; set; }
        //public List<VendorInvoicePaymentToRead> Payments { get; set; }

        //private SfGrid<VendorInvoicePaymentToWrite> Grid { get; set; }
        //private SfGrid<VendorInvoicePaymentToEdit> Grid { get; set; }
        public TelerikGrid<VendorInvoicePaymentToWrite> Grid { get; set; }

        //[CascadingParameter]
        //IModalService ModalService { get; set; }

        //protected override Task OnInitializedAsync()
        //{
        //    foreach (var payment in Payments)
        //    {
        //        PaymentsUpdate.Add(
        //            new VendorInvoicePaymentToEdit
        //            {
        //                Id = payment.Id,
        //                InvoiceId = payment.InvoiceId,
        //                PaymentMethod = payment.PaymentMethod,
        //                PaymentMethodName = payment.PaymentMethodName,
        //                Amount = payment.Amount
        //            });
        //    }
        //    return base.OnInitializedAsync();
        //}

        protected override void OnParametersSet()
        {
            if (Payments != null)
            {
                foreach (var payment in Payments)
                {
                    payment.PaymentMethodName = "VISA";
                }
            }
            base.OnParametersSet();
        }

        private long SelectedId = 0;

        private void OnEdit()
        {
            //if (selectedId == 0)
            //{
            //    var parameters = new ModalParameters();
            //    parameters.Add(nameof(ModalMessage.Message), "Please select a credit return to edit.");
            //    ModalService.Show<ModalMessage>("Edit Credit Return", parameters);

            //}
            //else
            //{
            //    navigationManager.NavigateTo($"/payables/returns/edit/{selectedId}");
            //}
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
            SelectedId = (args.Item as VendorInvoicePaymentToWrite).Id;
        }
    }
}
