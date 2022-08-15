using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Taxes
{
    public class SalesTax : Entity
    {
        public static readonly int DescriptionMaximumLength = 10000;
        public static readonly string DescriptionMaximumLengthMessage = $"Description cannot be over {DescriptionMaximumLength} characters in length.";
        public static readonly string InvalidMessage = $"Invalid value(s). Please be sure all entries are valid";
        public static readonly int TaxIdNumberMaximumLength = 255;
        public static readonly string TaxIdNumberMaximumLengthMessage = $"Tax Id Number cannot be over {TaxIdNumberMaximumLength} characters in length.";
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
        public List<SalesTaxTaxableExciseFee> TaxedExciseFees { get; private set; } = new List<SalesTaxTaxableExciseFee>();

        private SalesTax(
            string description,
            SalesTaxType taxType,
            int order,
            bool? isAppliedByDefault,
            bool? isTaxable,
            string taxIdNumber,
            double partTaxRate,
            double laborTaxRate)
        {
            Description = description;
            TaxType = taxType;
            Order = order;
            IsAppliedByDefault = isAppliedByDefault;
            IsTaxable = isTaxable;
            TaxIdNumber = taxIdNumber;
            PartTaxRate = partTaxRate;
            LaborTaxRate = laborTaxRate;
        }

        public static Result<SalesTax> Create(
            string description,
            SalesTaxType taxType,
            int order,
            bool? isAppliedByDefault,
            bool? isTaxable,
            string taxIdNumber,
            double partTaxRate,
            double laborTaxRate)
        {
            if (description is null)
                return Result.Failure<SalesTax>(RequiredMessage);

            if (!Enum.IsDefined(typeof(SalesTaxType), taxType))
                return Result.Failure<SalesTax>(InvalidMessage);

            description = (description ?? string.Empty).Trim();
            taxIdNumber = (taxIdNumber ?? string.Empty).Trim();

            if (description.Length > DescriptionMaximumLength)
                return Result.Failure<SalesTax>(DescriptionMaximumLengthMessage);

            if (taxIdNumber.Length > TaxIdNumberMaximumLength)
                return Result.Failure<SalesTax>(TaxIdNumberMaximumLengthMessage);

            if (partTaxRate < MinimumValue || laborTaxRate < MinimumValue)
                return Result.Failure<SalesTax>(MinimumValueMessage);

            return Result.Success(new SalesTax(
                description,
                taxType,
                order,
                isAppliedByDefault,
                isTaxable,
                taxIdNumber,
                partTaxRate,
                laborTaxRate));
        }

        // TODO - Should we have a list of taxable taxes too?

        #region ORM

        // EF requires an empty constructor
        public SalesTax() { }

        #endregion  
    }
}
