using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities.Taxes
{
    public class SalesTax : Entity
    {
        public static readonly int DescriptionMaximumLength = 10000;
        public static readonly string DescriptionMaximumLengthMessage = $"Description cannot be over {DescriptionMaximumLength} characters in length.";
        public static readonly string InvalidMessage = $"Invalid value(s). Please be sure all entries are valid";
        public static readonly int TaxIdNumberMaximumLength = 255;
        public static readonly string TaxIdNumberInvalidLengthMessage = $"Tax Id Number must be no more than {TaxIdNumberMaximumLength} characters in length.";
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly double MinimumValue = 0;
        public static readonly string MinimumValueMessage = $"Value(s) cannot be negative.";

        public string Description { get; private set; }
        public SalesTaxType TaxType { get; private set; }
        public int Order { get; private set; }
        public bool? IsAppliedByDefault { get; private set; }
        public bool? IsTaxable { get; private set; }
        public string TaxIdNumber { get; private set; }
        public double PartTaxRate { get; private set; }
        public double LaborTaxRate { get; private set; }

        private IList<ExciseFee> exciseFees = new List<ExciseFee>();
        public IReadOnlyList<ExciseFee> ExciseFees => exciseFees.ToList();

        private SalesTax(
            string description,
            SalesTaxType taxType,
            int order,
            double partTaxRate,
            double laborTaxRate,
            string taxIdNumber = null,
            List<ExciseFee> exciseFees = null,
            bool? isAppliedByDefault = null,
            bool? isTaxable = null)
        {
            Description = description;
            TaxType = taxType;
            Order = order;
            PartTaxRate = partTaxRate;
            LaborTaxRate = laborTaxRate;
            TaxIdNumber = taxIdNumber;
            this.exciseFees = exciseFees ?? new List<ExciseFee>();
            IsAppliedByDefault = isAppliedByDefault;
            IsTaxable = isTaxable;
        }

        public static Result<SalesTax> Create(
            string description,
            SalesTaxType taxType,
            int order,
            string taxIdNumber,
            double partTaxRate,
            double laborTaxRate,
            List<ExciseFee> exciseFees = null,
            bool? isAppliedByDefault = null,
            bool? isTaxable = null)
        {
            if (description is null)
                return Result.Failure<SalesTax>(RequiredMessage);

            description = (description ?? string.Empty).Trim();

            if (description.Length > DescriptionMaximumLength)
                return Result.Failure<SalesTax>(DescriptionMaximumLengthMessage);

            if (!Enum.IsDefined(typeof(SalesTaxType), taxType))
                return Result.Failure<SalesTax>(InvalidMessage);

            if (order < MinimumValue)
                return Result.Failure<SalesTax>(MinimumValueMessage);

            if (partTaxRate < MinimumValue || laborTaxRate < MinimumValue)
                return Result.Failure<SalesTax>(MinimumValueMessage);

            taxIdNumber = (taxIdNumber ?? string.Empty).Trim();

            if (taxIdNumber.Length > TaxIdNumberMaximumLength)
                return Result.Failure<SalesTax>(TaxIdNumberInvalidLengthMessage);

            return Result.Success(new SalesTax(
                description,
                taxType,
                order,
                partTaxRate,
                laborTaxRate,
                taxIdNumber,
                exciseFees,
                isAppliedByDefault,
                isTaxable
                ));
        }

        public Result<ExciseFee> AddExciseFee(ExciseFee fee)
        {
            if (fee is null)
                return Result.Failure<ExciseFee>(RequiredMessage);

            exciseFees ??= new List<ExciseFee>();

            // Can the next two lines be accomplished with one?
            exciseFees.Add(fee);
            return Result.Success(fee);
        }

        public Result<ExciseFee> RemoveExciseFee(ExciseFee fee)
        {
            if (fee is null)
                return Result.Failure<ExciseFee>(RequiredMessage);

            // Better to remove the ? opperator and add a null check?
            // Using the ? operatror seems like it might potentially
            // silently swallow a bug:
            exciseFees?.Remove(fee);

            return Result.Success(fee);
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

        public Result<SalesTaxType> SetTaxType(SalesTaxType taxType)
        {
            if (!Enum.IsDefined(typeof(SalesTaxType), taxType))
                return Result.Failure<SalesTaxType>(InvalidMessage);

            return Result.Success(TaxType = taxType);
        }

        public Result<int> SetOrder(int order)
        {
            if (order < MinimumValue)
                return Result.Failure<int>(MinimumValueMessage);

            return Result.Success(Order = order);
        }

        public Result<string> SetTaxIdNumber(string taxIdNumber)
        {
            taxIdNumber = (taxIdNumber ?? string.Empty).Trim();

            if (taxIdNumber.Length > TaxIdNumberMaximumLength)
                return Result.Failure<string>(TaxIdNumberInvalidLengthMessage);

            return Result.Success(TaxIdNumber = taxIdNumber);
        }

        public Result<double> SetPartTaxRate(double partTaxRate)
        {
            if (partTaxRate < MinimumValue)
                return Result.Failure<double>(MinimumValueMessage);

            return Result.Success(PartTaxRate = partTaxRate);
        }

        public Result<double> SetLaborTaxRate(double laborTaxRate)
        {
            if (laborTaxRate < MinimumValue)
                return Result.Failure<double>(MinimumValueMessage);

            return Result.Success(LaborTaxRate = laborTaxRate);
        }

        public Result<bool?> SetIsTaxable(bool? isTaxable)
        {
            return isTaxable.HasValue
                ? Result.Success(IsTaxable = isTaxable.Value)
                : Result.Success(IsTaxable = null);
        }

        public Result<bool?> SetIsAppliedByDefault(bool? isAppliedByDefault)
        {
            return isAppliedByDefault.HasValue
                ? Result.Success(IsAppliedByDefault = isAppliedByDefault.Value)
                : Result.Success(IsAppliedByDefault = null);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected SalesTax()
        {
            exciseFees = new List<ExciseFee>();
        }

        #endregion  
    }
}
