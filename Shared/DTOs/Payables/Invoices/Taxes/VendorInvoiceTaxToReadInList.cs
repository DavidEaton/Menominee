namespace MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Taxes
{
    public class VendorInvoiceTaxToReadInList
    {
        public long Id { get; set; }
        public int Order { get; set; }
        public string TaxName { get; set; }
        public double Amount { get; set; }
    }
}
