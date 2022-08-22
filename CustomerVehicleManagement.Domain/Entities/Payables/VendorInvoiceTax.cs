using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoiceTax : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";

        public SalesTax SalesTax { get; private set; }
        public int TaxId { get; private set; }

        private VendorInvoiceTax(SalesTax salesTax)
        {
            SalesTax = salesTax;
        }

        public static Result<VendorInvoiceTax> Create(SalesTax salesTax)
        {
            return salesTax is null
                ? Result.Failure<VendorInvoiceTax>(RequiredMessage)
                : Result.Success(new VendorInvoiceTax(salesTax));
        }
        #region ORM

        // EF requires an empty constructor
        public VendorInvoiceTax() { }

        #endregion
    }
}
