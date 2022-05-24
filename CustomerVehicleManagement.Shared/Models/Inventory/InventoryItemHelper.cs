using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemHelper
    {
        public static InventoryItemToReadInList CreateInventoryItemInList(InventoryItem item)
        {
            if (item is null)
                return null;

            return new()
            {
                Id = item.Id,
                //Manufacturer = item.Manufacturer,
                Manufacturer = ManufacturerHelper.CreateManufacturer(item.Manufacturer),
                //ManufacturerId = item.ManufacturerId,
                //ManufacturerName = item.Manufacturer?.Name,
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                //ProductCode = item.ProductCode,
                ProductCode = ProductCodeHelper.CreateProductCode(item.ProductCode),
                //ProductCodeId = item.ProductCodeId,
                //ProductCodeName = item.ProductCode?.Name,
                ItemType = item.ItemType,
                DetailId = item.DetailId
            };
        }

        public static InventoryItemToRead CreateInventoryItem(InventoryItem item)
        {
            if (item is null)
                return null;

            var ItemToRead = new InventoryItemToRead
            {
                Id = item.Id,
                //ManufacturerId = item.ManufacturerId,
                Manufacturer = ManufacturerHelper.CreateManufacturer(item.Manufacturer),
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                //ProductCodeId = item.ProductCodeId,
                ProductCode = ProductCodeHelper.CreateProductCode(item.ProductCode),
                ItemType = item.ItemType,
                DetailId = item.DetailId
            };

            if (item.ItemType == InventoryItemType.Part)
                ItemToRead.Part = InventoryPartHelper.CreateInventoryPart(item.Part);
            else if (item.ItemType == InventoryItemType.Labor)
                ItemToRead.Labor = InventoryLaborHelper.CreateInventoryLabor(item.Labor);
            else if (item.ItemType == InventoryItemType.Tire)
                ItemToRead.Tire = InventoryTireHelper.CreateInventoryTire(item.Tire);
            else if (item.ItemType == InventoryItemType.Package)
                ItemToRead.Package = InventoryPackageHelper.CreateInventoryPackage(item.Package);

            return ItemToRead;
        }

        public static InventoryItemToWrite CreateInventoryItem(InventoryItemToRead item)
        {
            if (item is null)
                return null;

            var Item = new InventoryItemToWrite
            {
                //ManufacturerId = item.ManufacturerId,
                Manufacturer = ManufacturerHelper.CreateManufacturer(item.Manufacturer),
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                //ProductCodeId = item.ProductCodeId,
                ProductCode = ProductCodeHelper.CreateProductCode(item.ProductCode),
                ItemType = item.ItemType,
                DetailId = item.DetailId
            };

            if (item.ItemType == InventoryItemType.Part)
                Item.Part = InventoryPartHelper.CreateInventoryPart(item.Part);
            else if (item.ItemType == InventoryItemType.Labor)
                Item.Labor = InventoryLaborHelper.CreateInventoryLabor(item.Labor);
            else if (item.ItemType == InventoryItemType.Tire)
                Item.Tire = InventoryTireHelper.CreateInventoryTire(item.Tire);
            else if (item.ItemType == InventoryItemType.Package)
                Item.Package = InventoryPackageHelper.CreateInventoryPackage(item.Package);

            return Item;
        }

        public static InventoryItem CreateInventoryItem(InventoryItemToWrite item)
        {
            if (item is null)
                return null;

            var Item = new InventoryItem
            {
                //ManufacturerId = item.ManufacturerId,
                Manufacturer = ManufacturerHelper.CreateManufacturer(item.Manufacturer),
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                //ProductCodeId = item.ProductCodeId,
                ItemType = item.ItemType,
                DetailId = item.DetailId
            };

            if (item.ItemType == InventoryItemType.Part)
                Item.Part = InventoryPartHelper.CreateInventoryPart(item.Part);
            else if (item.ItemType == InventoryItemType.Labor)
                Item.Labor = InventoryLaborHelper.CreateInventoryLabor(item.Labor);
            else if (item.ItemType == InventoryItemType.Tire)
                Item.Tire = InventoryTireHelper.CreateInventoryTire(item.Tire);
            else if (item.ItemType == InventoryItemType.Package)
                Item.Package = InventoryPackageHelper.CreateInventoryPackage(item.Package);

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
                InventoryPartHelper.CopyInventoryPart(itemToUpdate.Part, item.Part);
            else if (item.ItemType == InventoryItemType.Labor)
                InventoryLaborHelper.CopyInventoryLabor(itemToUpdate.Labor, item.Labor);
            else if (item.ItemType == InventoryItemType.Tire)
                InventoryTireHelper.CopyInventoryTire(itemToUpdate.Tire, item.Tire);
            else if (item.ItemType == InventoryItemType.Package)
                InventoryPackageHelper.CopyInventoryPackage(itemToUpdate.Package, item.Package);
        }
    }
}
