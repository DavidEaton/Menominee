using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemToWrite
    {
        public long Id { get; set; }
        public ManufacturerToWrite Manufacturer { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public ProductCodeToWrite ProductCode { get; set; }
        public InventoryItemType ItemType { get; set; }
        public long DetailId { get; set; }

        public InventoryPartToWrite Part { get; set; }
        public InventoryLaborToWrite Labor { get; set; }
        public InventoryTireToWrite Tire { get; set; }
        public InventoryPackageToWrite Package { get; set; }
    }
}
