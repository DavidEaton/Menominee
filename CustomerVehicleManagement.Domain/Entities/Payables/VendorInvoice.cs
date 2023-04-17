using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.BaseClasses;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoice : Entity
    {
        // We can make these messages more specific and/or define an Error class to send to client,
        // where we can decide exactly how to handle those errors and display them uniformly to 
        // user; keeping it simple here for now.

        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly string DateInvalidMessage = $"Date cannot be in the future.";
        public static readonly string InvoiceNumberMaximumLengthMessage = $"Invoice Number cannot be over {InvoiceNumberMaximumLength} characters in length.";
        public static readonly string MinimumValueMessage = $"Value(s) cannot be negative.";
        public static readonly double MinimumValue = 0;
        public static readonly int InvoiceNumberMaximumLength = 255;
        public static readonly string NonuniqueMessage = $"Invoice Number is already in use for this Vendor. Please enter a unique Vendor Invoice Number.";

        public Vendor Vendor { get; private set; }
        public VendorInvoiceStatus Status { get; private set; }
        public VendorInvoiceDocumentType DocumentType { get; private set; }

        // For vendor invoices, "posted" means it's all finished and can't be edited again (unless it's unposted). It may also mean it's been sent into an accounting integration. -Al
        public DateTime? DatePosted { get; private set; }

        // Vendor invoice date is whatever the invoice from the vendor says it is. We date-stamp it when it's created by defaulting Date to the current date on creation, but user can select a date in the past too so it matches. -Al
        public DateTime? Date { get; private set; }
        public string InvoiceNumber { get; private set; }
        public double Total { get; private set; }

        private readonly List<VendorInvoiceLineItem> lineItems = new();
        public IReadOnlyList<VendorInvoiceLineItem> LineItems => lineItems.ToList();

        private readonly List<VendorInvoicePayment> payments = new();
        public IReadOnlyList<VendorInvoicePayment> Payments => payments.ToList();

        private readonly List<VendorInvoiceTax> taxes = new();
        public IReadOnlyList<VendorInvoiceTax> Taxes => taxes.ToList();

        private VendorInvoice(
            Vendor vendor,
            VendorInvoiceStatus status,
            VendorInvoiceDocumentType documentType,
            double total,
            IReadOnlyList<string> vendorInvoiceNumbers,
            string invoiceNumber = null,
            DateTime? date = null,
            DateTime? datePosted = null)
        {
            if (vendor is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (!Enum.IsDefined(typeof(VendorInvoiceStatus), status))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (!Enum.IsDefined(typeof(VendorInvoiceDocumentType), documentType))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (total < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            if ((invoiceNumber ?? string.Empty).Trim().Length > InvoiceNumberMaximumLength)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            if ((invoiceNumber ?? string.Empty).Trim().Length > 0)
                if (!InvoiceNumberIsUnique(vendorInvoiceNumbers, invoiceNumber))
                    throw new ArgumentOutOfRangeException(NonuniqueMessage);

            if (!date.HasValue)
                Date = DateTime.UtcNow;

            if (date.HasValue)
            {
                if (date.Value > DateTime.Today)
                    throw new ArgumentOutOfRangeException(DateInvalidMessage);

                Date = date.Value;
            }

            if (datePosted.HasValue)
            {
                if (datePosted.Value > DateTime.Today)
                    throw new ArgumentOutOfRangeException(DateInvalidMessage);

                DatePosted = datePosted.Value;
            }

            Vendor = vendor;
            Status = status;
            DocumentType = documentType;
            Total = total;
            InvoiceNumber = invoiceNumber;
        }

        public static Result<VendorInvoice> Create(
            Vendor vendor,
            VendorInvoiceStatus status,
            VendorInvoiceDocumentType documentType,
            double total,
            IReadOnlyList<string> vendorInvoiceNumbers,
            string invoiceNumber = null,
            DateTime? date = null,
            DateTime? datePosted = null)
        {
            if (vendor is null)
                return Result.Failure<VendorInvoice>(RequiredMessage);

            if (!Enum.IsDefined(typeof(VendorInvoiceStatus), status))
                return Result.Failure<VendorInvoice>(RequiredMessage);

            if (!Enum.IsDefined(typeof(VendorInvoiceDocumentType), documentType))
                return Result.Failure<VendorInvoice>(RequiredMessage);

            if ((invoiceNumber ?? string.Empty).Trim().Length > InvoiceNumberMaximumLength)
                return Result.Failure<VendorInvoice>(InvoiceNumberMaximumLengthMessage);

            if ((invoiceNumber ?? string.Empty).Trim().Length > 0)
                if (!InvoiceNumberIsUnique(vendorInvoiceNumbers, invoiceNumber))
                    return Result.Failure<VendorInvoice>(NonuniqueMessage);

            if (total < MinimumValue)
                return Result.Failure<VendorInvoice>(MinimumValueMessage);

            if (date.HasValue && date.Value > DateTime.Today)
                return Result.Failure<VendorInvoice>(DateInvalidMessage);

            if (datePosted.HasValue && datePosted.Value > DateTime.Today)
                return Result.Failure<VendorInvoice>(DateInvalidMessage);

            return Result.Success(new VendorInvoice(
                vendor, status, documentType, total, vendorInvoiceNumbers, invoiceNumber, date, datePosted));
        }

        public Result UpdateProperties(Vendor vendor, VendorInvoiceStatus status,
            VendorInvoiceDocumentType documentType, DateTime? datePosted, DateTime? date,
            string invoiceNumber, IReadOnlyList<string> vendorInvoiceNumbers, double total)
        {
            return Result.Combine(
                (vendor is not null) && (vendor.Id != Vendor.Id)
                    ? SetVendor(vendor)
                    : Result.Success(),
                (status != Status)
                    ? SetStatus(status)
                    : Result.Success(),
                (documentType != DocumentType)
                    ? SetDocumentType(documentType)
                    : Result.Success(),
                (datePosted is not null) && (datePosted != DatePosted)
                    ? SetDatePosted(datePosted)
                    : Result.Success(),
                (date is not null) && (date != Date)
                    ? SetDate(date)
                    : Result.Success(),
                (invoiceNumber != InvoiceNumber)
                    ? SetInvoiceNumber(invoiceNumber, vendorInvoiceNumbers)
                    : Result.Success(),
                (total != Total)
                    ? SetTotal(total)
                    : Result.Success());
        }

        public Result UpdateCollections(VendorInvoiceCollections vendorInvoiceCollections)
        {
            return Result.Combine(
                SyncLineItems(vendorInvoiceCollections.LineItems),
                SyncPayments(vendorInvoiceCollections.Payments),
                SyncTaxes(vendorInvoiceCollections.Taxes));
        }

        private Result SyncTaxes(IReadOnlyList<VendorInvoiceTax> taxes)
        {
            var toAdd = taxes
                .Where(tax => tax.Id == 0)
                .ToArray();

            var toDelete = taxes
                .Where(tax => taxes.Any(callerTax => callerTax.Id == tax.Id) == false)
                .ToArray();

            var toModify = taxes
                .Where(tax => taxes.Any(callerTax => callerTax.Id == tax.Id))
                .ToArray();

            foreach (var tax in toAdd)
                AddTax(tax);

            foreach (var tax in toDelete)
                RemoveTax(tax);

            foreach (var tax in toModify)
            {
                var callerTax = this.taxes.Find(payment => payment.Id == payment.Id);

                if (tax.Amount != callerTax.Amount)
                    tax.SetAmount(callerTax.Amount);

                if (tax.SalesTax != callerTax.SalesTax)
                    tax.SetSalesTax(callerTax.SalesTax);
            }

            return Result.Success();
        }

        private Result SyncPayments(IReadOnlyList<VendorInvoicePayment> payments)
        {
            var toAdd = payments
                .Where(payment => payment.Id == 0)
                .ToArray();

            var toDelete = payments
                .Where(payment => payments.Any(callerPayment => callerPayment.Id == payment.Id) == false)
                .ToArray();

            var toModify = payments
                .Where(phone => payments.Any(callerPayment => callerPayment.Id == phone.Id))
                .ToArray();

            foreach (var payment in toAdd)
                AddPayment(payment);

            foreach (var payment in toDelete)
                RemovePayment(payment);

            foreach (var payment in toModify)
            {
                var callerPayment = this.payments.Find(payment => payment.Id == payment.Id);

                if (payment.PaymentMethod != callerPayment.PaymentMethod)
                    payment.SetPaymentMethod(callerPayment.PaymentMethod);

                if (payment.Amount != callerPayment.Amount)
                    payment.SetAmount(callerPayment.Amount);
            }

            return Result.Success();
        }

        private Result SyncLineItems(IReadOnlyList<VendorInvoiceLineItem> lineItems)
        {
            var toAdd = lineItems
                .Where(lineItem => lineItem.Id == 0)
                .ToArray();

            var toDelete = lineItems
                .Where(lineItem => lineItems.Any(callerLineItem => callerLineItem.Id == lineItem.Id) == false)
                .ToArray();

            var toModify = lineItems
                .Where(lineItem => lineItems.Any(callerLineItem => callerLineItem.Id == lineItem.Id))
                .ToArray();

            foreach (var lineItem in toAdd)
                AddLineItem(lineItem);

            foreach (var lineItem in toDelete)
                RemoveLineItem(lineItem);

            foreach (var lineItem in toModify)
            {
                var callerLineItem = this.lineItems.Find(lineItem => lineItem.Id == lineItem.Id);

                if (lineItem.Type != callerLineItem.Type)
                    lineItem.SetType(callerLineItem.Type);

                if (lineItem.Item != callerLineItem.Item)
                    lineItem.SetItem(callerLineItem.Item);

                if (lineItem.Cost != callerLineItem.Cost)
                    lineItem.SetCost(callerLineItem.Cost);

                if (lineItem.Core != callerLineItem.Core)
                    lineItem.SetCore(callerLineItem.Core);

                if (lineItem.PONumber != callerLineItem.PONumber)
                    lineItem.SetPONumber(callerLineItem.PONumber);

                if (lineItem.Quantity != callerLineItem.Quantity)
                    lineItem.SetQuantity(callerLineItem.Quantity);

                if (lineItem.TransactionDate != callerLineItem.TransactionDate)
                    lineItem.SetTransactionDate(callerLineItem.TransactionDate);
            }

            return Result.Success();
        }

        public Result<Vendor> SetVendor(Vendor vendor)
        {
            return vendor is null
                ? Result.Failure<Vendor>(RequiredMessage)
                : Result.Success(Vendor = vendor);
        }

        public Result<VendorInvoiceStatus> SetStatus(VendorInvoiceStatus status)
        {
            return !Enum.IsDefined(typeof(VendorInvoiceStatus), status)
                ? Result.Failure<VendorInvoiceStatus>(RequiredMessage)
                : Result.Success(Status = status);
        }

        public Result<VendorInvoiceDocumentType> SetDocumentType(VendorInvoiceDocumentType documentType)
        {
            return !Enum.IsDefined(typeof(VendorInvoiceDocumentType), documentType)
              ? Result.Failure<VendorInvoiceDocumentType>(RequiredMessage)
              : Result.Success(DocumentType = documentType);
        }

        public Result<string> SetInvoiceNumber(string invoiceNumber, IReadOnlyList<string> vendorInvoiceNumbers)
        {
            if (invoiceNumber is null)
                return Result.Failure<string>(RequiredMessage);

            invoiceNumber = (invoiceNumber ?? string.Empty).Trim();

            if (invoiceNumber.Length > InvoiceNumberMaximumLength)
                return Result.Failure<string>(InvoiceNumberMaximumLengthMessage);

            if (!InvoiceNumberIsUnique(vendorInvoiceNumbers, invoiceNumber))
                return Result.Failure<string>(NonuniqueMessage);

            return Result.Success(InvoiceNumber = invoiceNumber);
        }

        public Result<double> SetTotal(double total)
        {
            return total < MinimumValue
                ? Result.Failure<double>(MinimumValueMessage)
                : Result.Success(Total = total);
        }

        public Result<DateTime?> SetDate(DateTime? date)
        {
            if (date is null)
                return Result.Failure<DateTime?>(DateInvalidMessage);

            return date.HasValue && date.Value > DateTime.Today
                ? Result.Failure<DateTime?>(DateInvalidMessage)
                : Result.Success(Date = date.Value);
        }

        public void ClearDate() => Date = null;

        public Result<DateTime?> SetDatePosted(DateTime? datePosted)
        {
            if (datePosted is null)
                return Result.Failure<DateTime?>(DateInvalidMessage);

            return datePosted.HasValue && datePosted.Value > DateTime.Today
                ? Result.Failure<DateTime?>(DateInvalidMessage)
                : Result.Success(DatePosted = datePosted.Value);
        }

        public void ClearDatePosted() => DatePosted = null;

        public Result<VendorInvoiceLineItem> AddLineItem(VendorInvoiceLineItem lineItem)
        {
            if (lineItem is null)
                return Result.Failure<VendorInvoiceLineItem>(RequiredMessage);

            // TODO: VK: Is this the correct use of Result<T> in domain collection mutation method?
            lineItems.Add(lineItem);

            return Result.Success(lineItem);
        }

        public Result<VendorInvoiceLineItem> RemoveLineItem(VendorInvoiceLineItem lineItem)
        {
            if (lineItem is null)
                return Result.Failure<VendorInvoiceLineItem>(RequiredMessage);

            lineItems.Remove(lineItem);

            return Result.Success(lineItem);
        }

        public Result<VendorInvoicePayment> AddPayment(VendorInvoicePayment payment)
        {
            if (payment is null)
                return Result.Failure<VendorInvoicePayment>(RequiredMessage);

            payments.Add(payment);

            return Result.Success(payment);
        }

        public Result<VendorInvoicePayment> RemovePayment(VendorInvoicePayment payment)
        {
            if (payment is null)
                return Result.Failure<VendorInvoicePayment>(RequiredMessage);

            payments.Remove(payment);

            return Result.Success(payment);
        }

        public Result<VendorInvoiceTax> AddTax(VendorInvoiceTax tax)
        {
            if (tax is null)
                return Result.Failure<VendorInvoiceTax>(RequiredMessage);

            taxes.Add(tax);

            return Result.Success(tax);
        }

        public Result<VendorInvoiceTax> RemoveTax(VendorInvoiceTax tax)
        {
            if (tax is null)
                return Result.Failure<VendorInvoiceTax>(RequiredMessage);

            taxes.Remove(tax);

            return Result.Success(tax);
        }

        private static bool InvoiceNumberIsUnique(
            IReadOnlyList<string> vendorInvoiceNumbers,
            string invoiceNumber)
            =>
                !string.IsNullOrWhiteSpace(invoiceNumber) && !vendorInvoiceNumbers.Contains(invoiceNumber);

        #region ORM

        // EF requires a parameterless constructor
        protected VendorInvoice() { }

        #endregion
    }
}
