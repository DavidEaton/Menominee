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

        public SalesTax SalesTax { get; private set; }
        public int TaxId { get; private set; }

        private VendorInvoiceTax(SalesTax salesTax, int taxId)
        {
            if (salesTax is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (taxId >= int.MaxValue || taxId < 0)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            SalesTax = salesTax;
            TaxId = taxId;
        }

        public static Result<VendorInvoiceTax> Create(SalesTax salesTax, int taxId)
        {
            if (salesTax is null)
                return Result.Failure<VendorInvoiceTax>(RequiredMessage);

            if (taxId >= int.MaxValue || taxId < 0)
                return Result.Failure<VendorInvoiceTax>(InvalidTaxIdMessage);

            return Result.Success(new VendorInvoiceTax(salesTax, taxId));
        }

        public Result<SalesTax> SetSalesTax(SalesTax salesTax)
        {
            if (salesTax is null)
                return Result.Failure<SalesTax>(RequiredMessage);

            return Result.Success(SalesTax = salesTax);
        }

        public Result<int> SetTaxId(int taxId)
        {
            if (taxId >= int.MaxValue || taxId < 0)
                return Result.Failure<int>(InvalidTaxIdMessage);

            return Result.Success(TaxId = taxId);
        }

        #region ORM

        // EF requires an empty constructor
        public VendorInvoiceTax() { }

        #endregion
    }
}
