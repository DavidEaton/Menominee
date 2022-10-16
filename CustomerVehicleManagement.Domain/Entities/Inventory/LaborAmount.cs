using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class LaborAmount : AppValueObject
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly double MinimumAmount = 0;
        public static readonly string InvalidAmountMessage = $"Invalid Amount.";

        public ItemLaborType PayType { get; private set; }
        public double Amount { get; private set; } //> 0 if LaborType isn't None, otherwise is a dollar amount or 1/10th of an hour depending on type

        protected LaborAmount(ItemLaborType payType, double amount)
        {
            if (!Enum.IsDefined(typeof(ItemLaborType), payType))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            switch (payType)
            {
                case ItemLaborType.None:
                    amount = 0;
                    break;

                case ItemLaborType.Flat:
                case ItemLaborType.Time:

                    if (amount < MinimumAmount)
                        throw new ArgumentOutOfRangeException(InvalidAmountMessage);

                    break;

                default:
                    throw new ArgumentOutOfRangeException(RequiredMessage);
            }

            PayType = payType;
            Amount = amount;
        }

        public static Result<LaborAmount> Create(ItemLaborType payType, double amount)
        {
            if (!Enum.IsDefined(typeof(ItemLaborType), payType))
                return Result.Failure<LaborAmount>(RequiredMessage);

            switch (payType)
            {
                case ItemLaborType.None:
                    amount = 0;
                    break;

                case ItemLaborType.Flat:
                case ItemLaborType.Time:

                    if (amount < MinimumAmount)
                        return Result.Failure<LaborAmount>(InvalidAmountMessage);

                    break;

                default:
                    return Result.Failure<LaborAmount>("Invalid Labor Type");
            }

            return Result.Success(new LaborAmount(payType, amount));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected LaborAmount() { }

        #endregion    

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PayType;
            yield return Amount;
        }
    }
}
