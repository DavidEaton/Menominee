namespace MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentToReadInList
    {
        public long Id { get; set; }
        public string PaymentMethodName { get; set; }
        public double Amount { get; set; }
    }
}
