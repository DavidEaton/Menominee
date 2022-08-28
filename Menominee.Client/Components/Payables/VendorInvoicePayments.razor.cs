using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using Menominee.Client.Services.Payables.PaymentMethods;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoicePayments : ComponentBase
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Inject]
        public IVendorInvoicePaymentMethodDataService paymentMethodDataService { get; set; }

        [Parameter]
        public IList<VendorInvoicePaymentToWrite> Payments { get; set; }

        public TelerikGrid<VendorInvoicePaymentToWrite> Grid { get; set; }

        //[CascadingParameter]
        //IModalService ModalService { get; set; }
        private IEnumerable<VendorInvoicePaymentToWrite> SelectedPayments { get; set; } = Enumerable.Empty<VendorInvoicePaymentToWrite>();
        private VendorInvoicePaymentToWrite SelectedPayment { get; set; }
        private long SelectedId { get; set; } = 0;      // TODO: SelectedId won't work for new items since their Ids will all be 0
        private IReadOnlyList<VendorInvoicePaymentMethodToReadInList> PaymentMethods = new List<VendorInvoicePaymentMethodToReadInList>();
        private bool parametersSet = false;
        private long paymentMethodId { get; set; } = 0;

        protected override async Task OnInitializedAsync()
        {
            PaymentMethods = (await paymentMethodDataService.GetAllPaymentMethodsAsync()).ToList();
            //await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            if (parametersSet)
                return;
            parametersSet = true;

            if (Payments.Count > 0)
            {
                //SelectedPayment = Payments.FirstOrDefault();
                //SelectedPayments = new List<VendorInvoicePaymentToWrite> { SelectedPayment };
                //SelectedId = SelectedPayment.Id;
            }

            Grid.Rebind();
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        PaymentMethods = (await paymentMethodDataService.GetAllPaymentMethodsAsync()).ToList();
        //    }
        //}

        private async Task OnPaymentMethodChangeAsync()
        {
            //if (paymentMethodId > 0)
            //    SelectedPayment.PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertReadToWriteDto(await paymentMethodDataService.GetPaymentMethodAsync(paymentMethodId));
            //SelectedPayment.PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertReadToWriteDto(await paymentMethodDataService.GetPaymentMethodAsync(SelectedPayment.PaymentMethod.Id));
            //SelectedPayment.PaymentMethodId = SelectedPayment.PaymentMethod.Id;
            //paymentMethodId = SelectedPayment.PaymentMethod?.Id ?? 0;
        }

        private void OnEdit()
        {
        }

        private void OnNew()
        {
            paymentMethodId = 0;
            VendorInvoicePaymentToWrite paymentToAdd = new();
            paymentToAdd.PaymentMethod = new();
            Payments.Add(paymentToAdd);
            Grid.Rebind();
            SelectedPayment = paymentToAdd;
            SelectedPayments = new List<VendorInvoicePaymentToWrite> { SelectedPayment };
        }

        private void OnDelete()
        {
        }

        private void OnInsertBalanceClick()
        {
        }

        protected void OnPaymentSelect(IEnumerable<VendorInvoicePaymentToWrite> payments)
        {
            SelectedPayment = payments.FirstOrDefault();
            SelectedPayments = new List<VendorInvoicePaymentToWrite> { SelectedPayment };
            //paymentMethodId = SelectedPayment.PaymentMethod?.Id ?? 0;
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            //SelectedId = (args.Item as VendorInvoicePaymentToWrite).Id;
        }

        //public class PaymentMethodListItem
        //{
        //    public string Text { get; set; }
        //    public long Id { get; set; }
        //}

        //private List<PaymentMethodListItem> paymentMethodList { get; set; } = new List<PaymentMethodListItem>();
    }
}
