using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Inspection;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Part;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Tire;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Warranty;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemToRead
    {
        public long Id { get; set; }
        public ManufacturerToRead Manufacturer { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public ProductCodeToRead ProductCode { get; set; }
        public InventoryItemType ItemType { get; set; }
        public InventoryItemPartToRead Part { get; set; }
        public InventoryItemLaborToRead Labor { get; set; }
        public InventoryItemTireToRead Tire { get; set; }
        public InventoryItemPackageToRead Package { get; set; }
        public InventoryItemInspectionToRead Inspection { get; set; }
        public InventoryItemWarrantyToRead Warranty { get; set; }
    }
}
