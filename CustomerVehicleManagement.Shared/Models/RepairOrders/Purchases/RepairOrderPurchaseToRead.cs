using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using System;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Purchases
{
    public class RepairOrderPurchaseToRead
    {
        public long Id { get; set; }
        public VendorToRead Vendor { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string PONumber { get; set; }
        public string VendorInvoiceNumber { get; set; }
        public string VendorPartNumber { get; set; }
    }
}