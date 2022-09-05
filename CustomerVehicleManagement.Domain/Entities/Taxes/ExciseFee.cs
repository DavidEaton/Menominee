using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Taxes
{
    public class ExciseFee : Entity
    {
        public static readonly int DescriptionMaximumLength = 10000;
        public static readonly string DescriptionMaximumLengthMessage = $"Description cannot be over {DescriptionMaximumLength} characters in length.";
        public static readonly string InvalidMessage = $"Invalid value(s). Please be sure all entries are valid";
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly double MinimumValue = 0;
        public static readonly string MinimumValueMessage = $"Value(s) cannot be negative.";

        public string Description { get; private set; }
        public ExciseFeeType FeeType { get; private set; }
        public double Amount { get; private set; }

        private ExciseFee(string description, ExciseFeeType feeType, double amount)
        {
            if (description is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            description = (description ?? string.Empty).Trim();

            if (description.Length > DescriptionMaximumLength)
                throw new ArgumentOutOfRangeException(DescriptionMaximumLengthMessage);

            if (!Enum.IsDefined(typeof(ExciseFeeType), feeType))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (amount < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);
            
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

            if (amount < MinimumValue)
                return Result.Failure<ExciseFee>(MinimumValueMessage);

            description = (description ?? string.Empty).Trim();

            if (description.Length > DescriptionMaximumLength)
                return Result.Failure<ExciseFee>
                    (DescriptionMaximumLengthMessage);

            return Result.Success(
                new ExciseFee(description, feeType, amount));
        }

        public void SetDescription(string description)
        {
            if (description is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            description = (description ?? string.Empty).Trim();

            if (description.Length > DescriptionMaximumLength)
                throw new ArgumentOutOfRangeException(DescriptionMaximumLengthMessage);

            Description = description;
        }

        public void SetFeeType(ExciseFeeType feeType)
        {
            if (Enum.IsDefined(typeof(ExciseFeeType), feeType))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            FeeType = feeType;
        }

        public void SetAmount(double amount)
        {
            if (amount < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            Amount = amount;
        }

        #region ORM

        // EF requires an empty constructor
        public ExciseFee() { }

        #endregion
    }
}
