using CSharpFunctionalExtensions;
using Entity = Menominee.Domain.BaseClasses.Entity;

namespace Menominee.Domain.Entities.Inventory
{
    public class MaintenanceItem : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly string InvalidMessage = $"Please enter a valid Display Order";
        public static readonly int MinimumValue = 0;
        public int DisplayOrder { get; private set; }
        public InventoryItem InventoryItem { get; private set; }

        private MaintenanceItem(int displayOrder, InventoryItem inventoryItem)
        {
            DisplayOrder = displayOrder;
            InventoryItem = inventoryItem;
        }

        public static Result<MaintenanceItem> Create(int displayOrder, InventoryItem inventoryItem)
        {
            if (inventoryItem is null)
                return Result.Failure<MaintenanceItem>(RequiredMessage);

            if (displayOrder < 0)
                return Result.Failure<MaintenanceItem>(InvalidMessage);

            return Result.Success(new MaintenanceItem(displayOrder, inventoryItem));
        }

        public Result<InventoryItem> SetInventoryItem(InventoryItem inventoryItem)
        {
            if (inventoryItem is null)
                return Result.Failure<InventoryItem>(RequiredMessage);

            return Result.Success(InventoryItem = inventoryItem);
        }

        public Result<int> SetDisplayOrder(int displayOrder)
        {
            if (displayOrder < MinimumValue)
                return Result.Failure<int>(InvalidMessage);

            return Result.Success(DisplayOrder = displayOrder);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected MaintenanceItem() { }

        #endregion
    }
}
