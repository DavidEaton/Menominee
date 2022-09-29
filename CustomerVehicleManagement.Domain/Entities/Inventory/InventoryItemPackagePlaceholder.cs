using Menominee.Common;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPackagePlaceholder : Entity
    {
        public InventoryItemPackage InventoryItemPackage { get; private set; }
        public PackagePlaceholderItemType ItemType { get; private set; }
        public string Description { get; private set; }
        public int Order { get; private set; }
        public InventoryItemPackageDetails InventoryItemPackageDetails { get; private set; }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackagePlaceholder() { }

        #endregion    
    }
}
