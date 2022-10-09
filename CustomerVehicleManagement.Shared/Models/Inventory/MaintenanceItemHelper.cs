using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class MaintenanceItemHelper
    {
        public static MaintenanceItemToReadInList ConvertEntityToReadInListDto(MaintenanceItem item)
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

        public static MaintenanceItemToRead ConvertEntityToReadDto(MaintenanceItem item)
        {
            return item is null
                ? null
                : (new()
                {
                    Id = item.Id,
                    DisplayOrder = item.DisplayOrder,
                    Item = InventoryItemHelper.ConvertEntityToReadDto(item.InventoryItem)
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
