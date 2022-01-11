namespace MenomineePlayWASM.Shared.Dtos.Payables.Invoices
{
    public class VendorInvoiceToReadInList
    {
        public long Id { get; set; }
        public long VendorId { get; set; }
        public string VendorCode { get; set; }
        public string Name { get; set; }
        public string DateCreated { get; set; }
        public string DatePosted { get; set; }
        public string Status { get; set; }
        public string InvoiceNumber { get; set; }
        public double Total { get; set; }
    }
}
