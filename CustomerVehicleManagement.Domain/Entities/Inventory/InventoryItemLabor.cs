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

        public Result<InventoryItem> SetInventoryItem(InventoryItem inventoryItem)
        {
            if (inventoryItem is null)
                return Result.Failure<InventoryItem>(RequiredMessage);

            return Result.Success(InventoryItem = inventoryItem);
        }

        public Result<LaborAmount> SetLaborAmount(LaborAmount laborAmount)
        {
            if (laborAmount is null)
                return Result.Failure<LaborAmount>(RequiredMessage);

            return Result.Success(LaborAmount = laborAmount);
        }

        public Result<TechAmount> SetTechAmount(TechAmount techAmount)
        {
            if (techAmount is null)
                return Result.Failure<TechAmount>(RequiredMessage);

            return Result.Success(TechAmount = techAmount);
        }


        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemLabor() { }

        #endregion
    }
}
