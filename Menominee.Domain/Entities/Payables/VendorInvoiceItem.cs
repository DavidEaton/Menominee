using CSharpFunctionalExtensions;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.ValueObjects;
using System.Collections.Generic;

namespace Menominee.Domain.Entities.Payables
{
    public class VendorInvoiceItem : AppValueObject
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MaximumLength = 255;
        public static readonly int MinimumLength = 1;
        public static readonly string InvalidLengthMessage = $"Must be within {MinimumLength} to {MaximumLength} character(s) in length.";

        public string PartNumber { get; private set; } // required, 1..255 or maybe less. Really just a string?
        public string Description { get; private set; } // required, 1.255
        public Manufacturer Manufacturer { get; private set; } // not required for every item type
        public SaleCode SaleCode { get; private set; } // not required

        private VendorInvoiceItem(
            string partNumber,
            string description,
            Manufacturer manufacturer = null,
            SaleCode saleCode = null)
        {
            PartNumber = partNumber;
            Description = description;
            Manufacturer = manufacturer;
            SaleCode = saleCode;
        }

        public static Result<VendorInvoiceItem> Create(
            string partNumber,
            string description,
            Manufacturer manufacturer = null,
            SaleCode saleCode = null)
        {
            partNumber = (partNumber ?? string.Empty).Trim();
            description = (description ?? string.Empty).Trim();

            if (partNumber.Length > MaximumLength || description.Length > MaximumLength)
                return Result.Failure<VendorInvoiceItem>(InvalidLengthMessage);

            if (partNumber.Length < MinimumLength || description.Length < MinimumLength)
                return Result.Failure<VendorInvoiceItem>(InvalidLengthMessage);

            return Result.Success(new VendorInvoiceItem(
                partNumber, description, manufacturer, saleCode));
        }

        public Result<VendorInvoiceItem> NewPartNumber(string partNumber)
        {
            partNumber = (partNumber ?? string.Empty).Trim();

            if (partNumber.Length > MaximumLength || partNumber.Length < MinimumLength)
                return Result.Failure<VendorInvoiceItem>(InvalidLengthMessage);

            return Result.Success(new VendorInvoiceItem(partNumber, Description, Manufacturer, SaleCode));
        }

        public Result<VendorInvoiceItem> NewDescription(string description)
        {
            description = (description ?? string.Empty).Trim();

            if (description.Length < MinimumLength || description.Length > MaximumLength)
                return Result.Failure<VendorInvoiceItem>(InvalidLengthMessage);

            return Result.Success(new VendorInvoiceItem(PartNumber, description, Manufacturer, SaleCode));
        }

        public Result<VendorInvoiceItem> NewManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer is null)
                return Result.Failure<VendorInvoiceItem>(RequiredMessage);

            return Result.Success(new VendorInvoiceItem(PartNumber, Description, manufacturer, SaleCode));
        }

        public Result<VendorInvoiceItem> ClearManufacturer()
        {
            return Result.Success(new VendorInvoiceItem(PartNumber, Description, null, SaleCode));
        }

        public Result<VendorInvoiceItem> NewSaleCode(SaleCode saleCode)
        {
            if (saleCode is null)
                return Result.Failure<VendorInvoiceItem>(RequiredMessage);

            return Result.Success(new VendorInvoiceItem(PartNumber, Description, Manufacturer, saleCode));
        }

        public Result<VendorInvoiceItem> ClearSaleCode()
        {
            return Result.Success(new VendorInvoiceItem(PartNumber, Description, Manufacturer, null));

        }

        #region ORM

        // EF requires a parameterless constructor
        protected VendorInvoiceItem() { }

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
