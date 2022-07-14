using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemHelper
    {
        public static InventoryItemToReadInList ConvertEntityToReadInListDto(InventoryItem item)
        {
            if (item is null)
                return null;

            return new()
            {
                Id = item.Id,
                //Manufacturer = item.Manufacturer,
                //ManufacturerName = ManufacturerHelper.ConvertToRead(item.Manufacturer).Name,
                ManufacturerId = item.ManufacturerId,
                ManufacturerName = item.Manufacturer?.Name,
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                //ProductCode = item.ProductCode,
                //ProductCodeName = ProductCodeHelper.ConvertEntityToReadDto(item.ProductCode).Name,
                ProductCodeId = item.ProductCodeId,
                ProductCodeName = item.ProductCode?.Name,
                ItemType = item.ItemType
            };
        }

        public static InventoryItemToRead ConvertEntityToReadDto(InventoryItem item)
        {
            if (item is null)
                return null;

            var ItemToRead = new InventoryItemToRead
            {
                Id = item.Id,
                //ManufacturerId = item.ManufacturerId,
                Manufacturer = ManufacturerHelper.ConvertEntityToReadDto(item.Manufacturer),
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                //ProductCodeId = item.ProductCodeId,
                ProductCode = ProductCodeHelper.ConvertEntityToReadDto(item.ProductCode),
                ItemType = item.ItemType
            };

            if (item.ItemType == InventoryItemType.Part)
                ItemToRead.Part = InventoryPartHelper.ConvertEntityToReadDto(item.Part);
            else if (item.ItemType == InventoryItemType.Labor)
                ItemToRead.Labor = InventoryLaborHelper.ConvertEntityToReadDto(item.Labor);
            else if (item.ItemType == InventoryItemType.Tire)
                ItemToRead.Tire = InventoryTireHelper.ConvertEntityToReadDto(item.Tire);
            else if (item.ItemType == InventoryItemType.Package)
                ItemToRead.Package = InventoryPackageHelper.ConvertEntityToReadDto(item.Package);
            else if (item.ItemType == InventoryItemType.Inspection)
                ItemToRead.Inspection = InventoryInspectionHelper.ConvertEntityToReadDto(item.Inspection);
            // Coming soon...
            //else if (item.ItemType == InventoryItemType.Donation)
            //    ItemToRead.Donation = InventoryDonationHelper.ConvertEntityToReadDto(item.Donation);
            //else if (item.ItemType == InventoryItemType.GiftCertificate)
            //    ItemToRead.GiftCertificate = InventoryGiftCertificateHelper.ConvertEntityToReadDto(item.GiftCertificate);
            else if (item.ItemType == InventoryItemType.Warranty)
                ItemToRead.Warranty = InventoryWarrantyHelper.ConvertEntityToReadDto(item.Warranty);

            return ItemToRead;
        }

        public static InventoryItemToWrite ConvertReadToWriteDto(InventoryItemToRead item)
        {
            if (item is null)
                return null;

            var Item = new InventoryItemToWrite
            {
                //ManufacturerId = item.Manufacturer.Id,
                Id = item.Id,
                Manufacturer = ManufacturerHelper.ConvertReadToWriteDto(item.Manufacturer),
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                //ProductCodeId = item.ProductCodeId,
                ProductCode = ProductCodeHelper.ConvertReadToWriteDto(item.ProductCode),
                ItemType = item.ItemType
            };

            if (item.ItemType == InventoryItemType.Part)
                Item.Part = InventoryPartHelper.ConvertReadToWriteDto(item.Part);
            else if (item.ItemType == InventoryItemType.Labor)
                Item.Labor = InventoryLaborHelper.ConvertReadToWriteDto(item.Labor);
            else if (item.ItemType == InventoryItemType.Tire)
                Item.Tire = InventoryTireHelper.ConvertReadToWriteDto(item.Tire);
            else if (item.ItemType == InventoryItemType.Package)
                Item.Package = InventoryPackageHelper.ConvertReadToWriteDto(item.Package);
            else if (item.ItemType == InventoryItemType.Inspection)
                Item.Inspection = InventoryInspectionHelper.ConvertReadToWriteDto(item.Inspection);
            // Coming soon...
            //else if (item.ItemType == InventoryItemType.Donation)
            //    Item.Donation = InventoryDonationHelper.ConvertReadToWriteDto(item.Donation);
            //else if (item.ItemType == InventoryItemType.GiftCertificate)
            //    Item.GiftCertificate = InventoryGiftCertificateHelper.ConvertReadToWriteDto(item.GiftCertificate);
            else if (item.ItemType == InventoryItemType.Warranty)
                Item.Warranty = InventoryWarrantyHelper.ConvertReadToWriteDto(item.Warranty);

            return Item;
        }

        public static InventoryItem ConvertWriteDtoToEntity(InventoryItemToWrite item)
        {
            if (item is null)
                return null;

            var Item = new InventoryItem
            {
                ManufacturerId = item.Manufacturer.Id,
                //Manufacturer = ManufacturerHelper.ConvertWriteDtoToEntity(item.Manufacturer),
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                ProductCodeId = item.ProductCode.Id,
                //ProductCode = ProductCodeHelper.ConvertWriteDtoToEntity(item.ProductCode),
                ItemType = item.ItemType
            };

            if (item.ItemType == InventoryItemType.Part)
                Item.Part = InventoryPartHelper.ConvertWriteDtoToEntity(item.Part);
            else if (item.ItemType == InventoryItemType.Labor)
                Item.Labor = InventoryLaborHelper.ConvertWriteDtoToEntity(item.Labor);
            else if (item.ItemType == InventoryItemType.Tire)
                Item.Tire = InventoryTireHelper.ConvertWriteDtoToEntity(item.Tire);
            else if (item.ItemType == InventoryItemType.Package)
                Item.Package = InventoryPackageHelper.ConvertWriteDtoToEntity(item.Package);
            else if (item.ItemType == InventoryItemType.Inspection)
                Item.Inspection = InventoryInspectionHelper.ConvertWriteDtoToEntity(item.Inspection);
            // Coming soon...
            //else if (item.ItemType == InventoryItemType.Donation)
            //    Item.Donation = InventoryDonationHelper.ConvertWriteDtoToEntity(item.Donation);
            //else if (item.ItemType == InventoryItemType.GiftCertificate)
            //    Item.GiftCertificate = InventoryGiftCertificateHelper.ConvertWriteDtoToEntity(item.GiftCertificate);
            else if (item.ItemType == InventoryItemType.Warranty)
                Item.Warranty = InventoryWarrantyHelper.ConvertWriteDtoToEntity(item.Warranty);

            return Item;
        }

        public static void CopyWriteDtoToEntity(InventoryItemToWrite itemToUpdate, InventoryItem item)
        {
            item.ManufacturerId = itemToUpdate.Manufacturer.Id;
            //item.Manufacturer = ManufacturerHelper.ConvertWriteDtoToEntity(itemToUpdate.Manufacturer);
            item.ItemNumber = itemToUpdate.ItemNumber;
            item.Description = itemToUpdate.Description;
            item.ProductCodeId = itemToUpdate.ProductCode.Id;
            //item.ProductCode = ProductCodeHelper.ConvertWriteDtoToEntity(itemToUpdate.ProductCode);
            item.ItemType = itemToUpdate.ItemType;

            if (item.ItemType == InventoryItemType.Part)
                InventoryPartHelper.CopyWriteDtoToEntity(itemToUpdate.Part, item.Part);
            else if (item.ItemType == InventoryItemType.Labor)
                InventoryLaborHelper.CopyWriteDtoToEntity(itemToUpdate.Labor, item.Labor);
            else if (item.ItemType == InventoryItemType.Tire)
                InventoryTireHelper.CopyWriteDtoToEntity(itemToUpdate.Tire, item.Tire);
            else if (item.ItemType == InventoryItemType.Package)
                InventoryPackageHelper.CopyWriteDtoToEntity(itemToUpdate.Package, item.Package);
            else if (item.ItemType == InventoryItemType.Inspection)
                InventoryInspectionHelper.CopyWriteDtoToEntity(itemToUpdate.Inspection, item.Inspection);
            // Coming soon...
            //else if (item.ItemType == InventoryItemType.Donation)
            //    InventoryDonationHelper.CopyWriteDtoToEntity(itemToUpdate.Donation, item.Donation);
            //else if (item.ItemType == InventoryItemType.GiftCertificate)
            //    InventoryGiftCertificateHelper.CopyWriteDtoToEntity(itemToUpdate.GiftCertificate, item.GiftCertificate);
            else if (item.ItemType == InventoryItemType.Warranty)
                InventoryWarrantyHelper.CopyWriteDtoToEntity(itemToUpdate.Warranty, item.Warranty);
        }
    }
}
