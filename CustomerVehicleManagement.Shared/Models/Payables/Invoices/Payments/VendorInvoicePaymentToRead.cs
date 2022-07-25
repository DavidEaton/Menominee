
namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentToRead
    {
        public long Id { get; set; }
        public long VendorInvoiceId { get; set; }
        public VendorInvoicePaymentMethodToRead PaymentMethod { get; set; }
        public double Amount { get; set; }
    }
}
