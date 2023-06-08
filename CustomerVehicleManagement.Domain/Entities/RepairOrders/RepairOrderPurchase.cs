using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities.Payables;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    // TODO: DDD: Rename this class to ServiceLinePurchase
    public class RepairOrderPurchase : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidLengthMessage = $"Each item must be between {MinimumLength} and {MaximumLength} characters";
        public static readonly string DateInvalidMessage = $"Date cannot be in the future.";

        public Vendor Vendor { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public string PONumber { get; private set; }
        public string VendorInvoiceNumber { get; private set; }
        public string VendorPartNumber { get; private set; }
        private RepairOrderPurchase(Vendor vendor, DateTime purchaseDate, string pONumber, string vendorInvoiceNumber, string vendorPartNumber)
        {
            Vendor = vendor;
            PurchaseDate = purchaseDate;
            PONumber = pONumber;
            VendorInvoiceNumber = vendorInvoiceNumber;
            VendorPartNumber = vendorPartNumber;
        }

        public static Result<RepairOrderPurchase> Create(Vendor vendor, DateTime purchaseDate, string pONumber, string vendorInvoiceNumber, string vendorPartNumber)
        {
            if (vendor is null)
                return Result.Failure<RepairOrderPurchase>(RequiredMessage);

            if (purchaseDate > DateTime.Today)
                return Result.Failure<RepairOrderPurchase>(DateInvalidMessage);

            pONumber = (pONumber ?? string.Empty).Trim();
            vendorInvoiceNumber = (vendorInvoiceNumber ?? string.Empty).Trim();
            vendorPartNumber = (vendorPartNumber ?? string.Empty).Trim();

            if (pONumber.Length > MaximumLength
             || pONumber.Length < MinimumLength
             || vendorInvoiceNumber.Length > MaximumLength
             || vendorInvoiceNumber.Length < MinimumLength
             || vendorPartNumber.Length > MaximumLength
             || vendorPartNumber.Length < MinimumLength)
                return Result.Failure<RepairOrderPurchase>(InvalidLengthMessage);

            return Result.Success(new RepairOrderPurchase(vendor, purchaseDate, pONumber, vendorInvoiceNumber, vendorPartNumber));
        }

        public Result<Vendor> SetVendor(Vendor vendor)
        {
            if (vendor is null)
                return Result.Failure<Vendor>(RequiredMessage);

            return Result.Success(Vendor = vendor);
        }

        public Result<DateTime> SetPurchaseDate(DateTime date)
        {
            if (date > DateTime.Today)
                return Result.Failure<DateTime>(DateInvalidMessage);

            return Result.Success(PurchaseDate = date);
        }

        public Result<string> SetPONumber(string pONumber)
        {
            pONumber = (pONumber ?? string.Empty).Trim();

            if (pONumber.Length > MaximumLength || pONumber.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(PONumber = pONumber);
        }

        public Result<string> SetVendorInvoiceNumber(string pONumber)
        {
            pONumber = (pONumber ?? string.Empty).Trim();

            if (pONumber.Length > MaximumLength || pONumber.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(VendorInvoiceNumber = pONumber);
        }

        public Result<string> SetVendorPartNumber(string partNumber)
        {
            partNumber = (partNumber ?? string.Empty).Trim();

            if (partNumber.Length > MaximumLength || partNumber.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(VendorPartNumber = partNumber);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected RepairOrderPurchase() { }

        #endregion
    }
}
