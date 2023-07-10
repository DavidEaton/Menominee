using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems;

namespace Menominee.Shared.Models.Inventory.MaintenanceItems
{
    public class MaintenanceItemHelper
    {
        public static MaintenanceItemToReadInList ConverToReadInListDto(MaintenanceItem item)
        {
            return item is null
                ? null
                : (new()
                {
                    Id = item.Id,
                    DisplayOrder = item.DisplayOrder,
                    //Item = InventoryItemHelper.ConvertEntityToReadInListDto(item.Item)
                    InventoryItemId = item.InventoryItem.Id,
                    ItemNumber = item.InventoryItem.ItemNumber,
                    Description = item.InventoryItem.Description
                });
        }

        public static MaintenanceItemToRead ConvertToReadDto(MaintenanceItem item)
        {
            return item is null
                ? null
                : (new()
                {
                    Id = item.Id,
                    DisplayOrder = item.DisplayOrder,
                    Item = InventoryItemHelper.ConvertToReadDto(item.InventoryItem)
                });
        }

        public static MaintenanceItemToWrite ConvertReadToWriteDto(MaintenanceItemToRead item)
        {
            return item is null
                ? null
                : (new()
                {
                    Id = item.Id,
                    DisplayOrder = item.DisplayOrder,
                    Item = InventoryItemHelper.ConvertReadToWriteDto(item.Item)
                });
        }

        public static MaintenanceItem ConvertWriteDtoToEntity(
            MaintenanceItemToWrite maintenanceItem,
            InventoryItem inventoryItem)
        {
            return maintenanceItem is null
                ? null
                : MaintenanceItem.Create(
                maintenanceItem.DisplayOrder,
                inventoryItem).Value;
        }
    }
}
