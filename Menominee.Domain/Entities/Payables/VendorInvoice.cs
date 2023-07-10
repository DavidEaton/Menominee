using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities.Payables
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
            Vendor = vendor;
            Status = status;
            DocumentType = documentType;
            Total = total;
            InvoiceNumber = invoiceNumber;
            Date = date;
            DatePosted = datePosted;
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

        public Result UpdateTaxes(IReadOnlyList<VendorInvoiceTax> taxes)
        {
            var toAdd = taxes
                .Where(tax => tax.Id == 0)
                .ToList();

            var toDelete = Taxes
                .Where(tax => !taxes.Any(callerTax => callerTax.Id == tax.Id))
                .ToList();

            var toModify = Taxes
                .Where(tax => taxes.Any(callerTax => callerTax.Id == tax.Id))
                .ToList();

            var addResults = toAdd
                .Select(tax => AddTax(tax))
                .ToList();

            var deleteResults = toDelete
                .Select(tax => RemoveTax(tax))
                .ToList();

            foreach (var tax in toModify)
            {
                var taxFromCaller = taxes.Single(callerTax => callerTax.Id == tax.Id);

                if (tax.Amount != taxFromCaller.Amount)
                {
                    var result = tax.SetAmount(taxFromCaller.Amount);
                    if (result.IsFailure)
                        return Result.Failure(result.Error);
                }

                if (tax.SalesTax != taxFromCaller.SalesTax)
                {
                    var result = tax.SetSalesTax(taxFromCaller.SalesTax);
                    if (result.IsFailure)
                        return Result.Failure(result.Error);
                }
            }

            return Result.Success();
        }

        public Result UpdatePayments(IReadOnlyList<VendorInvoicePayment> payments)
        {
            var toAdd = payments
                .Where(payment => payment.Id == 0)
                .ToList();

            var toDelete = Payments
                .Where(payment => !payments.Any(callerPayment => callerPayment.Id == payment.Id))
                .ToList();

            var toModify = Payments
                .Where(payment => payments.Any(callerPayment => callerPayment.Id == payment.Id))
                .ToList();

            var addResults = toAdd
                .Select(payment => AddPayment(payment))
                .ToList();

            var deleteResults = toDelete
                .Select(payment => RemovePayment(payment))
                .ToList();

            foreach (var payment in toModify)
            {
                var paymentFromCaller = payments.Single(callerPayment => callerPayment.Id == payment.Id);

                if (payment.PaymentMethod != paymentFromCaller.PaymentMethod)
                    payment.SetPaymentMethod(paymentFromCaller.PaymentMethod);

                if (payment.Amount != paymentFromCaller.Amount)
                    payment.SetAmount(paymentFromCaller.Amount);
            }

            return Result.Success();
        }

        public Result UpdateLineItems(IReadOnlyList<VendorInvoiceLineItem> lineItems)
        {
            var toAdd = lineItems
                .Where(lineItem => lineItem.Id == 0)
                .ToList();

            var toDelete = LineItems
                .Where(lineItem => !lineItems.Any(callerLineItem => callerLineItem.Id == lineItem.Id))
                .ToList();

            var toModify = LineItems
                .Where(lineItem => lineItems.Any(callerLineItem => callerLineItem.Id == lineItem.Id))
                .ToList();

            var addResults = toAdd
                .Select(lineItem => AddLineItem(lineItem))
                .ToList();

            var deleteResults = toDelete
                .Select(lineItem => RemoveLineItem(lineItem))
                .ToList();

            foreach (var lineItem in toModify)
            {
                var lineItemFromCaller = lineItems.Single(callerLineItem => callerLineItem.Id == lineItem.Id);

                if (lineItem.Type != lineItemFromCaller.Type)
                    lineItem.SetType(lineItemFromCaller.Type);

                if (lineItem.Item != lineItemFromCaller.Item)
                    lineItem.SetItem(lineItemFromCaller.Item);

                if (lineItem.Cost != lineItemFromCaller.Cost)
                    lineItem.SetCost(lineItemFromCaller.Cost);

                if (lineItem.Core != lineItemFromCaller.Core)
                    lineItem.SetCore(lineItemFromCaller.Core);

                if (lineItem.PONumber != lineItemFromCaller.PONumber)
                    lineItem.SetPONumber(lineItemFromCaller.PONumber);

                if (lineItem.Quantity != lineItemFromCaller.Quantity)
                    lineItem.SetQuantity(lineItemFromCaller.Quantity);

                if (lineItem.TransactionDate != lineItemFromCaller.TransactionDate)
                    lineItem.SetTransactionDate(lineItemFromCaller.TransactionDate);
            }

            return Result.Success();
        }

        public Result<Vendor> SetVendor(Vendor vendor)
        {
            return 
                vendor is null
                ? Result.Failure<Vendor>(RequiredMessage)
                : Result.Success(Vendor = vendor);
        }

        public Result<VendorInvoiceStatus> SetStatus(VendorInvoiceStatus status)
        {
            return
                !Enum.IsDefined(typeof(VendorInvoiceStatus), status)
                ? Result.Failure<VendorInvoiceStatus>(RequiredMessage)
                : Result.Success(Status = status);
        }

        public Result<VendorInvoiceDocumentType> SetDocumentType(VendorInvoiceDocumentType documentType)
        {
            return
                !Enum.IsDefined(typeof(VendorInvoiceDocumentType), documentType)
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
            return date is null
                ? Result.Failure<DateTime?>(DateInvalidMessage)
                : date.HasValue && date.Value > DateTime.Today
                ? Result.Failure<DateTime?>(DateInvalidMessage)
                : Result.Success(Date = date.Value);
        }

        public void ClearDate() => Date = null;

        public Result<DateTime?> SetDatePosted(DateTime? datePosted)
        {
            return datePosted is null
                ? Result.Failure<DateTime?>(DateInvalidMessage)
                : datePosted.HasValue && datePosted.Value > DateTime.Today
                ? Result.Failure<DateTime?>(DateInvalidMessage)
                : Result.Success(DatePosted = datePosted.Value);
        }

        public void ClearDatePosted() => DatePosted = null;

        public Result<VendorInvoiceLineItem> AddLineItem(VendorInvoiceLineItem lineItem)
        {
            if (lineItem is null)
                return Result.Failure<VendorInvoiceLineItem>(RequiredMessage);

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
        protected VendorInvoice()
        {
            lineItems = new List<VendorInvoiceLineItem>();
            payments = new List<VendorInvoicePayment>();
            taxes = new List<VendorInvoiceTax>();
        }

        #endregion
    }
}
