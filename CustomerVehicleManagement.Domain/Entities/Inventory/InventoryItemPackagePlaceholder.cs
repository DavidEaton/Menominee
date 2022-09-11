using Menominee.Common;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPackagePlaceholder : Entity
    {
        public long InventoryItemPackageId { get; set; }
        public int Order { get; set; }
        public PackagePlaceholderItemType ItemType { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public bool PartAmountIsAdditional { get; set; }
        public bool LaborAmountIsAdditional { get; set; }
        public bool ExciseFeeIsAdditional { get; set; }

        #region ORM

        // EF requires a parameterless constructor
        public InventoryItemPackagePlaceholder() { }

        #endregion    
    }
}
