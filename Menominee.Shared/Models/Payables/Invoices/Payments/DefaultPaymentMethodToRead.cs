namespace Menominee.Shared.Models.Payables.Invoices.Payments
{
    public class DefaultPaymentMethodToRead
    {
        public VendorInvoicePaymentMethodToRead PaymentMethod { get; set; }
        public bool AutoCompleteDocuments { get; set; }
    }
}