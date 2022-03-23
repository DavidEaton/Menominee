using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties
{
    public class RepairOrderWarrantyToWrite
    {
        public long RepairOrderItemId { get; set; } = 0;
        public double Quantity { get; set; } = 0.0;
        public WarrantyType Type { get; set; } = WarrantyType.NewWarranty;
        public string NewWarranty { get; set; } = string.Empty;
        public string OriginalWarranty { get; set; } = string.Empty;
        public long OriginalInvoiceId { get; set; } = 0;
    }
}
