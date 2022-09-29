using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPackageItem : Entity
    {
        public InventoryItemPackage InventoryItemPackage { get; private set; }
        public int Order { get; private set; }
        public InventoryItem Item { get; private set; }
        public InventoryItemPackageDetails InventoryItemPackageDetails { get; private set; }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackageItem() { }

        #endregion    
    }
}
