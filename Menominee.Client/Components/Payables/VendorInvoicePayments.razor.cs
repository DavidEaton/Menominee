﻿using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
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
        public IVendorInvoicePaymentMethodDataService PaymentMethodDataService { get; set; }

        [Parameter]
        public IList<VendorInvoicePaymentToWrite> Payments { get; set; }

        public VendorInvoicePaymentToWrite Payment { get; set; }
        public TelerikGrid<VendorInvoicePaymentToWrite> Grid { get; set; }

        private IReadOnlyList<VendorInvoicePaymentMethodToReadInList> PaymentMethods = new List<VendorInvoicePaymentMethodToReadInList>();

        private long SelectedId = 0;
        private long paymentMethodId = 0;
        protected override async Task OnInitializedAsync()
        {
            PaymentMethods = (await PaymentMethodDataService.GetAllPaymentMethodsAsync()).ToList();

        }

        private void OnEdit(GridRowClickEventArgs args)
        {
            //Payment = 
            if (args is not null)
                SelectedId = (args.Item as VendorInvoicePaymentToWrite).Id;

            // open dialog with FormMode == Edit

            // TEMPORARILY Edit an existing payment TO TEST
            //var payment = Payments.FirstOrDefault(payment => payment.Id == SelectedId);

            //payment.Amount = 99.9;
        }

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

            Grid.Rebind();
        }

        private void OnDelete()
        {
        }

        private void OnInsertBalanceClick()
        {
        }

        private void OnPaymentMethodChange()
        {
            if (paymentMethodId > 0 && Payment?.PaymentMethod?.Id != paymentMethodId)
                Payment.PaymentMethod = VendorInvoicePaymentMethodHelper.ConvertReadInListToReadDto(
                    PaymentMethods.FirstOrDefault(paymentMethod => paymentMethod.Id == paymentMethodId));
        }

        protected void OnPaymentSelect(IEnumerable<VendorInvoicePaymentToWrite> payments)
        {
            //SelectedPayment = payments.FirstOrDefault();
            //SelectedPayments = new List<VendorInvoicePaymentToWrite> { SelectedPayment };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedId = (args.Item as VendorInvoicePaymentToWrite).Id;
        }
    }
}
