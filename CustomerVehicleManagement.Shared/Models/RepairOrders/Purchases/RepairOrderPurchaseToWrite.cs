using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using System;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Purchases
{
    public class RepairOrderPurchaseToWrite
    {
        public long Id { get; set; } = 0;
        public VendorToRead Vendor { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public string VendorInvoiceNumber { get; set; } = string.Empty;
        public string VendorPartNumber { get; set; } = string.Empty;
    }
}
