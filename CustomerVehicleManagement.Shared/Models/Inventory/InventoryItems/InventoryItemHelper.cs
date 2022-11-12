using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Inspection;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Part;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Tire;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Warranty;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using static CustomerVehicleManagement.Shared.Utilities;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems
{
    public class InventoryItemHelper
    {
        public static InventoryItemToReadInList ConvertEntityToReadInListDto(InventoryItem item)
        {
            return item is null
                ? new InventoryItemToReadInList()
                : (new()
                {
                    Id = item.Id,
                    Manufacturer = new ManufacturerToRead()
                    {
                        Code = item.Manufacturer.Code,
                        Id = item.Manufacturer.Id,
                        Name = item.Manufacturer.Name,
                        Prefix = item.Manufacturer.Prefix
                    },
                    ItemNumber = item.ItemNumber,
                    Description = item.Description,
                    ProductCode = new ProductCodeToRead()
                    {
                        Name = item.ProductCode.Name,
                        Code = item.ProductCode.Code,
                        Id = item.ProductCode.Id,
                        Manufacturer = new ManufacturerToRead()
                        {
                            Id = item.ProductCode.Manufacturer.Id,
                            Code = item.ProductCode.Manufacturer.Code,
                            Name = item.ProductCode.Manufacturer.Name,
                            Prefix = item.ProductCode.Manufacturer.Prefix
                        },
                    },
                    ItemType = item.ItemType.ToString(),
                });
        }

        public static InventoryItemToRead ConvertEntityToReadDto(InventoryItem item)
        {
            if (item is null)
                return null;

            var inventoryItemToRead = new InventoryItemToRead
            {
                Id = item.Id,
                Manufacturer = new ManufacturerToRead
                {
                    Code = item.Manufacturer.Code,
                    Id = item.Manufacturer.Id,
                    Name = item.Manufacturer.Name,
                    Prefix = item.Manufacturer.Prefix
                },
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                ProductCode = new ProductCodeToRead
                {
                    Name = item.ProductCode.Name,
                    Code = item.ProductCode.Code,
                    Id = item.ProductCode.Id,
                    Manufacturer = new ManufacturerToRead
                    {
                        Id = item.ProductCode.Manufacturer.Id,
                        Code = item.ProductCode.Manufacturer.Code,
                        Name = item.ProductCode.Manufacturer.Name,
                        Prefix = item.ProductCode.Manufacturer.Prefix
                    },
                },
                ItemType = item.ItemType
            };

            switch (item.ItemType)
            {
                case InventoryItemType.Part:
                    inventoryItemToRead.Part = InventoryItemPartHelper.ConvertEntityToReadDto(item.Part);
                    break;
                case InventoryItemType.Labor:
                    inventoryItemToRead.Labor = InventoryItemLaborHelper.ConvertEntityToReadDto(item.Labor);
                    break;
                case InventoryItemType.Tire:
                    inventoryItemToRead.Tire = InventoryItemTireHelper.ConvertEntityToReadDto(item.Tire);
                    break;
                case InventoryItemType.Package:
                    inventoryItemToRead.Package = InventoryItemPackageHelper.ConvertEntityToReadDto(item.Package);
                    break;
                case InventoryItemType.Inspection:
                    inventoryItemToRead.Inspection = InventoryItemInspectionHelper.ConvertEntityToReadDto(item.Inspection);
                    break;
                case InventoryItemType.GiftCertificate:
                    inventoryItemToRead.Warranty = InventoryWarrantyHelper.ConvertEntityToReadDto(item.Warranty);
                    break;
                case InventoryItemType.Donation:
                    break;
                case InventoryItemType.Warranty:
                    break;
                default:
                    throw new ArgumentException("Invalid Inventory Item Type.");
            };

            return inventoryItemToRead;
        }

        public static InventoryItemToWrite ConvertReadToWriteDto(InventoryItemToRead item)
        {
            if (item is null)
                return null;

            var inventoryItemToWrite = new InventoryItemToWrite
            {
                Id = item.Id,
                Manufacturer = item.Manufacturer,
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                ProductCode = item.ProductCode,
                ItemType = item.ItemType.ToString()
            };

            switch (item.ItemType)
            {
                case InventoryItemType.Part:
                    inventoryItemToWrite.Part = InventoryItemPartHelper.ConvertReadToWriteDto(item.Part);
                    break;
                case InventoryItemType.Labor:
                    inventoryItemToWrite.Labor = InventoryItemLaborHelper.ConvertReadToWriteDto(item.Labor);
                    break;
                case InventoryItemType.Tire:
                    inventoryItemToWrite.Tire = InventoryItemTireHelper.ConvertReadToWriteDto(item.Tire);
                    break;
                case InventoryItemType.Package:
                    inventoryItemToWrite.Package = InventoryItemPackageHelper.ConvertReadToWriteDto(item.Package);
                    break;
                case InventoryItemType.Inspection:
                    inventoryItemToWrite.Inspection = InventoryItemInspectionHelper.ConvertReadToWriteDto(item.Inspection);
                    break;
                case InventoryItemType.GiftCertificate:
                    inventoryItemToWrite.Warranty = InventoryWarrantyHelper.ConvertReadToWriteDto(item.Warranty);
                    break;
                case InventoryItemType.Donation:
                    break;
                case InventoryItemType.Warranty:
                    break;
                default:
                    throw new ArgumentException("Invalid Inventory Item Type.");
            };

            return inventoryItemToWrite;
        }

        public static InventoryItem ConvertWriteDtoToEntity(InventoryItemToWrite item, IReadOnlyList<Manufacturer> manufacturers, IReadOnlyList<ProductCode> productCodes, IReadOnlyList<InventoryItem> inventoryItems)
        {
            if (item is null)
                return null;

            var inventoryItem = InventoryItem.Create(
                manufacturers.FirstOrDefault(manufacturer => manufacturer.Id == item.Manufacturer.Id),
                item.ItemNumber,
                item.Description,
                productCodes.FirstOrDefault(productCode => productCode.Id == item.ProductCode.Id),
                ParseEnum<InventoryItemType>(item.ItemType),

                item.Part is null
                    ? null
                    : InventoryItemPartHelper.ConvertWriteDtoToEntity(item.Part),
                item.Labor is null
                    ? null
                    : InventoryItemLaborHelper.ConvertWriteDtoToEntity(item.Labor),
                item.Tire is null
                    ? null
                    : InventoryItemTireHelper.ConvertWriteDtoToEntity(item.Tire),
                item.Package is null
                    ? null
                    : InventoryItemPackageHelper.ConvertWriteDtoToEntity(item.Package, inventoryItems),
                item.Inspection is null
                    ? null
                    : InventoryItemInspectionHelper.ConvertWriteDtoToEntity(item.Inspection),
                item.Warranty is null
                    ? null
                    : InventoryWarrantyHelper.ConvertWriteDtoToEntity(item.Warranty))
                .Value;

            return inventoryItem;
        }

        public static InventoryItem ConvertWriteDtoToEntity(InventoryItemToWrite item, Manufacturer manufacturer, ProductCode productCode, IReadOnlyList<InventoryItem> inventoryItems)
        {
            return item is null
                ? null
                : InventoryItem.Create(
                manufacturer,
                item.ItemNumber,
                item.Description,
                productCode,
                Utilities.ParseEnum<InventoryItemType>(item.ItemType),
                item.Part is null
                    ? null
                    : InventoryItemPartHelper.ConvertWriteDtoToEntity(item.Part),
                item.Labor is null
                    ? null
                    : InventoryItemLaborHelper.ConvertWriteDtoToEntity(item.Labor),
                item.Tire is null
                    ? null
                    : InventoryItemTireHelper.ConvertWriteDtoToEntity(item.Tire),
                item.Package is null
                    ? null
                    : InventoryItemPackageHelper.ConvertWriteDtoToEntity(item.Package, inventoryItems),
                item.Inspection is null
                    ? null
                    : InventoryItemInspectionHelper.ConvertWriteDtoToEntity(item.Inspection),
                item.Warranty is null
                    ? null
                    : InventoryWarrantyHelper.ConvertWriteDtoToEntity(item.Warranty))
                .Value;
        }
    }
}