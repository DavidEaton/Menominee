namespace MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Taxes
{
    public class VendorInvoiceTaxToWrite
    {
        public long Id { get; set; } = 0;
        public long InvoiceId { get; set; } = 0;
        public int Order { get; set; } = 0;
        public int TaxId { get; set; } = 0;
        public string TaxName { get; set; } = string.Empty;
        public double Amount { get; set; } = 0.0;
    }
}
