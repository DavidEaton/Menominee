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

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems
{
    public class InventoryItemHelper
    {
        public static InventoryItemToReadInList ConvertToReadInListDto(InventoryItem item)
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
                    ItemType = item.ItemType,
                });
        }

        public static InventoryItemToRead ConvertToReadDto(InventoryItem item)
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
                    inventoryItemToRead.Part = InventoryItemPartHelper.ConvertToReadDto(item.Part);
                    break;
                case InventoryItemType.Labor:
                    inventoryItemToRead.Labor = InventoryItemLaborHelper.ConvertToReadDto(item.Labor);
                    break;
                case InventoryItemType.Tire:
                    inventoryItemToRead.Tire = InventoryItemTireHelper.ConvertToReadDto(item.Tire);
                    break;
                case InventoryItemType.Package:
                    inventoryItemToRead.Package = InventoryItemPackageHelper.ConvertToReadDto(item.Package);
                    break;
                case InventoryItemType.Inspection:
                    inventoryItemToRead.Inspection = InventoryItemInspectionHelper.ConvertToReadDto(item.Inspection);
                    break;
                case InventoryItemType.GiftCertificate:
                    inventoryItemToRead.Warranty = InventoryItemWarrantyHelper.ConvertToReadDto(item.Warranty);
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
                ItemType = item.ItemType
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
                case InventoryItemType.Warranty:
                    inventoryItemToWrite.Warranty = InventoryItemWarrantyHelper.ConvertReadToWriteDto(item.Warranty);
                    break;
                case InventoryItemType.GiftCertificate:
                    break;
                case InventoryItemType.Donation:
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
                item.ItemType,

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
                    : InventoryItemWarrantyHelper.ConvertWriteDtoToEntity(item.Warranty))
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
                    item.ItemType,
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
                        : InventoryItemWarrantyHelper.ConvertWriteDtoToEntity(item.Warranty))
                .Value;
        }

        public static InventoryItemInspectionToWrite ConvertToWriteDto(InventoryItemInspection inspection)
        {
            return inspection is null
                ? null
                : new()
                {
                    Id = inspection.Id,
                    LaborAmount = new() { Amount = inspection.LaborAmount.Amount, PayType = inspection.LaborAmount.PayType },
                    TechAmount = new() { Amount = inspection.TechAmount.Amount, PayType = inspection.TechAmount.PayType, SkillLevel = inspection.TechAmount.SkillLevel },
                    Type = inspection.InspectionType
                };
        }

        public static InventoryItemLaborToWrite ConvertToWriteDto(InventoryItemLabor labor)
        {
            return labor is null
                ? null
                : new()
                {
                    Id = labor.Id,
                    LaborAmount = new() { Amount = labor.LaborAmount.Amount, PayType = labor.LaborAmount.PayType },
                    TechAmount = new() { Amount = labor.TechAmount.Amount, PayType = labor.TechAmount.PayType, SkillLevel = labor.TechAmount.SkillLevel }
                };
        }

        internal static InventoryItemToWrite ConvertToWriteDto(InventoryItem item)
        {
            return item is null
                ? null
                : new()
                {
                    Id = item.Id,
                    ItemNumber = item.ItemNumber,
                    Description = item.Description,
                    ItemType = item.ItemType,
                    Manufacturer = new ManufacturerToRead
                    {
                        Id = item.ProductCode.Manufacturer.Id,
                        Code = item.ProductCode.Manufacturer.Code,
                        Name = item.ProductCode.Manufacturer.Name,
                        Prefix = item.ProductCode.Manufacturer.Prefix
                    },
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
                    Part = InventoryItemPartHelper.ConvertToWriteDto(item.Part),
                    Labor = InventoryItemLaborHelper.ConvertToWriteDto(item.Labor),
                    Tire = InventoryItemTireHelper.ConvertToWriteDto(item.Tire),
                    Package = InventoryItemPackageHelper.ConvertToWriteDto(item.Package),
                    Inspection = InventoryItemInspectionHelper.ConvertToWriteDto(item.Inspection),
                    Warranty = InventoryItemWarrantyHelper.ConvertToWriteDto(item.Warranty)
                };
        }
    }
}