using CSharpFunctionalExtensions;
using Menominee.Domain.Entities.Taxes;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities.Payables
{
    public class VendorInvoiceTax : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly string InvalidTaxIdMessage = $"Value exceeds allowed value range.";
        public static readonly double MinimumValue = 0.0;
        public static readonly string MinimumValueMessage = $"Amount cannot be negative.";

        public SalesTax SalesTax { get; private set; }
        public double Amount { get; private set; }

        private VendorInvoiceTax(SalesTax salesTax, double amount)
        {
            SalesTax = salesTax;
            Amount = amount;
        }

        public static Result<VendorInvoiceTax> Create(SalesTax salesTax, double amount)
        {
            if (salesTax is null)
                return Result.Failure<VendorInvoiceTax>(RequiredMessage);

            if (amount < MinimumValue)
                return Result.Failure<VendorInvoiceTax>(MinimumValueMessage);

            return Result.Success(new VendorInvoiceTax(salesTax, amount));
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

        #region ORM

        // EF requires a parameterless constructor
        protected VendorInvoiceTax() { }

        #endregion
    }
}
