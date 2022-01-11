namespace MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Items
{
    public class VendorInvoiceItemToReadInList
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public double Extended { get; set; }
    }
}
