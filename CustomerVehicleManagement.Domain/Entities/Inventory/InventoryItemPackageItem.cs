using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPackageItem : Entity
    {
        public long InventoryItemPackageId { get; set; }
        public int Order { get; set; }
        public InventoryItem Item { get; set; }
        public long InventoryItemId { get; set; }
        public double Quantity { get; set; }
        public bool PartAmountIsAdditional { get; set; }
        public bool LaborAmountIsAdditional { get; set; }
        public bool ExciseFeeIsAdditional { get; set; }

        #region ORM

        // EF requires a parameterless constructor
        public InventoryItemPackageItem() { }

        #endregion    
    }
}
