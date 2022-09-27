using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemToReadInList
    {
        public long Id { get; set; }
        public long ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        //public ProductCodeToRead ProductCode { get; set; }
        public long ProductCodeId { get; set; }
        public string ProductCodeName { get; set; }
        public InventoryItemType ItemType { get; set; }
        public string ItemTypeDisplayText
        {
            get
            {
                return EnumExtensions.GetDisplayName(ItemType);
            }
        }

        //public int QuantityOnHand { get; set; }
        //public double Cost { get; set; }
        //public double SuggestedPrice { get; set; }
        //public double Labor { get; set; }
    }
}
