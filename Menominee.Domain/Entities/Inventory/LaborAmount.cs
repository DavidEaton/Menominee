using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace Menominee.Domain.Entities.Inventory
{
    public class LaborAmount : AppValueObject
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly double MinimumAmount = 0;
        public static readonly string InvalidAmountMessage = $"Invalid Amount.";

        public ItemLaborType Type { get; private set; }
        public double Amount { get; private set; } //> 0 if LaborType isn't None, otherwise is a dollar amount or 1/10th of an hour depending on type

        internal LaborAmount(ItemLaborType type, double amount)
        {
            Type = type;
            Amount = amount;
        }

        public static Result<LaborAmount> Create(ItemLaborType type, double amount)
        {
            if (!Enum.IsDefined(typeof(ItemLaborType), type))
                return Result.Failure<LaborAmount>(RequiredMessage);

            switch (type)
            {
                // TODO: Should we be resetting the user's input here?
                // Maybe a warning instead...
                case ItemLaborType.None:

                    amount = 0;
                    break;

                case ItemLaborType.Flat or ItemLaborType.Time:

                    if (amount < MinimumAmount)
                        return Result.Failure<LaborAmount>(InvalidAmountMessage);

                    break;

                default:
                    return Result.Failure<LaborAmount>("Invalid Labor Type");
            }

            return Result.Success(new LaborAmount(type, amount));
        }

        public Result<LaborAmount> NewPayType(ItemLaborType type)
        {
            return new LaborAmount(type, Amount);
        }

        public Result<LaborAmount> NewAmount(double newAmount)
        {
            return new LaborAmount(Type, newAmount);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
            yield return Amount;
        }

        #region ORM

        // EF requires a parameterless constructor
        protected LaborAmount() { }

        #endregion    
    }
}
