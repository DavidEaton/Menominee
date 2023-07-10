namespace Menominee.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentToWrite
    {
        public long Id { get; set; }
        public VendorInvoicePaymentMethodToRead PaymentMethod { get; set; }
        public double Amount { get; set; } = 0.0;
    }
}