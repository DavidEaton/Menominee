﻿
namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemPackageItemToWrite
    {
        public long Id { get; set; }
        public int DisplayOrder { get; set; }
        public InventoryItemToWrite Item { get; set; }
        public InventoryItemPackageDetailsToWrite Details { get; set; }
    }
}
