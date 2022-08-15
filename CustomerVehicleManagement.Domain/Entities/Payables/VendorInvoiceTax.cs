using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoiceTax : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int TaxNameMaximumLength = 255;
        public static readonly string TaxNameMaximumLengthMessage = $"Tax Name cannot be over {TaxNameMaximumLength} characters in length.";
        public static readonly double MinimumValue = 0;
        public static readonly string MinimumValueMessage = $"Amount cannot be negative.";

        public SalesTax SalesTax { get; private set; }
        public int Order { get; private set; }
        public int TaxId { get; private set; }
        public string TaxName { get; private set; }
        public double Amount { get; private set; }

        private VendorInvoiceTax(
            SalesTax salesTax,
            int order,
            string taxName,
            double amount)
        {
            SalesTax = salesTax;
            Order = order;
            TaxName = taxName;
            Amount = amount;
        }

        public static Result<VendorInvoiceTax> Create(
            SalesTax salesTax,
            int order,
            string taxName,
            double amount)
        {
            if (salesTax is null)
                return Result.Failure<VendorInvoiceTax>(RequiredMessage);

            taxName = (taxName ?? string.Empty).Trim();

            if (taxName.Length > TaxNameMaximumLength)
                return Result.Failure<VendorInvoiceTax>(TaxNameMaximumLengthMessage);

            if (amount < MinimumValue)
                return Result.Failure<VendorInvoiceTax>(MinimumValueMessage);

            return Result.Success(new VendorInvoiceTax(salesTax, order, taxName, amount));
        }
        #region ORM

        // EF requires an empty constructor
        public VendorInvoiceTax() { }

        #endregion
    }
}
