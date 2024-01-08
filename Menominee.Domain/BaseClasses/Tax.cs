using CSharpFunctionalExtensions;
using Menominee.Domain.ValueObjects;
using System.Collections.Generic;

namespace Menominee.Domain.BaseClasses
{
    public abstract class Tax<T> : AppValueObject where T : Tax<T>, new()
    {
        public static readonly double MinimumAmount = 0;
        public static readonly string InvalidMessage = $"Invalid ";

        public double Rate { get; private set; }
        public double Amount { get; private set; }


        public static Result<T> Create(double rate, double amount)
        {
            return
                rate < MinimumAmount
                ? Result.Failure<T>($"{InvalidMessage} rate and/or amount.")
                : Result.Success(new T { Rate = rate, Amount = amount });
        }

        public Result<T> NewRate(double rate)
        {
            return
                rate < MinimumAmount
                ? Result.Failure<T>($"{InvalidMessage} rate.")
                : Result.Success(new T { Rate = rate, Amount = Amount });
        }

        public Result<T> NewAmount(double amount)
        {
            return Result.Success(new T { Rate = Rate, Amount = amount });
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Rate;
            yield return Amount;
        }

        #region ORM

        // EF requires a parameterless constructor

        protected Tax() { }

        #endregion
    }
}
