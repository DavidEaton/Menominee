using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class MaintenanceItem : Entity
    {
        public long DisplayOrder { get; private set; }
        public InventoryItem Item { get; private set; }
        
        
        #region ORM

        // EF requires a parameterless constructor
        protected MaintenanceItem() { }

        #endregion
    }
}
