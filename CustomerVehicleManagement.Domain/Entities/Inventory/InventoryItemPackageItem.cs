using CSharpFunctionalExtensions;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPackageItem : Entity
    {
        public static readonly string RequiredMessage = "Please include all required items.";
        public static readonly int MinimumValue = 0;
        public static readonly string MinimumValueMessage = $"Value must be > {MinimumValue}.";

        public InventoryItemPackage InventoryItemPackage { get; private set; }
        public int DisplayOrder { get; private set; }
        public InventoryItem InventoryItem { get; private set; }
        public InventoryItemPackageDetails Details { get; private set; }

        private InventoryItemPackageItem(InventoryItemPackage package, int displayOrder, InventoryItem inventoryItem, InventoryItemPackageDetails details)
        {
            if (package is null || inventoryItem is null || details is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (displayOrder < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            InventoryItemPackage = package;
            DisplayOrder = displayOrder;
            InventoryItem = inventoryItem;
            Details = details;
        }

        public static Result<InventoryItemPackageItem> Create(InventoryItemPackage package, int displayOrder, InventoryItem inventoryItem, InventoryItemPackageDetails details)
        {
            if (package is null || inventoryItem is null || details is null)
                return Result.Failure<InventoryItemPackageItem>(RequiredMessage);

            if (displayOrder < MinimumValue)
                return Result.Failure<InventoryItemPackageItem>(MinimumValueMessage);

            return Result.Success(new InventoryItemPackageItem(package, displayOrder, inventoryItem, details));
        }

        public Result<InventoryItem> SetInventoryItem(InventoryItem inventoryItem)
        {
            if (inventoryItem is null)
                return Result.Failure<InventoryItem>(RequiredMessage);

            return Result.Success(InventoryItem = inventoryItem);
        }

        public Result<int> SetDisplayOrder(int displayOrder)
        {
            return Result.Success(DisplayOrder = displayOrder);
        }

        public Result<InventoryItemPackageDetails> SetDetails(InventoryItemPackageDetails details)
        {
            // deatils paremeter has already been validated
            // by the time we get here (type InventoryItemPackageDetails
            // Value Object)
            return Result.Success(Details = details);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackageItem() { }

        #endregion    
    }
}
