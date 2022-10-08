using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemPackageHelper
    {
        #region <---- ConvertEntityToReadDto ---->

        public static InventoryItemPackageToRead ConvertEntityToReadDto(InventoryItemPackage package)
        {
            if (package is null)
                return null;

            return new()
            {
                Id = package.Id,
                BasePartsAmount = package.BasePartsAmount,
                BaseLaborAmount = package.BaseLaborAmount,
                Script = package.Script,
                IsDiscountable = package.IsDiscountable,
                Items = ConvertEntiiesToReadDtos(package.Items),
                Placeholders = ProjectPlaceholders(package.Placeholders)
            };
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
                    Item = InventoryItemHelper.ConvertEntityToReadDto(item.InventoryItem),
                    Details = new()
                    {
                        Quantity = item.Details.Quantity,
                        ExciseFeeIsAdditional = item.Details.ExciseFeeIsAdditional,
                        LaborAmountIsAdditional = item.Details.LaborAmountIsAdditional,
                        PartAmountIsAdditional = item.Details.PartAmountIsAdditional
                    }
                };
        }

        private static List<InventoryItemPackagePlaceholderToRead> ProjectPlaceholders(IList<InventoryItemPackagePlaceholder> placeholders)
        {
            return placeholders?.Select(TransformPlaceholderToReadToEntity()).ToList()
                ?? new List<InventoryItemPackagePlaceholderToRead>();
        }

        private static Func<InventoryItemPackagePlaceholder, InventoryItemPackagePlaceholderToRead> TransformPlaceholderToReadToEntity()
        {
            return placeholder =>
                            new InventoryItemPackagePlaceholderToRead()
                            {
                                Id = placeholder.Id,
                                Order = placeholder.DisplayOrder,
                                Description = placeholder.Description,
                                ItemType = placeholder.ItemType,
                                Quantity = placeholder.Quantity,
                                PartAmountIsAdditional = placeholder.PartAmountIsAdditional,
                                LaborAmountIsAdditional = placeholder.LaborAmountIsAdditional,
                                ExciseFeeIsAdditional = placeholder.ExciseFeeIsAdditional
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
                            InventoryItemPackageItem.Create(item.Order, )
                            {
                //Id = item.Id,
                Order = item.Order,
                                InventoryItemId = item.Item.Id,
                                //Item = InventoryItemHelper.ConvertWriteDtoToEntity(item.Item),
                                Quantity = item.Quantity,
                                PartAmountIsAdditional = item.PartAmountIsAdditional,
                                LaborAmountIsAdditional = item.LaborAmountIsAdditional,
                                ExciseFeeIsAdditional = item.ExciseFeeIsAdditional
                            };
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
                                (PackagePlaceholderItemType)Enum.Parse(
                                    typeof(PackagePlaceholderItemType),
                                    placeholder.ItemType),
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
            if (package is null)
                return null;

            return new()
            {
                BasePartsAmount = package.BasePartsAmount,
                BaseLaborAmount = package.BaseLaborAmount,
                Script = package.Script,
                IsDiscountable = package.IsDiscountable,
                Items = ProjectItemsToRead(package.Items),
                Placeholders = ProjectPlaceholdersToRead(package.Placeholders)
            };
        }

        private static List<InventoryItemPackageItemToWrite> ProjectItemsToRead(IList<InventoryItemPackageItemToRead> items)
        {
            return items?.Select(TransformItemToRead()).ToList()
                ?? new List<InventoryItemPackageItemToWrite>();
        }

        private static Func<InventoryItemPackageItemToRead, InventoryItemPackageItemToWrite> TransformItemToRead()
        {
            return item =>
                            new InventoryItemPackageItemToWrite()
                            {
                                Id = item.Id,
                                Order = item.Order,
                                Item = InventoryItemHelper.ConvertReadToWriteDto(item.Item),
                                InventoryItemId = item.InventoryItemId,
                                Quantity = item.Quantity,
                                PartAmountIsAdditional = item.PartAmountIsAdditional,
                                LaborAmountIsAdditional = item.LaborAmountIsAdditional,
                                ExciseFeeIsAdditional = item.ExciseFeeIsAdditional
                            };
        }

        private static List<InventoryItemPackagePlaceholderToWrite> ProjectPlaceholdersToRead(IList<InventoryItemPackagePlaceholderToRead> placeholders)
        {
            return placeholders?.Select(TransformPlaceholderToRead()).ToList()
                ?? new List<InventoryItemPackagePlaceholderToWrite>();
        }

        private static Func<InventoryItemPackagePlaceholderToRead, InventoryItemPackagePlaceholderToWrite> TransformPlaceholderToRead()
        {
            return placeholder =>
                            new InventoryItemPackagePlaceholderToWrite()
                            {
                                Id = placeholder.Id,
                                Order = placeholder.Order,
                                Description = placeholder.Description,
                                ItemType = placeholder.ItemType,
                                Quantity = placeholder.Quantity,
                                PartAmountIsAdditional = placeholder.PartAmountIsAdditional,
                                LaborAmountIsAdditional = placeholder.LaborAmountIsAdditional,
                                ExciseFeeIsAdditional = placeholder.ExciseFeeIsAdditional
                            };
        }
        #endregion

    }
}
