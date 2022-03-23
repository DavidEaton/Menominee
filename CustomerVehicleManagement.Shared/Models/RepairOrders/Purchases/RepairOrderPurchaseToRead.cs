using System;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Purchases
{
    public class RepairOrderPurchaseToRead
    {
        public long Id { get; set; }
        public long RepairOrderItemId { get; set; }
        public long VendorId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string PONumber { get; set; }
        public string VendorInvoiceNumber { get; set; }
        public string VendorPartNumber { get; set; }
    }
}