using CSharpFunctionalExtensions;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPackageDetails : AppValueObject
    {
        public static readonly string RequiredMessage = "Please include all required items.";
        public static readonly int MinimumValue = 0;
        public static readonly int MaximumValue = 99999;
        public static readonly string MinimumValueMessage = $"Value must be > {MinimumValue}.";

        public double Quantity { get; private set; }
        public bool PartAmountIsAdditional { get; private set; } = false;
        public bool LaborAmountIsAdditional { get; private set; } = false;
        public bool ExciseFeeIsAdditional { get; private set; } = false;

        private InventoryItemPackageDetails(double quantity, bool partAmountIsAdditional, bool laborAmountIsAdditional, bool exciseFeeIsAdditional)
        {
            if (quantity <= MinimumValue || quantity > MaximumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            Quantity = quantity;
            PartAmountIsAdditional = partAmountIsAdditional;
            LaborAmountIsAdditional = laborAmountIsAdditional;
            ExciseFeeIsAdditional = exciseFeeIsAdditional;
        }

        public static Result<InventoryItemPackageDetails> Create(double quantity, bool partAmountIsAdditional, bool laborAmountIsAdditional, bool exciseFeeIsAdditional)
        {
            if (quantity <= MinimumValue || quantity > MaximumValue)
                return Result.Failure<InventoryItemPackageDetails>(MinimumValueMessage);

            return Result.Success(new InventoryItemPackageDetails(quantity, partAmountIsAdditional, laborAmountIsAdditional, exciseFeeIsAdditional));
        }

        public Result<InventoryItemPackageDetails> SetQuantity(double quantity)
        {
            if (quantity <= MinimumValue || quantity > MaximumValue)
                return Result.Failure<InventoryItemPackageDetails>(MinimumValueMessage);

            return Result.Success(new InventoryItemPackageDetails(
                quantity, PartAmountIsAdditional, LaborAmountIsAdditional, ExciseFeeIsAdditional)).Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Quantity;
            yield return PartAmountIsAdditional;
            yield return LaborAmountIsAdditional;
            yield return ExciseFeeIsAdditional;
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackageDetails() { }

        #endregion    
    }
}
