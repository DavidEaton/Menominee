using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemToReadInList
    {
        public long Id { get; set; }
        public ManufacturerToRead Manufacturer { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public ProductCodeToRead ProductCode { get; set; }
        public string ItemType { get; set; }
        public InventoryItemPartToRead Part { get; set; }
        public InventoryItemLaborToRead Labor { get; set; }
        public InventoryItemTireToRead Tire { get; set; }
        public InventoryItemPackageToRead Package { get; set; }
        public InventoryItemInspectionToRead Inspection { get; set; }
        public InventoryItemWarrantyToRead Warranty { get; set; }
    }
}
