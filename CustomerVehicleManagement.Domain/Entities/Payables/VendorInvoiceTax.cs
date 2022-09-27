using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoiceTax : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly string InvalidTaxIdMessage = $"Value exceeds allowed value range.";
        public static readonly double MinimumValue = 0.0;
        public static readonly string MinimumValueMessage = $"Amount cannot be negative.";

        public SalesTax SalesTax { get; private set; }
        public double Amount { get; private set; }
        public int TaxId { get; private set; }

        private VendorInvoiceTax(SalesTax salesTax, double amount, int taxId)
        {
            if (salesTax is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if(amount < 0.0)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            if (taxId >= int.MaxValue || taxId < 0)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            SalesTax = salesTax;
            Amount = amount;
            TaxId = taxId;
        }

        public static Result<VendorInvoiceTax> Create(SalesTax salesTax, double amount, int taxId)
        {
            if (salesTax is null)
                return Result.Failure<VendorInvoiceTax>(RequiredMessage);

            if (amount < MinimumValue)
                return Result.Failure<VendorInvoiceTax>(MinimumValueMessage);

            if (taxId >= int.MaxValue || taxId < 0)
                return Result.Failure<VendorInvoiceTax>(InvalidTaxIdMessage);

            return Result.Success(new VendorInvoiceTax(salesTax, amount, taxId));
        }

        public Result<SalesTax> SetSalesTax(SalesTax salesTax)
        {
            if (salesTax is null)
                return Result.Failure<SalesTax>(RequiredMessage);

            return Result.Success(SalesTax = salesTax);
        }

        public Result<double> SetAmount(double amount)
        {
            if (amount < MinimumValue)
                return Result.Failure<double>(MinimumValueMessage);

            return Result.Success(Amount = amount);
        }

        public Result<int> SetTaxId(int taxId)
        {
            if (taxId >= int.MaxValue || taxId < 0)
                return Result.Failure<int>(InvalidTaxIdMessage);

            return Result.Success(TaxId = taxId);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected VendorInvoiceTax() { }

        #endregion
    }
}
