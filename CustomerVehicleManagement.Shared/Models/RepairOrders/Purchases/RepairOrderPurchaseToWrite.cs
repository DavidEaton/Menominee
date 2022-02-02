using System;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Purchases
{
    public class RepairOrderPurchaseToWrite
    {
        public long Id { get; set; } = 0;
        public long RepairOrderItemId { get; set; } = 0;
        public long VendorId { get; set; } = 0;
        public DateTime? PurchaseDate { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public string VendorInvoiceNumber { get; set; } = string.Empty;
        public string VendorPartNumber { get; set; } = string.Empty;
    }
}
