using CSharpFunctionalExtensions;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class MaintenanceItem : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public int DisplayOrder { get; private set; }
        public InventoryItem Item { get; private set; }

        private MaintenanceItem(int displayOrder, InventoryItem inventoryItem)
        {
            if (inventoryItem is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            DisplayOrder = displayOrder;
            Item = inventoryItem;
        }

        public static Result<MaintenanceItem> Create(int displayOrder, InventoryItem inventoryItem)
        {
            if (inventoryItem is null)
                return Result.Failure<MaintenanceItem>(RequiredMessage);

            return Result.Success(new MaintenanceItem(displayOrder, inventoryItem));
        }

        public Result<InventoryItem> SetInventoryItem(InventoryItem inventoryItem)
        {
            if (inventoryItem is null)
                return Result.Failure<InventoryItem>(RequiredMessage);

            return Result.Success(Item = inventoryItem);
        }

        public Result<int> SetDisplayOrder(int displayOrder)
        {
            return Result.Success(DisplayOrder = displayOrder);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected MaintenanceItem() { }

        #endregion
    }
}
