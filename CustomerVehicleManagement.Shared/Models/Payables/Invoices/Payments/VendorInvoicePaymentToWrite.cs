﻿namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentToWrite
    {
        public long Id { get; set; } = 0;
        public long InvoiceId { get; set; } = 0;
        public int PaymentMethod { get; set; } = 0;
        public string PaymentMethodName { get; set; } = string.Empty;
        public double Amount { get; set; } = 0.0;
    }
}