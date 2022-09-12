using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class MaintenanceItemHelper
    {
        public static MaintenanceItemToReadInList ConvertEntityToReadInListDto(MaintenanceItem item)
        {
            if (item is null)
                return null;

            return new()
            {
                Id = item.Id,
                DisplayOrder = item.DisplayOrder,
                //Item = InventoryItemHelper.ConvertEntityToReadInListDto(item.Item)
                InventoryItemId = item.Item.Id,
                ItemNumber = item.Item.ItemNumber,
                Description = item.Item.Description
            };
        }

        public static MaintenanceItemToRead ConvertEntityToReadDto(MaintenanceItem item)
        {
            if (item is null)
                return null;

            return new()
            {
                Id = item.Id,
                DisplayOrder = item.DisplayOrder,
                Item = InventoryItemHelper.ConvertEntityToReadDto(item.Item)
            };
        }

        public static MaintenanceItemToWrite ConvertReadToWriteDto(MaintenanceItemToRead item)
        {
            if (item is null)
                return null;

            return new()
            {
                Id = item.Id,
                DisplayOrder = item.DisplayOrder,
                Item = InventoryItemHelper.ConvertReadToWriteDto(item.Item)
            };
        }

        public static MaintenanceItem ConvertWriteDtoToEntity(MaintenanceItemToWrite item)
        {
            if (item is null)
                return null;

            return new()
            {
                DisplayOrder = item.DisplayOrder,
                Item = InventoryItemHelper.ConvertWriteDtoToEntity(item.Item)
            };
        }

        public static void CopyWriteDtoToEntity(MaintenanceItemToWrite itemToUpdate, MaintenanceItem item)
        {
            item.DisplayOrder = itemToUpdate.DisplayOrder;
            item.Item = InventoryItemHelper.ConvertWriteDtoToEntity(itemToUpdate.Item);
        }
    }
}
