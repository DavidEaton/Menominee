using CSharpFunctionalExtensions;
using Menominee.Domain.Enums;
using Menominee.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Menominee.Domain.Entities.RepairOrders
{
    public class DiscountAmount : AppValueObject
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly double MinimumAmount = 0;
        public static readonly string InvalidAmountMessage = $"Invalid Amount.";
        public ItemDiscountType Type { get; private set; }
        public double Amount { get; private set; }

        private DiscountAmount(ItemDiscountType type, double amount)
        {
            Type = type;
            Amount = amount;
        }

        public static Result<DiscountAmount> Create(ItemDiscountType type, double amount)
        {
            if (!Enum.IsDefined(typeof(ItemDiscountType), type))
                return Result.Failure<DiscountAmount>(RequiredMessage);

            switch (type)
            {
                // TODO: Should we be resetting the user's input here?
                // Maybe a warning instead...
                case ItemDiscountType.None or ItemDiscountType.Predefined:

                    amount = 0;
                    break;

                case ItemDiscountType.Percent or ItemDiscountType.Dollar:

                    if (amount < MinimumAmount)
                        return Result.Failure<DiscountAmount>(InvalidAmountMessage);

                    break;

                default:
                    return Result.Failure<DiscountAmount>(InvalidAmountMessage);
            }

            return Result.Success(new DiscountAmount(type, amount));
        }

        public Result<DiscountAmount> NewDiscountAmount(double amount)
        {
            if (amount < MinimumAmount)
                return Result.Failure<DiscountAmount>(InvalidAmountMessage);

            return new DiscountAmount(Type, amount);
        }

        public Result<DiscountAmount> NewDiscountType(ItemDiscountType type)
        {
            if (!Enum.IsDefined(typeof(ItemDiscountType), type))
                return Result.Failure<DiscountAmount>(RequiredMessage);

            return new DiscountAmount(type, Amount);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
            yield return Amount;
        }

        #region ORM

        // EF requires a parameterless constructor
        protected DiscountAmount() { }

        #endregion    
    }
}
