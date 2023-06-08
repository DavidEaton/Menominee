using CSharpFunctionalExtensions;
using Menominee.Common.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class LaborTax : AppValueObject
    {
        public static readonly double MinimumAmount = 0;
        public static readonly string InvalidAmountMessage = $"Invalid Amount.";

        public double Rate { get; private set; }
        public double Amount { get; private set; }
        private LaborTax(double rate, double amount)
        {
            Rate = rate;
            Amount = amount;
        }

        public static Result<LaborTax> Create(double rate, double amount)
        {
            if (amount < MinimumAmount || rate < MinimumAmount)
                return Result.Failure<LaborTax>(InvalidAmountMessage);

            return Result.Success(new LaborTax(rate, amount));
        }

        public Result<LaborTax> NewRate(double rate)
        {
            if (rate < MinimumAmount)
                return Result.Failure<LaborTax>(InvalidAmountMessage);

            return Result.Success(new LaborTax(rate, Amount));
        }

        public Result<LaborTax> NewAmount(double amount)
        {
            if (amount < MinimumAmount)
                return Result.Failure<LaborTax>(InvalidAmountMessage);

            return Result.Success(new LaborTax(Rate, amount));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Rate;
            yield return Amount;
        }

        #region ORM

        // EF requires an empty constructor
        protected LaborTax() { }

        #endregion
    }
}
