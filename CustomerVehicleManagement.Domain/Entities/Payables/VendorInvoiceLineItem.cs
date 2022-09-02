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

        public VendorInvoiceItemType Type { get; private set; } // required
        public VendorInvoiceItem Item { get; private set; } // required
        public double Quantity { get; private set; } // required, must be > 0
        public double Cost { get; private set; } // >= 0
        public double Core { get; private set; } // >= 0
        public string PONumber { get; private set; } // not required, 40 length
        public DateTime? TransactionDate { get; set; } // cannot be in the future

        private VendorInvoiceLineItem(
            VendorInvoiceItemType type,
            VendorInvoiceItem item,
            double quantity,
            double cost,
            double core,
            string poNumber = null,
            DateTime? transactionDate = null)
        {
            if (!Enum.IsDefined(typeof(VendorInvoiceItemType), type))
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
            VendorInvoiceItemType type,
            VendorInvoiceItem item,
            double quantity,
            double cost,
            double core,
            string poNumber = null,
            DateTime? transactionDate = null)
        {
            if (!Enum.IsDefined(typeof(VendorInvoiceItemType), type))
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

        public void SetType(VendorInvoiceItemType type)
        {
            if (!Enum.IsDefined(typeof(VendorInvoiceItemType), type))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            Type = type;
        }

        public void SetItem(VendorInvoiceItem item)
        {
            if (item is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            Item = item;
        }

        public void SetQuantity(double quantity)
        {
            if (quantity <= MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            Quantity = quantity;
        }

        public void SetCost(double cost)
        {
            if (cost < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            Cost = cost;
        }

        public void SetCore(double core)
        {
            if (core < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            Core = core;
        }

        public void SetPONumber(string poNumber)
        {
            if (string.IsNullOrWhiteSpace(poNumber))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            poNumber = (poNumber ?? string.Empty).Trim();

            if (poNumber.Length > PONumberMaximumLength)
                throw new ArgumentOutOfRangeException(PONumberMaximumLengthMessage);

            PONumber = poNumber;
        }

        public void ClearPONumber() => PONumber = string.Empty;

        public void SetTransactionDate(DateTime? transactionDate)
        {
            if (transactionDate is null)
                throw new ArgumentOutOfRangeException(TransactionDateInvalidMessage);
            
            if (transactionDate.HasValue && transactionDate.Value > DateTime.Today)
                throw new ArgumentOutOfRangeException(TransactionDateInvalidMessage);

            TransactionDate = transactionDate;
        }

        public void ClearTransactionDate() => TransactionDate = null;

        #region ORM

        // EF requires an empty constructor
        public VendorInvoiceLineItem() { }

        #endregion
    }
}
