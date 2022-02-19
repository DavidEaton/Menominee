using Menominee.Common;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class RepairOrderWarranty : Entity
    {
        public long RepairOrderItemId { get; set; }
        public int SequenceNumber { get; set; }
        public double Quantity { get; set; }
        public WarrantyType Type { get; set; }
        public string NewWarranty { get; set; }
        public string OriginalWarranty { get; set; }
        public long OriginalInvoiceId { get; set; }

        #region ORM

        // EF requires an empty constructor
        public RepairOrderWarranty() { }

        #endregion
    }
}
