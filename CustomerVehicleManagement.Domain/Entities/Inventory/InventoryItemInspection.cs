using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemInspection : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";

        public InventoryItem InventoryItem { get; private set; }
        public LaborAmount LaborAmount { get; set; }
        public TechAmount TechAmount { get; set; }
        public InventoryItemInspectionType Type { get; private set; }

        private InventoryItemInspection(InventoryItem inventoryItem, LaborAmount laborAmount, TechAmount techAmount, InventoryItemInspectionType type)
        {
            if (inventoryItem is null || laborAmount is null || techAmount is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (!Enum.IsDefined(typeof(InventoryItemInspectionType), type))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            InventoryItem = inventoryItem;
            LaborAmount = laborAmount;
            TechAmount = techAmount;
            Type = type;
        }
        public static Result<InventoryItemInspection> Create(InventoryItem inventoryItem, LaborAmount laborAmount, TechAmount techAmount, InventoryItemInspectionType type)
        {
            if (inventoryItem is null || laborAmount is null || techAmount is null)
                return Result.Failure<InventoryItemInspection>(RequiredMessage);

            if (!Enum.IsDefined(typeof(InventoryItemInspectionType), type))
                return Result.Failure<InventoryItemInspection>(RequiredMessage);

            return Result.Success(new InventoryItemInspection(inventoryItem, laborAmount, techAmount, type));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemInspection() { }

        #endregion    
    }
}