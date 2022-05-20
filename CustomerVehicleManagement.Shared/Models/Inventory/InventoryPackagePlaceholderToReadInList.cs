using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryPackagePlaceholderToReadInList
    {
        public long Id { get; set; }
        public int Order { get; set; }
        public PackagePlaceholderItemType ItemType { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public bool PartAmountIsAdditional { get; set; }
        public bool LaborAmountIsAdditional { get; set; }
        public bool ExciseFeeIsAdditional { get; set; }
    }
}
