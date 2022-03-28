using System;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Purchases
{
    public class PurchaseListItem
    {
        public long ItemId { get; set; }
        public long VendorId { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string VendorName { get; set; }
        public string VendorPartNumber { get; set; }
        public string VendorInvoiceNumber { get; set; }
        public string PONumber { get; set; }
        public double Quantity { get; set; }
        public double FileCost { get; set; }
        public double VendorCost { get; set; }
        public double VendorCore { get; set; }
        public DateTime? DatePurchased { get; set; }

        public bool IsComplete()
        {
            return !string.IsNullOrWhiteSpace(VendorName) && !string.IsNullOrWhiteSpace(VendorInvoiceNumber) && VendorCost > 0.0;
        }
    }
}
