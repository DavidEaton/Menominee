using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems.Items;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Common.Enums;
using System;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems
{
    public class VendorInvoiceLineItemToRead
    {
        public long Id { get; set; }
        public VendorToRead Vendor { get; set; }
        public VendorInvoiceItemType Type { get; set; }
        public VendorInvoiceItemToRead Item { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public string PONumber { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
