using Menominee.Common;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemWarranty : Entity
    {
        public long InventoryItemId { get; set; }
        public InventoryItemWarrantyPeriodType PeriodType { get; set; }
        public int Duration { get; set; }

        #region ORM

        // EF requires an empty constructor
        public InventoryItemWarranty() { }

        #endregion  
    }
}
