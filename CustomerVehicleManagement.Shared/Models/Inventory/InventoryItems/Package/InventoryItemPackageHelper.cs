using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackageHelper
    {
        #region <---- ConvertEntityToReadDto ---->

        public static InventoryItemPackageToRead ConvertEntityToReadDto(InventoryItemPackage package)
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
                    Items = ConvertEntiiesToReadDtos(package.Items),
                    Placeholders = ConvertEntiiesToReadDtos(package.Placeholders)
                });
        }

        private static List<InventoryItemPackageItemToRead> ConvertEntiiesToReadDtos(IList<InventoryItemPackageItem> items)
        {
            return items?.Select(ConvertEntityToReadDto()).ToList()
                ?? new List<InventoryItemPackageItemToRead>();
        }

        private static Func<InventoryItemPackageItem, InventoryItemPackageItemToRead> ConvertEntityToReadDto()
        {
            return item =>
                new InventoryItemPackageItemToRead()
                {
                    Id = item.Id,
                    DisplayOrder = item.DisplayOrder,
                    Item = InventoryItemHelper.ConvertEntityToReadDto(item.Item),
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
            return placeholder =>
                new InventoryItemPackagePlaceholderToRead()
                {
                    Id = placeholder.Id,
                    DisplayOrder = placeholder.DisplayOrder,
                    ItemType = placeholder.ItemType.ToString(),
                    Details = new InventoryItemPackageDetailsToRead()
                    {
                        Quantity = placeholder.Details.Quantity,
                        LaborAmountIsAdditional = placeholder.Details.LaborAmountIsAdditional,
                        ExciseFeeIsAdditional = placeholder.Details.ExciseFeeIsAdditional,
                        PartAmountIsAdditional = placeholder.Details.PartAmountIsAdditional
                    }
                };
        }
        #endregion

        #region <---- ConvertWriteDtoToEntity ---->

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
            return packageItems?.Select(ConvertWriteDtoToEntity(inventoryItems)).ToList()
                ?? new List<InventoryItemPackageItem>();
        }

        private static Func<InventoryItemPackageItemToWrite, InventoryItemPackageItem> ConvertWriteDtoToEntity(IReadOnlyList<InventoryItem> inventoryItems)
        {
            return item =>
                InventoryItemPackageItem.Create(
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
            return placeholder =>
                InventoryItemPackagePlaceholder.Create(
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

        #endregion

        #region <---- ConvertReadToWriteDto ---->

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
            return placeholder =>
                new InventoryItemPackagePlaceholderToWrite()
                {
                    Id = placeholder.Id,
                    DisplayOrder = placeholder.DisplayOrder,
                    ItemType = Utilities.ParseEnum<PackagePlaceholderItemType>(placeholder.ItemType),
                    Details = new()
                    {
                        ExciseFeeIsAdditional = placeholder.Details.ExciseFeeIsAdditional,
                        LaborAmountIsAdditional = placeholder.Details.LaborAmountIsAdditional,
                        PartAmountIsAdditional = placeholder.Details.PartAmountIsAdditional,
                        Quantity = placeholder.Details.Quantity
                    }
                };
        }
        #endregion
    }
}
