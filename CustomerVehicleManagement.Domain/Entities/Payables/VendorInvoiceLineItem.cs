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
        public static readonly string MinimumValueMessage = "Value(s) cannot be negative.";
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
            string poNumber,
            DateTime? transactionDate)
        {
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
            string poNumber,
            DateTime? transactionDate)
        {
            if (!Enum.IsDefined(typeof(VendorInvoiceItemType), type))
                return Result.Failure<VendorInvoiceLineItem>(RequiredMessage);

            if (item is null)
                return Result.Failure<VendorInvoiceLineItem>(RequiredMessage);

            if (quantity <= MinimumValue)
                return Result.Failure<VendorInvoiceLineItem>("Quantity must be greater than zero");

            if (cost < MinimumValue || core < MinimumValue)
                return Result.Failure<VendorInvoiceLineItem>(MinimumValueMessage);

            poNumber = (poNumber ?? string.Empty).Trim();

            if (poNumber.Length > PONumberMaximumLength)
                return Result.Failure<VendorInvoiceLineItem>(PONumberMaximumLengthMessage);

            return transactionDate.HasValue && transactionDate.Value > DateTime.Today
                ? Result.Failure<VendorInvoiceLineItem>(TransactionDateInvalidMessage)
                : Result.Success(
                new VendorInvoiceLineItem(type, item, quantity, cost, core, poNumber, transactionDate));
        }

        #region ORM

        // EF requires an empty constructor
        public VendorInvoiceLineItem() { }

        #endregion
    }
}
