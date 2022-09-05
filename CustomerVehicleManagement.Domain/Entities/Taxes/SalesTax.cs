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
        public List<ExciseFee> ExciseFees { get; private set; } = new List<ExciseFee>();

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

            SetExciseFees(exciseFees);
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

        public void SetDescription(string description)
        {
            if (description is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            description = (description ?? string.Empty).Trim();

            if (description.Length > DescriptionMaximumLength)
                throw new ArgumentOutOfRangeException(DescriptionMaximumLengthMessage);

            Description = description;
        }

        public void SetTaxType(SalesTaxType taxType)
        {
            if (!Enum.IsDefined(typeof(SalesTaxType), taxType))
                throw new ArgumentOutOfRangeException(nameof(taxType));

            TaxType = taxType;
        }

        public void SetOrder(int order)
        {
            if (order < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            Order = order;
        }

        public void SetTaxIdNumber(string taxIdNumber)
        {
            taxIdNumber = (taxIdNumber ?? string.Empty).Trim();

            if (taxIdNumber.Length > TaxIdNumberMaximumLength || taxIdNumber.Length < TaxIdNumberMinumumLength)
                throw new ArgumentOutOfRangeException(TaxIdNumberInvalidLengthMessage);

            TaxIdNumber = taxIdNumber;
        }

        public void SetPartTaxRate(double partTaxRate)
        {
            if (partTaxRate < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            PartTaxRate = partTaxRate;
        }

        public void SetLaborTaxRate(double laborTaxRate)
        {
            if (laborTaxRate < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            LaborTaxRate = laborTaxRate;
        }

        public void SetIsTaxable(bool? isTaxable)
        {
            if (isTaxable.HasValue)
                IsTaxable = isTaxable.Value;
        }

        public void SetIsAppliedByDefault(bool? isAppliedByDefault)
        {
            if (isAppliedByDefault.HasValue)
                IsAppliedByDefault = isAppliedByDefault.Value;
        }


        public void SetExciseFees(List<ExciseFee> exciseFees)
        {
            if (exciseFees is null || exciseFees?.Count == 0)
                ExciseFees = exciseFees;

            if (exciseFees?.Count > 0)
            {
                // Remove not found items (items that caller removed)
                foreach (var fee in exciseFees)
                {
                    if (!ExciseFees.Any(x => x.Id == fee.Id))
                        exciseFees.Remove(fee);
                }

                // Find and Update each
                foreach (var fee in exciseFees)
                {
                    var foundFee = ExciseFees.FirstOrDefault(f => f.Id == fee.Id);
                    foundFee.SetDescription(fee.Description);
                    foundFee.SetFeeType(fee.FeeType);
                    foundFee.SetAmount(fee.Amount);
                }

                // Validate the collection

            }
        }




        #region ORM

        // EF requires an empty constructor
        public SalesTax() { }

        #endregion  
    }
}
