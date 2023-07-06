using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackageHelper
    {
        public static InventoryItemPackageToRead ConvertToReadDto(InventoryItemPackage package)
        {
            return package is null
                ? null
                : (new()
                {
                    Id = package.Id,
                    BasePartsAmount = package.BasePartsAmount,
                    BaseLaborAmount = package.BaseLaborAmount,
                    Script = package.Script,
                    IsDiscountable = package.IsDiscountable,
                    Items = ConvertToReadDtos(package.Items),
                    Placeholders = ConvertEntiiesToReadDtos(package.Placeholders)
                });
        }

        public static List<InventoryItemPackageItemToRead> ConvertToReadDtos(IList<InventoryItemPackageItem> items)
        {
            return items?.Select(ConvertToReadDto()).ToList()
                ?? new List<InventoryItemPackageItemToRead>();
        }

        private static Func<InventoryItemPackageItem, InventoryItemPackageItemToRead> ConvertToReadDto()
        {
            return item => item is null
                ? null
                : new InventoryItemPackageItemToRead()
                {
                    Id = item.Id,
                    DisplayOrder = item.DisplayOrder,
                    Item = InventoryItemHelper.ConvertToReadDto(item.Item),
                    Details = new()
                    {
                        Quantity = item.Details.Quantity,
                        ExciseFeeIsAdditional = item.Details.ExciseFeeIsAdditional,
                        LaborAmountIsAdditional = item.Details.LaborAmountIsAdditional,
                        PartAmountIsAdditional = item.Details.PartAmountIsAdditional
                    }
                };
        }

        private static List<InventoryItemPackagePlaceholderToRead> ConvertEntiiesToReadDtos(IList<InventoryItemPackagePlaceholder> placeholders)
        {
            return placeholders?.Select(ConvertEntiyToReadDto()).ToList()
                ?? new List<InventoryItemPackagePlaceholderToRead>();
        }

        private static Func<InventoryItemPackagePlaceholder, InventoryItemPackagePlaceholderToRead> ConvertEntiyToReadDto()
        {
            return placeholder => placeholder is null
            ? null
            : new InventoryItemPackagePlaceholderToRead()
            {
                Id = placeholder.Id,
                DisplayOrder = placeholder.DisplayOrder,
                ItemType = placeholder.ItemType,
                Details = new InventoryItemPackageDetailsToRead()
                {
                    Quantity = placeholder.Details.Quantity,
                    LaborAmountIsAdditional = placeholder.Details.LaborAmountIsAdditional,
                    ExciseFeeIsAdditional = placeholder.Details.ExciseFeeIsAdditional,
                    PartAmountIsAdditional = placeholder.Details.PartAmountIsAdditional
                }
            };
        }

        public static InventoryItemPackage ConvertWriteDtoToEntity(InventoryItemPackageToWrite package, IReadOnlyList<InventoryItem> inventoryItems)
        {
            return package is null
                ? null
                : InventoryItemPackage.Create(
                package.BasePartsAmount,
                package.BaseLaborAmount,
                package.Script,
                package.IsDiscountable,
                ConvertWriteDtosToEntities(package.Items, inventoryItems),
                ConvertWriteDtosToEntities(package.Placeholders))
            .Value;
        }

        private static List<InventoryItemPackageItem> ConvertWriteDtosToEntities(IList<InventoryItemPackageItemToWrite> packageItems, IReadOnlyList<InventoryItem> inventoryItems)
        {
            return packageItems?.Select(
                ConvertWriteDtoToEntity(inventoryItems)).ToList()
                ?? new List<InventoryItemPackageItem>();
        }

        private static Func<InventoryItemPackageItemToWrite, InventoryItemPackageItem> ConvertWriteDtoToEntity(IReadOnlyList<InventoryItem> inventoryItems)
        {
            return item => item is null
                ? null
                : InventoryItemPackageItem.Create(
                    item.DisplayOrder,
                    inventoryItems.FirstOrDefault(x => x.Id == item.Item.Id),
                    InventoryItemPackageDetails.Create(
                        item.Quantity,
                        item.PartAmountIsAdditional,
                        item.LaborAmountIsAdditional,
                        item.ExciseFeeIsAdditional)
                    .Value)
                .Value;
        }

        private static List<InventoryItemPackagePlaceholder> ConvertWriteDtosToEntities(IList<InventoryItemPackagePlaceholderToWrite> placeholders)
        {
            return placeholders?.Select(ConvertWriteDtoToEntity()).ToList()
                ?? new List<InventoryItemPackagePlaceholder>();
        }

        private static Func<InventoryItemPackagePlaceholderToWrite, InventoryItemPackagePlaceholder> ConvertWriteDtoToEntity()
        {
            return placeholder => placeholder is null
            ? null
            : InventoryItemPackagePlaceholder.Create(
                    placeholder.ItemType,
                    placeholder.Description,
                    placeholder.DisplayOrder,
                    InventoryItemPackageDetails.Create(
                        placeholder.Details.Quantity,
                        placeholder.Details.PartAmountIsAdditional,
                        placeholder.Details.LaborAmountIsAdditional,
                        placeholder.Details.ExciseFeeIsAdditional
                        ).Value
                    ).Value;
        }

        public static InventoryItemPackageToWrite ConvertReadToWriteDto(InventoryItemPackageToRead package)
        {
            return package is null
                ? null
                : (new()
                {
                    BasePartsAmount = package.BasePartsAmount,
                    BaseLaborAmount = package.BaseLaborAmount,
                    Script = package.Script,
                    IsDiscountable = package.IsDiscountable,
                    Items = ConvertWriteDtosToRead(package.Items),
                    Placeholders = ConvertWriteDtosToRead(package.Placeholders)
                });
        }

        private static List<InventoryItemPackageItemToWrite> ConvertWriteDtosToRead(IList<InventoryItemPackageItemToRead> items)
        {
            return items?.Select(ConvertWriteDtoToRead()).ToList()
                ?? new List<InventoryItemPackageItemToWrite>();
        }

        private static Func<InventoryItemPackageItemToRead, InventoryItemPackageItemToWrite> ConvertWriteDtoToRead()
        {
            return item =>
                new()
                {
                    Id = item.Id,
                    DisplayOrder = item.DisplayOrder,
                    Item = InventoryItemHelper.ConvertReadToWriteDto(item.Item),
                    ExciseFeeIsAdditional = item.Details.ExciseFeeIsAdditional,
                    LaborAmountIsAdditional = item.Details.LaborAmountIsAdditional,
                    PartAmountIsAdditional = item.Details.PartAmountIsAdditional,
                    Quantity = item.Details.Quantity
                };
        }

        private static List<InventoryItemPackagePlaceholderToWrite> ConvertWriteDtosToRead(IList<InventoryItemPackagePlaceholderToRead> placeholders)
        {
            return placeholders?.Select(ConverPlaceholdertWriteDtoToRead()).ToList()
                ?? new List<InventoryItemPackagePlaceholderToWrite>();
        }

        private static Func<InventoryItemPackagePlaceholderToRead, InventoryItemPackagePlaceholderToWrite> ConverPlaceholdertWriteDtoToRead()
        {
            return placeholder => placeholder is null
            ? new InventoryItemPackagePlaceholderToWrite()
            : new InventoryItemPackagePlaceholderToWrite()
            {
                Id = placeholder.Id,
                DisplayOrder = placeholder.DisplayOrder,
                ItemType = placeholder.ItemType,
                Details = new()
                {
                    ExciseFeeIsAdditional = placeholder.Details.ExciseFeeIsAdditional,
                    LaborAmountIsAdditional = placeholder.Details.LaborAmountIsAdditional,
                    PartAmountIsAdditional = placeholder.Details.PartAmountIsAdditional,
                    Quantity = placeholder.Details.Quantity
                }
            };
        }

        public static InventoryItemPackageToWrite ConvertToWriteDto(InventoryItemPackage package)
        {
            return package is null
                ? new InventoryItemPackageToWrite()
                : (new()
                {
                    BasePartsAmount = package.BasePartsAmount,
                    BaseLaborAmount = package.BaseLaborAmount,
                    Script = package.Script,
                    IsDiscountable = package.IsDiscountable,
                    Items = ConvertToWriteDtos(package.Items),
                    Placeholders = ConvertPlaceholdersToWriteDtos(package.Placeholders)
                });
        }

        private static List<InventoryItemPackagePlaceholderToWrite> ConvertPlaceholdersToWriteDtos(List<InventoryItemPackagePlaceholder> placeholders)
        {
            return
                placeholders?.Select(placeholder =>
                new InventoryItemPackagePlaceholderToWrite()
                {
                    Description = placeholder.Description,
                    Details = placeholder.Details is null
                        ? new InventoryItemPackageDetailsToWrite()
                        : new InventoryItemPackageDetailsToWrite()
                        {
                            ExciseFeeIsAdditional = placeholder.Details.ExciseFeeIsAdditional,
                            LaborAmountIsAdditional = placeholder.Details.LaborAmountIsAdditional,
                            PartAmountIsAdditional = placeholder.Details.PartAmountIsAdditional,
                            Quantity = placeholder.Details.Quantity
                        },
                    DisplayOrder = placeholder.DisplayOrder,
                    Id = placeholder.Id,
                    ItemType = placeholder.ItemType
                }).ToList()
                ??
                new List<InventoryItemPackagePlaceholderToWrite>();
        }

        private static List<InventoryItemPackageItemToWrite> ConvertToWriteDtos(List<InventoryItemPackageItem> packages)
        {
            return
                packages?.Select(package => 
                new InventoryItemPackageItemToWrite()
                {
                    DisplayOrder = package.DisplayOrder,
                    Id = package.Id,
                    ExciseFeeIsAdditional = package.Details.ExciseFeeIsAdditional,
                    LaborAmountIsAdditional = package.Details.LaborAmountIsAdditional,
                    Quantity = package.Details.Quantity,
                    PartAmountIsAdditional = package.Details.PartAmountIsAdditional,
                    Item = InventoryItemHelper.ConvertToWriteDto(package.Item)
                }).ToList()
                ??
                new List<InventoryItemPackageItemToWrite>();
        }

    }
}
