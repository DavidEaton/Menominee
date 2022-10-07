using CustomerVehicleManagement.Domain.Entities.Inventory;
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
                Items = ProjectItems(package.Items),
                Placeholders = ProjectPlaceholders(package.Placeholders)
            };
        }

        private static List<InventoryItemPackageItemToRead> ProjectItems(IList<InventoryItemPackageItem> items)
        {
            return items?.Select(TransformItemEntityToRead()).ToList()
                ?? new List<InventoryItemPackageItemToRead>();
        }

        private static Func<InventoryItemPackageItem, InventoryItemPackageItemToRead> TransformItemEntityToRead()
        {
            return item =>
                            new InventoryItemPackageItemToRead()
                            {
                                Id = item.Id,
                                Order = item.DisplayOrder,
                                Item = InventoryItemHelper.ConvertEntityToReadDto(item.InventoryItem),
                                Quantity = item.Quantity,
                                PartAmountIsAdditional = item.PartAmountIsAdditional,
                                LaborAmountIsAdditional = item.LaborAmountIsAdditional,
                                ExciseFeeIsAdditional = item.ExciseFeeIsAdditional
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

        public static InventoryItemPackage ConvertWriteDtoToEntity(InventoryItemPackageToWrite package)
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

        private static List<InventoryItemPackageItem> ProjectItemsToWrite(IList<InventoryItemPackageItemToWrite> items)
        {
            return items?.Select(TransformItem()).ToList()
                ?? new List<InventoryItemPackageItem>();
        }

        private static Func<InventoryItemPackageItemToWrite, InventoryItemPackageItem> TransformItem()
        {
            return item =>
                            new InventoryItemPackageItem()
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

        private static List<InventoryItemPackagePlaceholder> ProjectPlaceholdersToWrite(IList<InventoryItemPackagePlaceholderToWrite> placeholders)
        {
            return placeholders?.Select(TransformPlaceholder()).ToList()
                ?? new List<InventoryItemPackagePlaceholder>();
        }

        private static Func<InventoryItemPackagePlaceholderToWrite, InventoryItemPackagePlaceholder> TransformPlaceholder()
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

        public static void CopyWriteDtoToEntity(InventoryItemPackageToWrite packageToWrite, InventoryItemPackage package)
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
