using CSharpFunctionalExtensions;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemWarranty : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public InventoryItem InventoryItem { get; private set; }
        public InventoryItemWarrantyPeriod WarrantyPeriod { get; private set; }

        private InventoryItemWarranty(InventoryItem inventoryItem, InventoryItemWarrantyPeriod warrantyPeriod)
        {
            if (inventoryItem is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (!Enum.IsDefined(typeof(InventoryItemWarrantyPeriod), warrantyPeriod))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            InventoryItem = inventoryItem;
            WarrantyPeriod = warrantyPeriod;
        }

        public static Result<InventoryItemWarranty> Create(InventoryItem inventoryItem, InventoryItemWarrantyPeriod warrantyPeriod)
        {
            if (inventoryItem is null)
                return Result.Failure<InventoryItemWarranty>(RequiredMessage);

            if (!Enum.IsDefined(typeof(InventoryItemWarrantyPeriod), warrantyPeriod))
                return Result.Failure<InventoryItemWarranty>(RequiredMessage);

            return Result.Success(new InventoryItemWarranty(inventoryItem, warrantyPeriod));
        }

        public Result<InventoryItem> SetInventoryItem(InventoryItem inventoryItem)
        {
            if (inventoryItem is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            return Result.Success(InventoryItem = inventoryItem);
        }

        public Result<InventoryItemWarrantyPeriod> SetWarrantyPeriod(InventoryItemWarrantyPeriod warrantyPeriod)
        {
            if (warrantyPeriod is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            return Result.Success(WarrantyPeriod = warrantyPeriod);
        }


        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemWarranty() { }

        #endregion  
    }
}
