using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Payables
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
            partNumber = (partNumber ?? string.Empty).Trim();
            description = (description ?? string.Empty).Trim();

            if (partNumber.Length > MaximumLength || partNumber.Length < MinimumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            if (description.Length < MinimumLength || description.Length > MaximumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            PartNumber = partNumber;
            Description = description;

            if (manufacturer is not null)
                Manufacturer = manufacturer;

            if (saleCode is not null)
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

        public Result<string> SetPartNumber(string partNumber)
        {
            partNumber = (partNumber ?? string.Empty).Trim();

            if (partNumber.Length > MaximumLength || partNumber.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(PartNumber = partNumber);
        }

        public Result<string> SetDescription(string description)
        {
            description = (description ?? string.Empty).Trim();

            if (description.Length < MinimumLength || description.Length > MaximumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(Description = description);
        }

        public Result<Manufacturer> SetManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer is null)
                return Result.Failure<Manufacturer>(RequiredMessage);

            return Result.Success(Manufacturer = manufacturer);
        }

        public void ClearManufacturer() => Manufacturer = null;

        public Result<SaleCode> SetSaleCode(SaleCode saleCode)
        {
            if (saleCode is null)
                return Result.Failure<SaleCode>(RequiredMessage);

            return Result.Success(SaleCode = saleCode);
        }

        public void ClearSaleCode() => SaleCode = null;

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
