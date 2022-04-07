using CustomerVehicleManagement.Shared.Models.Inventory;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Helpers.Inventory
{
    public class InventoryItemHelper
    {
        public static InventoryItemToWrite CreateWriteDtoFromReadDto(InventoryItemToRead item)
        {
            var Item = new InventoryItemToWrite
            {
                ManufacturerId = item.ManufacturerId,
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                ProductCodeId = item.ProductCodeId,
                ItemType = item.ItemType,
                DetailId = item.DetailId
            };

            if (item.ItemType == InventoryItemType.Part)
            {
                Item.Part = InventoryPartHelper.CreateWriteDtoFromReadDto(item.Part);
            }
            else if (item.ItemType == InventoryItemType.Labor)
            {
                Item.Labor = InventoryLaborHelper.CreateWriteDtoFromReadDto(item.Labor);
            }
            else if (item.ItemType == InventoryItemType.Tire)
            {
                Item.Tire = InventoryTireHelper.CreateWriteDtoFromReadDto(item.Tire);
            }

            return Item;
        }
    }
}
