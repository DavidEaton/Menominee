using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoiceItem : ValueObject
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MaximumLength = 255;
        public static readonly string MaximumLengthMessage = $"Cannot be over {MaximumLength} characters in length.";

        public string PartNumber { get; private set; } // Really just a string?
        public Manufacturer Manufacturer { get; private set; }
        public string Description { get; private set; }
        public SaleCode SaleCode { get; private set; }

        private VendorInvoiceItem(
            string partNumber,
            Manufacturer manufacturer,
            string description,
            SaleCode saleCode)
        {
            PartNumber = partNumber;
            Manufacturer = manufacturer;
            Description = description;
            SaleCode = saleCode;
        }

        public static Result<VendorInvoiceItem> Create(
            string partNumber,
            Manufacturer manufacturer,
            string description,
            SaleCode saleCode)
        {
            partNumber = (partNumber ?? string.Empty).Trim();
            description = (description ?? string.Empty).Trim();

            if (partNumber.Length > MaximumLength || description.Length > MaximumLength)
                return Result.Failure<VendorInvoiceItem>(MaximumLengthMessage);

            if (manufacturer is null)
                return Result.Failure<VendorInvoiceItem>(RequiredMessage);

            if (saleCode is null)
                return Result.Failure<VendorInvoiceItem>(RequiredMessage);

            return Result.Success(new VendorInvoiceItem(
                partNumber, manufacturer, description, saleCode));
        }

        #region ORM

        // EF requires an empty constructor
        public VendorInvoiceItem() { }

        #endregion

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PartNumber;
            yield return Manufacturer;
            yield return Description;
            yield return SaleCode;
        }
    }
}
