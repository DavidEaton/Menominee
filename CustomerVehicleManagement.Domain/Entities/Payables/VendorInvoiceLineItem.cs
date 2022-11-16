using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoiceLineItem : Entity
    {
        public static readonly string RequiredMessage = "Please include all required items.";
        public static readonly double MinimumValue = 0;
        public static readonly string MinimumValueMessage = $"Value(s) must be greater than {MinimumValue}.";
        public static readonly int PONumberMaximumLength = 40;
        public static readonly string PONumberMaximumLengthMessage = $"PO Number cannot be over {PONumberMaximumLength} characters in length.";
        public static readonly string TransactionDateInvalidMessage = "Transaction Date cannot be in the future.";

        public VendorInvoiceLineItemType Type { get; private set; } // required
        public VendorInvoiceItem Item { get; private set; } // required
        public double Quantity { get; private set; } // required, must be > 0
        public double Cost { get; private set; } // >= 0, aka nonnegative
        public double Core { get; private set; } // >= 0, aka nonnegative
        public string PONumber { get; private set; } // optional, 40 length
        public DateTime? TransactionDate { get; set; } // cannot be in the future

        private VendorInvoiceLineItem(
            VendorInvoiceLineItemType type,
            VendorInvoiceItem item,
            double quantity,
            double cost,
            double core,
            string poNumber = null,
            DateTime? transactionDate = null)
        {
            if (!Enum.IsDefined(typeof(VendorInvoiceLineItemType), type))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (item is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (quantity <= MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            if (cost < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            if (core < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            poNumber = (poNumber ?? string.Empty).Trim();

            if (poNumber is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (poNumber.Length > PONumberMaximumLength)
                throw new ArgumentOutOfRangeException(PONumberMaximumLengthMessage);

            if (transactionDate.HasValue && transactionDate.Value > DateTime.Today)
                throw new ArgumentOutOfRangeException(TransactionDateInvalidMessage);

            Type = type;
            Item = item;
            Quantity = quantity;
            Cost = cost;
            Core = core;
            PONumber = poNumber;
            TransactionDate = transactionDate;
        }

        public static Result<VendorInvoiceLineItem> Create(
            VendorInvoiceLineItemType type,
            VendorInvoiceItem item,
            double quantity,
            double cost,
            double core,
            string poNumber = null,
            DateTime? transactionDate = null)
        {
            if (!Enum.IsDefined(typeof(VendorInvoiceLineItemType), type))
                return Result.Failure<VendorInvoiceLineItem>(RequiredMessage);

            if (item is null)
                return Result.Failure<VendorInvoiceLineItem>(RequiredMessage);

            if (quantity <= MinimumValue)
                return Result.Failure<VendorInvoiceLineItem>(MinimumValueMessage);

            if (cost < MinimumValue || core < MinimumValue)
                return Result.Failure<VendorInvoiceLineItem>(MinimumValueMessage);

            poNumber = (poNumber ?? string.Empty).Trim();

            if (poNumber.Length > PONumberMaximumLength)
                return Result.Failure<VendorInvoiceLineItem>(PONumberMaximumLengthMessage);

            if (transactionDate.HasValue && transactionDate.Value > DateTime.Today)
                return Result.Failure<VendorInvoiceLineItem>(TransactionDateInvalidMessage);

            return Result.Success(new VendorInvoiceLineItem(
                type, item, quantity, cost, core, poNumber, transactionDate));
        }

        public Result<VendorInvoiceLineItemType> SetType(VendorInvoiceLineItemType type)
        {
            if (!Enum.IsDefined(typeof(VendorInvoiceLineItemType), type))
                return Result.Failure<VendorInvoiceLineItemType>(RequiredMessage);

            return Result.Success(Type = type);
        }

        public Result<VendorInvoiceItem> SetItem(VendorInvoiceItem item)
        {
            if (item is null)
                return Result.Failure<VendorInvoiceItem>(RequiredMessage);

            return Result.Success(Item = item);
        }

        public Result<double> SetQuantity(double quantity)
        {
            if (quantity <= MinimumValue)
                return Result.Failure<double>(MinimumValueMessage);

            return Result.Success(Quantity = quantity);
        }

        public Result<double> SetCost(double cost)
        {
            if (cost < MinimumValue)
                return Result.Failure<double>(MinimumValueMessage);

            return Result.Success(Cost = cost);
        }

        public Result<double> SetCore(double core)
        {
            if (core < MinimumValue)
                return Result.Failure<double>(MinimumValueMessage);

            return Result.Success(Core = core);
        }

        public Result<string> SetPONumber(string poNumber)
        {
            if (string.IsNullOrWhiteSpace(poNumber))
                return Result.Failure<string>(RequiredMessage);

            poNumber = (poNumber ?? string.Empty).Trim();

            if (poNumber.Length > PONumberMaximumLength)
                return Result.Failure<string>(PONumberMaximumLengthMessage);

            return Result.Success(PONumber = poNumber);
        }

        public void ClearPONumber() => PONumber = string.Empty;

        public Result<DateTime?> SetTransactionDate(DateTime? transactionDate)
        {
            if (transactionDate is null)
                return Result.Failure<DateTime?>(TransactionDateInvalidMessage);

            if (transactionDate.HasValue && transactionDate.Value > DateTime.Today)
                return Result.Failure<DateTime?>(TransactionDateInvalidMessage);

            return Result.Success(TransactionDate = transactionDate);
        }

        public void ClearTransactionDate() => TransactionDate = null;

        #region ORM

        // EF requires a parameterless constructor
        protected VendorInvoiceLineItem() { }

        #endregion
    }
}
