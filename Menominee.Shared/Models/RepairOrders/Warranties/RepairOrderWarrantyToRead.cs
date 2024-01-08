using Menominee.Domain.Enums;

namespace Menominee.Shared.Models.RepairOrders.Warranties
{
    public class RepairOrderWarrantyToRead
    {
        public long Id { get; set; }
        public double Quantity { get; set; }
        public WarrantyType Type { get; set; }
        public string NewWarranty { get; set; }
        public string OriginalWarranty { get; set; }
        public long OriginalInvoiceId { get; set; }
    }
}
