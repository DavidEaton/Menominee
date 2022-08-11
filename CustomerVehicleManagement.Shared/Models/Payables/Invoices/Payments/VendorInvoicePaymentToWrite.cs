namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentToWrite
    {
        public long Id { get; set; } = 0;
        public long VendorInvoiceId { get; set; } = 0;
        public VendorInvoicePaymentMethodToWrite PaymentMethod { get; set; }
        //public long PaymentMethodId { get; set; }
        public double Amount { get; set; } = 0.0;
    }
}
