using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPackageItem : Entity
    {
        public InventoryItemPackage InventoryItemPackage { get; private set; }
        public int Order { get; private set; }
        public InventoryItem Item { get; private set; }
        public double Quantity { get; private set; }
        public bool PartAmountIsAdditional { get; private set; }
        public bool LaborAmountIsAdditional { get; private set; }
        public bool ExciseFeeIsAdditional { get; private set; }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackageItem() { }

        #endregion    
    }
}
