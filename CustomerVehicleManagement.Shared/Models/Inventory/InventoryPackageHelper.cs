using CustomerVehicleManagement.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryPackageHelper
    {
        #region <---- Create InventoryPackageToRead from InventoryItemPackage ---->

        public static InventoryPackageToRead CreateInventoryPackage(InventoryItemPackage package)
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
                Items = ProjectItems(package.Items),
                Placeholders = ProjectPlaceholders(package.Placeholders)
            };
        }

        private static List<InventoryPackageItemToRead> ProjectItems(IList<InventoryItemPackageItem> items)
        {
            return items?.Select(TransformItemEntityToRead()).ToList()
                ?? new List<InventoryPackageItemToRead>();
        }

        private static Func<InventoryItemPackageItem, InventoryPackageItemToRead> TransformItemEntityToRead()
        {
            return item =>
                            new InventoryPackageItemToRead()
                            {
                                Id = item.Id,
                                Order = item.Order,
                                Item = InventoryItemHelper.CreateInventoryItem(item.Item),
                                Quantity = item.Quantity,
                                PartAmountIsAdditional = item.PartAmountIsAdditional,
                                LaborAmountIsAdditional = item.LaborAmountIsAdditional,
                                ExciseFeeIsAdditional = item.ExciseFeeIsAdditional
                            };
        }

        private static List<InventoryPackagePlaceholderToRead> ProjectPlaceholders(IList<InventoryItemPackagePlaceholder> placeholders)
        {
            return placeholders?.Select(TransformPlaceholderToReadToEntity()).ToList()
                ?? new List<InventoryPackagePlaceholderToRead>();
        }

        private static Func<InventoryItemPackagePlaceholder, InventoryPackagePlaceholderToRead> TransformPlaceholderToReadToEntity()
        {
            return placeholder =>
                            new InventoryPackagePlaceholderToRead()
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

        #region <---- Create InventoryItemPackage from InventoryPackageToWrite ---->

        public static InventoryItemPackage CreateInventoryPackage(InventoryPackageToWrite package)
        {
            if (package is null)
                return null;

            return new()
            {
                BasePartsAmount = package.BasePartsAmount,
                BaseLaborAmount = package.BaseLaborAmount,
                Script = package.Script,
                IsDiscountable = package.IsDiscountable,
                Items = ProjectItemsToWrite(package.Items),
                Placeholders = ProjectPlaceholdersToWrite(package.Placeholders)
            };
        }

        private static List<InventoryItemPackageItem> ProjectItemsToWrite(IList<InventoryPackageItemToWrite> items)
        {
            return items?.Select(TransformItem()).ToList()
                ?? new List<InventoryItemPackageItem>();
        }

        private static Func<InventoryPackageItemToWrite, InventoryItemPackageItem> TransformItem()
        {
            return item =>
                            new InventoryItemPackageItem()
                            {
                                //Id = item.Id,
                                Order = item.Order,
                                Item = InventoryItemHelper.CreateInventoryItem(item.Item),
                                Quantity = item.Quantity,
                                PartAmountIsAdditional = item.PartAmountIsAdditional,
                                LaborAmountIsAdditional = item.LaborAmountIsAdditional,
                                ExciseFeeIsAdditional = item.ExciseFeeIsAdditional
                            };
        }

        private static List<InventoryItemPackagePlaceholder> ProjectPlaceholdersToWrite(IList<InventoryPackagePlaceholderToWrite> placeholders)
        {
            return placeholders?.Select(TransformPlaceholder()).ToList()
                ?? new List<InventoryItemPackagePlaceholder>();
        }

        private static Func<InventoryPackagePlaceholderToWrite, InventoryItemPackagePlaceholder> TransformPlaceholder()
        {
            return placeholder =>
                            new InventoryItemPackagePlaceholder()
                            {
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

        #region <---- Create InventoryPackageToWrite from InventoryPackageToRead ---->

        public static InventoryPackageToWrite CreateInventoryPackage(InventoryPackageToRead package)
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

        private static List<InventoryPackageItemToWrite> ProjectItemsToRead(IList<InventoryPackageItemToRead> items)
        {
            return items?.Select(TransformItemToRead()).ToList()
                ?? new List<InventoryPackageItemToWrite>();
        }

        private static Func<InventoryPackageItemToRead, InventoryPackageItemToWrite> TransformItemToRead()
        {
            return item =>
                            new InventoryPackageItemToWrite()
                            {
                                Id = item.Id,
                                Order = item.Order,
                                Item = InventoryItemHelper.CreateInventoryItem(item.Item),
                                Quantity = item.Quantity,
                                PartAmountIsAdditional = item.PartAmountIsAdditional,
                                LaborAmountIsAdditional = item.LaborAmountIsAdditional,
                                ExciseFeeIsAdditional = item.ExciseFeeIsAdditional
                            };
        }

        private static List<InventoryPackagePlaceholderToWrite> ProjectPlaceholdersToRead(IList<InventoryPackagePlaceholderToRead> placeholders)
        {
            return placeholders?.Select(TransformPlaceholderToRead()).ToList()
                ?? new List<InventoryPackagePlaceholderToWrite>();
        }

        private static Func<InventoryPackagePlaceholderToRead, InventoryPackagePlaceholderToWrite> TransformPlaceholderToRead()
        {
            return placeholder =>
                            new InventoryPackagePlaceholderToWrite()
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

        public static void CopyInventoryPackage(InventoryPackageToWrite packageToWrite, InventoryItemPackage package)
        {

            package.BasePartsAmount = packageToWrite.BasePartsAmount;
            package.BaseLaborAmount = packageToWrite.BaseLaborAmount;
            package.Script = packageToWrite.Script;
            package.IsDiscountable = packageToWrite.IsDiscountable;
            package.Items = ProjectItemsToWrite(packageToWrite.Items);
            package.Placeholders = ProjectPlaceholdersToWrite(packageToWrite.Placeholders);
        }
    }
}
