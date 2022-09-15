namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentToWrite
    {
        public VendorInvoicePaymentMethodToRead PaymentMethod { get; set; }
        public double Amount { get; set; } = 0.0;
    }
}
