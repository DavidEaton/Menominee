using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemInspection : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public LaborAmount LaborAmount { get; private set; } //optional
        public TechAmount TechAmount { get; private set; } //optional
        public InventoryItemInspectionType InspectionType { get; private set; } //required
        private InventoryItemInspection(LaborAmount laborAmount, TechAmount techAmount, InventoryItemInspectionType type)
        {
            if (!Enum.IsDefined(typeof(InventoryItemInspectionType), type))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            LaborAmount = laborAmount;
            TechAmount = techAmount;
            InspectionType = type;
        }

        public static Result<InventoryItemInspection> Create(LaborAmount laborAmount, TechAmount techAmount, InventoryItemInspectionType type)
        {
            if (!Enum.IsDefined(typeof(InventoryItemInspectionType), type))
                return Result.Failure<InventoryItemInspection>(RequiredMessage);

            return Result.Success(new InventoryItemInspection(laborAmount, techAmount, type));
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

        public Result<InventoryItemInspectionType> SetInspectionType(InventoryItemInspectionType inspectionType)
        {
            if (!Enum.IsDefined(typeof(InventoryItemInspectionType), inspectionType))
                return Result.Failure<InventoryItemInspectionType>(RequiredMessage);

            return Result.Success(InspectionType = inspectionType);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemInspection() { }

        #endregion    
    }
}