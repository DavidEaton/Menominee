using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemWarrantyPeriod : AppValueObject
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly string NonNegativeMessage = $"Must be a positive value.";

        public InventoryItemWarrantyPeriodType PeriodType { get; private set; }
        public int Duration { get; private set; }

        private InventoryItemWarrantyPeriod(InventoryItemWarrantyPeriodType periodType, int duration)
        {
            if (!Enum.IsDefined(typeof(InventoryItemWarrantyPeriodType), periodType))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (duration < 0)
                throw new ArgumentOutOfRangeException(NonNegativeMessage);

            PeriodType = periodType;
            Duration = duration;
        }

        public static Result<InventoryItemWarrantyPeriod> Create(InventoryItemWarrantyPeriodType periodType, int duration)
        {
            if (!Enum.IsDefined(typeof(InventoryItemWarrantyPeriodType), periodType))
                return Result.Failure<InventoryItemWarrantyPeriod>(RequiredMessage);

            if (duration < 0)
                return Result.Failure<InventoryItemWarrantyPeriod>(RequiredMessage);

            return Result.Success(new InventoryItemWarrantyPeriod(periodType, duration));
        }

        public Result<InventoryItemWarrantyPeriodType> SetPeriodType(InventoryItemWarrantyPeriodType periodType)
        {
            if (!Enum.IsDefined(typeof(InventoryItemWarrantyPeriodType), periodType))
                return Result.Failure<InventoryItemWarrantyPeriodType>(RequiredMessage);

            return Result.Success(PeriodType = periodType);
        }

        public Result<int> SetDuration(int duration)
        {
            if (duration < 0)
                return Result.Failure<int>(NonNegativeMessage);

            return Result.Success(Duration = duration);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PeriodType;
            yield return Duration;
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemWarrantyPeriod() { }

        #endregion    
    }
}
