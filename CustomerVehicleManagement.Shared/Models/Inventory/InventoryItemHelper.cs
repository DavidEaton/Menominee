using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemHelper
    {
        public static InventoryItemToReadInList CreateInventoryItemInList(InventoryItem item)
        {
            if (item is not null)
            {
                return new InventoryItemToReadInList
                {
                    Id = item.Id,
                    Manufacturer = item.Manufacturer,
                    ManufacturerId = item.ManufacturerId,
                    ManufacturerName = item.Manufacturer?.Name,
                    ItemNumber = item.ItemNumber,
                    Description = item.Description,
                    ProductCode = item.ProductCode,
                    ProductCodeId = item.ProductCodeId,
                    ProductCodeName = item.ProductCode?.Name,
                    ItemType = item.ItemType,
                    DetailId = item.DetailId
                };
            }

            return null;
        }

        public static InventoryItemToRead CreateInventoryItem(InventoryItem item)
        {
            var ItemToRead = new InventoryItemToRead
            {
                Id = item.Id,
                ManufacturerId = item.ManufacturerId,
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                ProductCodeId = item.ProductCodeId,
                ItemType = item.ItemType,
                DetailId = item.DetailId
            };

            if (item.ItemType == InventoryItemType.Part)
                ItemToRead.Part = InventoryPartHelper.Transform(item.Part);
            else if (item.ItemType == InventoryItemType.Labor)
                ItemToRead.Labor = InventoryLaborHelper.Transform(item.Labor);
            else if (item.ItemType == InventoryItemType.Tire)
                ItemToRead.Tire = InventoryTireHelper.Transform(item.Tire);

            return ItemToRead;
        }

        public static InventoryItemToWrite CreateInventoryItem(InventoryItemToRead item)
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
                Item.Part = InventoryPartHelper.Transform(item.Part);
            else if (item.ItemType == InventoryItemType.Labor)
                Item.Labor = InventoryLaborHelper.Transform(item.Labor);
            else if (item.ItemType == InventoryItemType.Tire)
                Item.Tire = InventoryTireHelper.Transform(item.Tire);

            return Item;
        }

        public static void CopyInventoryItem(InventoryItemToWrite itemToUpdate, InventoryItem item)
        {
            item.ManufacturerId = itemToUpdate.ManufacturerId;
            item.ItemNumber = itemToUpdate.ItemNumber;
            item.Description = itemToUpdate.Description;
            item.ProductCodeId = itemToUpdate.ProductCodeId;
            item.ItemType = itemToUpdate.ItemType;
            item.DetailId = itemToUpdate.DetailId;

            if (item.ItemType == InventoryItemType.Part)
                InventoryPartHelper.CopyWriteDtoToEntity(itemToUpdate.Part, item.Part);
            else if (item.ItemType == InventoryItemType.Labor)
                InventoryLaborHelper.CopyWriteDtoToEntity(itemToUpdate.Labor, item.Labor);
            else if (item.ItemType == InventoryItemType.Tire)
                InventoryTireHelper.CopyWriteDtoToEntity(itemToUpdate.Tire, item.Tire);
        }
    }
}
