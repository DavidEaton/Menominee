using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemWarranty : Entity
    {
        public InventoryItem InventoryItem { get; private set; }
        public InventoryItemWarrantyPeriod Period { get; private set; }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemWarranty() { }

        #endregion  
    }
}
