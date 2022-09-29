using CSharpFunctionalExtensions;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class MaintenanceItem : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public long DisplayOrder { get; private set; }
        public InventoryItem Item { get; private set; }

        private MaintenanceItem(long displayOrder, InventoryItem inventoryItem)
        {
            if (inventoryItem is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            DisplayOrder = displayOrder;
            Item = inventoryItem;
        }

        public static Result<MaintenanceItem> Create(long displayOrder, InventoryItem inventoryItem)
        {
            if (inventoryItem is null)
                return Result.Failure<MaintenanceItem>(RequiredMessage);

            return Result.Success(new MaintenanceItem(displayOrder, inventoryItem));
        }



        #region ORM

        // EF requires a parameterless constructor
        protected MaintenanceItem() { }

        #endregion
    }
}
