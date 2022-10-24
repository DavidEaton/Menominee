using CustomerVehicleManagement.Shared.Models.Inventory;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using Menominee.Client.Services.Payables.PaymentMethods;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IEnumerable<VendorInvoicePaymentToWrite> SelectedPayments { get; set; } = Enumerable.Empty<VendorInvoicePaymentToWrite>();

        public VendorInvoicePaymentToWrite Payment { get; set; }
        public TelerikGrid<VendorInvoicePaymentToWrite> Grid { get; set; }

        private IReadOnlyList<VendorInvoicePaymentMethodToReadInList> PaymentMethods = new List<VendorInvoicePaymentMethodToReadInList>();

        public int SelectedItemIndex
        {
            get => selectedItemIndex;
            set
            {
                selectedItemIndex = value;
                CanDelete = selectedItemIndex >= 0;
            }
        }

        private int selectedItemIndex = -1;
        //private long SelectedId = 0;
        //private long paymentMethodId = 0;

        private bool CanDelete { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            PaymentMethods = (await PaymentMethodDataService.GetAllPaymentMethodsAsync()).ToList();
        }

        protected override void OnParametersSet()
        {
            if (Payments?.Count > 0)
            {
                Payment = Payments.FirstOrDefault();
                SelectedPayments = new List<VendorInvoicePaymentToWrite> { Payment };
                SelectedItemIndex = Payments.IndexOf(Payment);
                //paymentMethodId = Payment.PaymentMethod.Id;
            }
        }

        //private void OnEdit(GridRowClickEventArgs args)
        //{
        //    //Payment = 
        //    if (args is not null)
        //        SelectedId = (args.Item as VendorInvoicePaymentToWrite).Id;

        //    // open dialog with FormMode == Edit

        //    // TEMPORARILY Edit an existing payment TO TEST
        //    //var payment = Payments.FirstOrDefault(payment => payment.Id == SelectedId);

        //    //payment.Amount = 99.9;
        //}

        private void Add()
        {
            // open dialog with FormMode == Add

            // TEMPORARILY Add a new tax to collection TO TEST
            //Payments.Add(new()
            //{
            //    Amount = 88.0,
            //    PaymentMethod = new()
            //    {
            //        Id = 1,
            //        Name = "Credit Card",
            //        IsActive = true,
            //        IsOnAccountPaymentType = false
            //    }
            //});

            Payment = new();
            Payment.PaymentMethod = new();
            Payments.Add(Payment);
            //paymentMethodId = 0;
            SelectedItemIndex = Payments.Count - 1;
            SelectedPayments = new List<VendorInvoicePaymentToWrite> { Payment };
            Grid.Rebind();
        }

        private async Task OnDelete()
        {
            if (Payment is not null
            && await Dialogs.ConfirmAsync("Are you sure you want to remove the selected payment?", "Remove Payment"))
            {
                Payments.Remove(Payment);
                if (Payments.Count > 0)
                {
                    Payment = Payments.FirstOrDefault();
                    //paymentMethodId = Payment.PaymentMethod.Id;
                    SelectedPayments = new List<VendorInvoicePaymentToWrite> { Payment };
                    SelectedItemIndex = Payments.IndexOf(Payment);
                }
                else
                {
                    Payment = null;
                    //paymentMethodId = 0;
                    SelectedPayments = new List<VendorInvoicePaymentToWrite>();
                    SelectedItemIndex = -1;
                }
                Grid.Rebind();

                OnCalculateTotals?.Invoke();
                //OnCalculateTotals();
            }
        }

        private void OnInsertBalanceClick(GridCommandEventArgs args)
        {
            Payment = args.Item as VendorInvoicePaymentToWrite;
            SelectedItemIndex = Payments.IndexOf(Payment);
            SelectedPayments = new List<VendorInvoicePaymentToWrite> { Payment };

            var payments = InvoiceTotals.Payments - Payment.Amount;
            Payment.Amount = InvoiceTotals.Total - payments;
            OnCalculateTotals?.Invoke();
        }

        private void OnPaymentMethodChange()
        {
            //if (paymentMethodId > 0 && Payment?.PaymentMethod?.Id != paymentMethodId)
            //{
            //    Payment.PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertReadInListToReadDto(
            //        PaymentMethods.FirstOrDefault(paymentMethod => paymentMethod.Id == paymentMethodId));
            //}
            Payment.PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertReadInListToReadDto(
                PaymentMethods.FirstOrDefault(paymentMethod => paymentMethod.Id == Payment.PaymentMethod.Id));
        }

        private void OnPaymentAmountChange()
        {
            OnCalculateTotals?.Invoke();
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            //SelectedId = (args.Item as VendorInvoicePaymentToWrite).Id;

            Payment = args.Item as VendorInvoicePaymentToWrite;
            //paymentMethodId = Payment.PaymentMethod.Id;
            SelectedItemIndex = Payments.IndexOf(Payment);
            SelectedPayments = new List<VendorInvoicePaymentToWrite> { Payment };
        }
    }
}
