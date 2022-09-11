using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Taxes
{
    public class SalesTax : Entity
    {
        public static readonly int DescriptionMaximumLength = 10000;
        public static readonly string DescriptionMaximumLengthMessage = $"Description cannot be over {DescriptionMaximumLength} characters in length.";
        public static readonly string InvalidMessage = $"Invalid value(s). Please be sure all entries are valid";
        public static readonly int TaxIdNumberMinumumLength = 1;
        public static readonly int TaxIdNumberMaximumLength = 255;
        public static readonly string TaxIdNumberInvalidLengthMessage = $"Tax Id Number must be between {TaxIdNumberMinumumLength} and {TaxIdNumberMaximumLength} characters in length.";
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

        private IList<ExciseFee> exciseFees;
        public IReadOnlyList<ExciseFee> ExciseFees => exciseFees.ToList();

        private SalesTax(
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
                throw new ArgumentOutOfRangeException(RequiredMessage);

            description = (description ?? string.Empty).Trim();

            if (description.Length > DescriptionMaximumLength)
                throw new ArgumentOutOfRangeException(DescriptionMaximumLengthMessage);

            if (!Enum.IsDefined(typeof(SalesTaxType), taxType))
                throw new ArgumentOutOfRangeException(nameof(taxType));

            if (order < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            Description = description;
            TaxType = taxType;
            Order = order;

            taxIdNumber = (taxIdNumber ?? string.Empty).Trim();

            if (taxIdNumber.Length > TaxIdNumberMaximumLength || taxIdNumber.Length < TaxIdNumberMinumumLength)
                throw new ArgumentOutOfRangeException(TaxIdNumberInvalidLengthMessage);

            if (partTaxRate < MinimumValue || laborTaxRate < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            TaxIdNumber = taxIdNumber;
            PartTaxRate = partTaxRate;
            LaborTaxRate = laborTaxRate;

            if (isAppliedByDefault.HasValue)
                IsAppliedByDefault = isAppliedByDefault.Value;

            if (isTaxable.HasValue)
                IsTaxable = isTaxable.Value;

            if (exciseFees is not null)
                foreach (var fee in exciseFees)
                    AddExciseFee(fee);
        }

        // TODO: Make AddExciseFee signature honest: replace void return
        // type with Result<ExciseFee>; replace exception with Result.Failure
        private void AddExciseFee(ExciseFee fee)
        {
            if (fee is null)
                throw new ArgumentNullException(RequiredMessage);

            exciseFees.Add(fee);
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

            taxIdNumber = (taxIdNumber ?? string.Empty).Trim();

            if (taxIdNumber.Length > TaxIdNumberMaximumLength || taxIdNumber.Length < TaxIdNumberMinumumLength)
                return Result.Failure<SalesTax>(TaxIdNumberInvalidLengthMessage);

            if (partTaxRate < MinimumValue || laborTaxRate < MinimumValue)
                return Result.Failure<SalesTax>(MinimumValueMessage);

            return Result.Success(new SalesTax(
                description,
                taxType,
                order,
                taxIdNumber,
                partTaxRate,
                laborTaxRate,
                exciseFees,
                isAppliedByDefault,
                isTaxable
                ));
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

            if (taxIdNumber.Length > TaxIdNumberMaximumLength || taxIdNumber.Length < TaxIdNumberMinumumLength)
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
