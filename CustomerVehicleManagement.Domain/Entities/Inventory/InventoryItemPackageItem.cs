using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPackageItem : Entity
    {
        public int Order { get; set; }
        public virtual InventoryItem Item { get; set; }
        public double Quantity { get; set; }
        public bool PartAmountIsAdditional { get; set; }
        public bool LaborAmountIsAdditional { get; set; }
        public bool ExciseFeeIsAdditional { get; set; }

        #region ORM

        // EF requires an empty constructor
        public InventoryItemPackageItem() { }

        #endregion    
    }
}
