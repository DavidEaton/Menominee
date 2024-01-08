using CSharpFunctionalExtensions;
using Menominee.Domain.Enums;
using System;
using Entity = Menominee.Domain.BaseClasses.Entity;

namespace Menominee.Domain.Entities.Taxes
{
    public class ExciseFee : Entity
    {
        public static readonly int DescriptionMaximumLength = 10000;
        public static readonly string DescriptionMaximumLengthMessage = $"Description cannot be over {DescriptionMaximumLength} characters in length.";
        public static readonly string InvalidMessage = $"Invalid value(s). Please be sure all entries are valid";
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly double MinimumValue = 0;
        public static readonly double MaximumValue = 99999.00;
        public static readonly string InvalidValueMessage = $"Value(s) must be within {MinimumValue} and {MaximumValue}.";

        public string Description { get; private set; }
        public ExciseFeeType FeeType { get; private set; }
        public double Amount { get; private set; }

        private ExciseFee(string description, ExciseFeeType feeType, double amount)
        {
            Description = description;
            FeeType = feeType;
            Amount = amount;
        }

        public static Result<ExciseFee> Create(
            string description,
            ExciseFeeType feeType,
            double amount)
        {
            if (description is null)
                return Result.Failure<ExciseFee>(RequiredMessage);

            if (!Enum.IsDefined(typeof(ExciseFeeType), feeType))
                return Result.Failure<ExciseFee>(InvalidMessage);

            if (amount < MinimumValue || amount > MaximumValue)
                return Result.Failure<ExciseFee>(InvalidValueMessage);

            description = (description ?? string.Empty).Trim();

            if (description.Length > DescriptionMaximumLength)
                return Result.Failure<ExciseFee>(DescriptionMaximumLengthMessage);

            return Result.Success(
                new ExciseFee(description, feeType, amount));
        }

        public Result<string> SetDescription(string description)
        {
            if (description is null)
                return Result.Failure<string>(RequiredMessage);

            description = (description ?? string.Empty).Trim();

            if (description.Length > DescriptionMaximumLength)
                return Result.Failure<string>(DescriptionMaximumLengthMessage);

            return Result.Success(Description = description);
        }

        public Result<ExciseFeeType> SetFeeType(ExciseFeeType feeType)
        {
            if (!Enum.IsDefined(typeof(ExciseFeeType), feeType))
                return Result.Failure<ExciseFeeType>(InvalidMessage);

            return Result.Success(FeeType = feeType);
        }

        public Result<double> SetAmount(double amount)
        {
            if (amount < MinimumValue || amount > MaximumValue)
                return Result.Failure<double>(InvalidValueMessage);

            return Result.Success(Amount = amount);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected ExciseFee() { }

        #endregion
    }
}
