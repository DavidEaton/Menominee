using CSharpFunctionalExtensions;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemWarranty : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public InventoryItem InventoryItem { get; private set; }
        public InventoryItemWarrantyPeriod Period { get; private set; }

        private InventoryItemWarranty(InventoryItem inventoryItem, InventoryItemWarrantyPeriod period)
        {
            if (inventoryItem is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (!Enum.IsDefined(typeof(InventoryItemWarrantyPeriod), period))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            InventoryItem = inventoryItem;
            Period = period;
        }

        public static Result<InventoryItemWarranty> Create(InventoryItem inventoryItem, InventoryItemWarrantyPeriod period)
        {
            if (inventoryItem is null)
                return Result.Failure<InventoryItemWarranty>(RequiredMessage);

            if (!Enum.IsDefined(typeof(InventoryItemWarrantyPeriod), period))
                return Result.Failure<InventoryItemWarranty>(RequiredMessage);

            return Result.Success(new InventoryItemWarranty(inventoryItem, period));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemWarranty() { }

        #endregion  
    }
}
