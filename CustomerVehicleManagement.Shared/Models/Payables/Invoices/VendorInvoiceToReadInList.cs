using CustomerVehicleManagement.Shared.Models.Payables.Vendors;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices
{
    public class VendorInvoiceToReadInList
    {
        public long Id { get; set; }
        public VendorToRead Vendor { get; set; }
        public string Date { get; set; }
        public string DatePosted { get; set; }
        public string Status { get; set; }
        public string InvoiceNumber { get; set; }
        public double Total { get; set; }
    }
}
