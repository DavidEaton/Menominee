using CSharpFunctionalExtensions;
using Menominee.Common.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class PartTax : AppValueObject
    {
        public double Rate { get; private set; }
        public double Amount { get; private set; }

        private PartTax(double rate, double amount)
        {
            Rate = rate;
            Amount = amount;
        }

        public static Result<PartTax> Create(double rate, double amount)
        {
            return Result.Success(new PartTax(rate, amount));
        }

        public Result<PartTax> NewRate(double rate)
        {
            return Result.Success(new PartTax(rate, Amount));
        }

        public Result<PartTax> NewAmount(double amount)
        {
            return Result.Success(new PartTax(Rate, amount));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Rate;
            yield return Amount;
        }

        #region ORM

        // EF requires an empty constructor
        protected PartTax() { }

        #endregion
    }
}
