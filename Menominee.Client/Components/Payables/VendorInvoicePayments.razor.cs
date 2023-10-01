using CSharpFunctionalExtensions;
using Menominee.Client.Components.Settings.Pages;
using Menominee.Client.Services.Payables.PaymentMethods;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Payables.Invoices;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoicePayments : ComponentBase
    {
        [Inject]
        public IVendorInvoicePaymentMethodDataService PaymentMethodDataService { get; set; }

        [Parameter]
        public IList<VendorInvoicePaymentToWrite> Payments { get; set; }

        [Parameter]
        public Action OnCalculateTotals { get; set; }

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        [CascadingParameter]
        public InvoiceTotals InvoiceTotals { get; set; }

        [CascadingParameter]
        public FormMode FormMode { get; set; }

        [Inject]
        ILogger<VendorPaymentMethodsPage> Logger { get; set; }

        public IEnumerable<VendorInvoicePaymentToWrite> SelectedPayments { get; set; } = Enumerable.Empty<VendorInvoicePaymentToWrite>();

        public VendorInvoicePaymentToWrite Payment { get; set; }
        public TelerikGrid<VendorInvoicePaymentToWrite> Grid { get; set; }

        private IReadOnlyList<VendorInvoicePaymentMethodToReadInList> PaymentMethods = new List<VendorInvoicePaymentMethodToReadInList>();

        private int popupHeight = 260;
        private bool CanEdit { get; set; } = false;

        private bool CanDelete()
        {
            return Payments?.Count > 0;
        }

        protected override async Task OnInitializedAsync()
        {
            await PaymentMethodDataService.GetAllAsync()
            .Match(
                success => PaymentMethods = success,
                failure => Logger.LogError(failure));

            if (PaymentMethods.Count < 9)
            {
                popupHeight = PaymentMethods.Count * 32;
            }
        }

        protected override void OnParametersSet()
        {
            if (Payments?.Count > 0)
            {
                SelectPayment(Payments.FirstOrDefault());
            }
            CanEdit = FormMode == FormMode.Add || FormMode == FormMode.Edit;
        }

        private void SelectPayment(VendorInvoicePaymentToWrite payment)
        {
            Payment = payment;
            SelectedPayments = new List<VendorInvoicePaymentToWrite> { Payment };
        }

        private void Add()
        {
            // TODO: Validation issue -- Enterprise doesn't allow more than one payment method to be selected that 
            //       is set up to be reconciled by another vendor
            Payment = new()
            {
                PaymentMethod = new()
            };
            Payments.Add(Payment);
            SelectPayment(Payment);
            Grid.Rebind();
        }

        private async Task OnDelete()
        {
            if (Payment is not null
            && await Dialogs.ConfirmAsync("Are you sure you want to remove the selected payment?", "Remove Payment"))
            {
                Payments.Remove(Payment);
                SelectPayment(Payments.Count > 0 ? Payments.FirstOrDefault() : null);
                Grid.Rebind();

                OnCalculateTotals?.Invoke();
            }
        }

        private void OnInsertBalanceClick(GridCommandEventArgs args)
        {
            SelectPayment(args.Item as VendorInvoicePaymentToWrite);

            var payments = Math.Round(InvoiceTotals.Payments - Payment.Amount, 2);
            Payment.Amount = Math.Round((InvoiceTotals.Total - payments), 2);
            OnCalculateTotals?.Invoke();
        }

        private void OnPaymentMethodChange()
        {
            Payment.PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertReadInListToReadDto(
                PaymentMethods.FirstOrDefault(paymentMethod => paymentMethod.Id == Payment.PaymentMethod.Id));
        }

        private void OnPaymentAmountChange()
        {
            OnCalculateTotals?.Invoke();
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectPayment(args.Item as VendorInvoicePaymentToWrite);
        }
    }
}
