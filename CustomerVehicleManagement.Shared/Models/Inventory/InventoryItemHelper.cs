using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Helpers.Inventory;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemHelper
    {
        public static InventoryItemToReadInList ConvertToReadInListDto(InventoryItem item)
        {
            if (item is not null)
            {
                return new InventoryItemToReadInList
                {
                    Id = item.Id,
                    Manufacturer = item.Manufacturer,
                    ManufacturerId = item.ManufacturerId,
                    //PartNumber = item.PartNumber,
                    Description = item.Description,
                    ProductCode = item.ProductCode,
                    ProductCodeId = item.ProductCodeId,
                    //PartType = item.PartType,
                    //QuantityOnHand = item.QuantityOnHand,
                    //Cost = item.Cost,
                    //SuggestedPrice = item.SuggestedPrice,
                    //Labor = item.Labor
                };
            }

            return null;
        }

        public static InventoryItemToRead ConvertToDto(InventoryItem item)
        {
            if (item != null)
            {
                return new InventoryItemToRead
                {
                    Id = item.Id,
                    Manufacturer = item.Manufacturer,
                    ManufacturerId = item.ManufacturerId,
                    //PartNumber = item.PartNumber,
                    Description = item.Description,
                    ProductCode = item.ProductCode,
                    ProductCodeId = item.ProductCodeId,
                    //PartType = item.PartType,
                    //QuantityOnHand = item.QuantityOnHand,
                    //Cost = item.Cost,
                    //SuggestedPrice = item.SuggestedPrice,
                    //Labor = item.Labor
                };
            }

            return null;
        }
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

        public static void CopyWriteDtoToEntity(InventoryItemToWrite itemToUpdate, InventoryItem item)
        {
            item.ManufacturerId = itemToUpdate.ManufacturerId;
            item.ItemNumber = itemToUpdate.ItemNumber;
            item.Description = itemToUpdate.Description;
            item.ProductCodeId = itemToUpdate.ProductCodeId;
            item.ItemType = itemToUpdate.ItemType;
            item.DetailId = itemToUpdate.DetailId;

            if (item.ItemType == InventoryItemType.Part)
            {
                InventoryPartHelper.CopyWriteDtoToEntity(itemToUpdate.Part, item.Part);
            }
            else if (item.ItemType == InventoryItemType.Labor)
            {
                InventoryLaborHelper.CopyWriteDtoToEntity(itemToUpdate.Labor, item.Labor);
            }
            else if (item.ItemType == InventoryItemType.Tire)
            {
                InventoryTireHelper.CopyWriteDtoToEntity(itemToUpdate.Tire, item.Tire);
            }
        }

    }
}
