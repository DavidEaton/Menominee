using CSharpFunctionalExtensions;
using System;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPart : InstallablePart
    {
        private InventoryItemPart(double list, double cost, double core, double retail, TechAmount techAmount, bool fractional, string lineCode = null, string subLineCode = null)
            : base(list, cost, core, retail, techAmount, fractional, lineCode, subLineCode)
        { }

        public static Result<InventoryItemPart> Create(double list, double cost, double core, double retail, TechAmount techAmount, bool fractional, string lineCode = null, string subLineCode = null)
        {
            if (list < MinimumValue ||
                cost < MinimumValue ||
                core < MinimumValue ||
                retail < MinimumValue ||
                list > MaximumValue ||
                cost > MaximumValue ||
                core > MaximumValue ||
                retail > MaximumValue)
            return Result.Failure<InventoryItemPart>(InvalidValueMessage);

            lineCode = (lineCode ?? string.Empty).Trim();
            subLineCode = (subLineCode ?? string.Empty).Trim();

            // TechAmount Value Object is validated before we ever get here

            if (lineCode.Length < MinimumLength ||
                lineCode.Length > MaximumLength ||
                subLineCode.Length < MinimumLength ||
                subLineCode.Length > MaximumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            return Result.Success(new InventoryItemPart(list, cost, core, retail, techAmount, fractional, lineCode, subLineCode));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPart() { }

        #endregion
    }
}
