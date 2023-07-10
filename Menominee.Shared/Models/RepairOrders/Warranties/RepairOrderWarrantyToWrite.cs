using Menominee.Common.Enums;

namespace Menominee.Shared.Models.RepairOrders.Warranties
{
    public class RepairOrderWarrantyToWrite
    {
        public long Id { get; set; }
        public double Quantity { get; set; } = 0.0;
        public WarrantyType Type { get; set; } = WarrantyType.NewWarranty;
        public string NewWarranty { get; set; } = string.Empty;
        public string OriginalWarranty { get; set; } = string.Empty;
        public long OriginalInvoiceId { get; set; } = 0;
    }
}
