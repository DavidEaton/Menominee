using CSharpFunctionalExtensions;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemLabor : Entity
    {
        public static readonly string RequiredMessage = "Please include all required items.";

        public InventoryItem InventoryItem { get; private set; }
        public LaborAmount LaborAmount { get; private set; }
        public TechAmount TechAmount { get; private set; }

        private InventoryItemLabor(InventoryItem inventoryItem, LaborAmount laborAmount, TechAmount techAmount)
        {
            if (inventoryItem is null || laborAmount is null || techAmount is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            InventoryItem = inventoryItem;
            LaborAmount = laborAmount;
            TechAmount = techAmount;
        }

        public static Result<InventoryItemLabor> Create(InventoryItem inventoryItem, LaborAmount laborAmount, TechAmount techAmount)
        {
            if (inventoryItem is null || laborAmount is null || techAmount is null)
                return Result.Failure<InventoryItemLabor>(RequiredMessage);

            return Result.Success(new InventoryItemLabor(inventoryItem, laborAmount, techAmount));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemLabor() { }

        #endregion
    }
}
