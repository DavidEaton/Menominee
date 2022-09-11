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

        // EF requires a parameterless constructor
        public InventoryItemWarranty() { }

        #endregion  
    }
}
