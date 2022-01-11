namespace MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentToRead
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public double Amount { get; set; }
    }
}
