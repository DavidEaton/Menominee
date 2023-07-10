using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace Menominee.Domain.Entities.Inventory
{
    public class InventoryItemWarrantyPeriod : AppValueObject
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly string NonNegativeMessage = $"Must be a positive value.";

        public InventoryItemWarrantyPeriodType PeriodType { get; private set; }
        public int Duration { get; private set; }

        private InventoryItemWarrantyPeriod(InventoryItemWarrantyPeriodType periodType, int duration)
        {
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

        public Result<InventoryItemWarrantyPeriod> NewPeriodType(InventoryItemWarrantyPeriodType periodType)
        {
            if (!Enum.IsDefined(typeof(InventoryItemWarrantyPeriodType), periodType))
                return Result.Failure<InventoryItemWarrantyPeriod>(RequiredMessage);

            return Result.Success(new InventoryItemWarrantyPeriod(periodType, Duration));
        }

        public Result<InventoryItemWarrantyPeriod> NewDuration(int duration)
        {
            if (duration < 0)
                return Result.Failure<InventoryItemWarrantyPeriod>(NonNegativeMessage);

            return Result.Success(new InventoryItemWarrantyPeriod(PeriodType, duration));
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
