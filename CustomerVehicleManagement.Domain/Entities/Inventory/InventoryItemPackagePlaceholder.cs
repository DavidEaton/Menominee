using Menominee.Common;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPackagePlaceholder : Entity
    {
        public long InventoryItemPackage { get; private set; }
        public int Order { get; private set; }
        public PackagePlaceholderItemType ItemType { get; private set; }
        public string Description { get; private set; }
        public double Quantity { get; private set; }
        public bool PartAmountIsAdditional { get; private set; }
        public bool LaborAmountIsAdditional { get; private set; }
        public bool ExciseFeeIsAdditional { get; private set; }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackagePlaceholder() { }

        #endregion    
    }
}
