using Menominee.Common;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemWarranty : Entity
    {
        public InventoryItem InventoryItem { get; private set; }
        public InventoryItemWarrantyPeriodType PeriodType { get; private set; }
        public int Duration { get; private set; }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemWarranty() { }

        #endregion  
    }
}
