using CSharpFunctionalExtensions;
using Entity = Menominee.Domain.BaseClasses.Entity;

namespace Menominee.Domain.Entities.Inventory
{
    public class InventoryItemPackageItem : Entity
    {
        public static readonly string RequiredMessage = "Please include all required items.";
        public static readonly int MinimumValue = 0;
        public static readonly string MinimumValueMessage = $"Value must be > {MinimumValue}.";

        public int DisplayOrder { get; private set; }
        public InventoryItem Item { get; private set; }
        public InventoryItemPackageDetails Details { get; private set; }

        private InventoryItemPackageItem(int displayOrder, InventoryItem inventoryItem, InventoryItemPackageDetails details)
        {
            DisplayOrder = displayOrder;
            Item = inventoryItem;
            Details = details;
        }

        public static Result<InventoryItemPackageItem> Create(int displayOrder, InventoryItem inventoryItem, InventoryItemPackageDetails details)
        {
            if (displayOrder < MinimumValue)
                return Result.Failure<InventoryItemPackageItem>(MinimumValueMessage);

            if (inventoryItem is null)
                return Result.Failure<InventoryItemPackageItem>(RequiredMessage);

            if (details is null)
                return Result.Failure<InventoryItemPackageItem>(RequiredMessage);

            return Result.Success(new InventoryItemPackageItem(displayOrder, inventoryItem, details));
        }

        public Result<InventoryItem> SetInventoryItem(InventoryItem inventoryItem)
        {
            if (inventoryItem is null)
                return Result.Failure<InventoryItem>(RequiredMessage);

            return Result.Success(Item = inventoryItem);
        }

        public Result<int> SetDisplayOrder(int displayOrder)
        {
            if (displayOrder < MinimumValue)
                return Result.Failure<int>(MinimumValueMessage);

            return Result.Success(DisplayOrder = displayOrder);
        }

        public Result<InventoryItemPackageDetails> SetDetails(InventoryItemPackageDetails details)
        {
            // deatils paremeter has already been validated
            // by the time we get here (type InventoryItemPackageDetails
            // Value Object)
            if (details is null)
                return Result.Failure<InventoryItemPackageDetails>(RequiredMessage);

            return Result.Success(Details = details);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackageItem() { }

        #endregion    
    }
}
