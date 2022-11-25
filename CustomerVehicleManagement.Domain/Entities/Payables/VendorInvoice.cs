using CSharpFunctionalExtensions;
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
        public DateTime? DatePosted { get; private set; }
        public DateTime? Date { get; private set; }
        public string InvoiceNumber { get; private set; }
        public double Total { get; private set; }

        public IList<VendorInvoiceLineItem> LineItems { get; private set; }
        public IList<VendorInvoicePayment> Payments { get; private set; }
        public IList<VendorInvoiceTax> Taxes { get; private set; }

        private VendorInvoice(
            Vendor vendor,
            VendorInvoiceStatus status,
            VendorInvoiceDocumentType documentType,
            double total,
            IReadOnlyList<string> vendorInvoiceNumbers,
            string invoiceNumber = null,
            DateTime? date = null,
            DateTime? datePosted = null,
            IList<VendorInvoiceLineItem> lineItems = null,
            IList<VendorInvoicePayment> payments = null,
            IList<VendorInvoiceTax> taxes = null)
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
                if (!InvoiceNumberIsUnique(vendor, vendorInvoiceNumbers, invoiceNumber))
                    throw new ArgumentOutOfRangeException(NonuniqueMessage);

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
            LineItems = lineItems ?? new List<VendorInvoiceLineItem>();
            Payments = payments ?? new List<VendorInvoicePayment>();
            Taxes = taxes ?? new List<VendorInvoiceTax>();
        }

        public static Result<VendorInvoice> Create(
            Vendor vendor,
            VendorInvoiceStatus status,
            VendorInvoiceDocumentType documentType,
            double total,
            IReadOnlyList<string> vendorInvoiceNumbers,
            string invoiceNumber = null,
            DateTime? date = null,
            DateTime? datePosted = null,
            IList<VendorInvoiceLineItem> lineItems = null,
            IList<VendorInvoicePayment> payments = null,
            IList<VendorInvoiceTax> taxes = null)
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
                if (!InvoiceNumberIsUnique(vendor, vendorInvoiceNumbers, invoiceNumber))
                    return Result.Failure<VendorInvoice>(NonuniqueMessage);

            if (total < MinimumValue)
                return Result.Failure<VendorInvoice>(MinimumValueMessage);

            if (date.HasValue && date.Value > DateTime.Today)
                return Result.Failure<VendorInvoice>(DateInvalidMessage);

            if (datePosted.HasValue && datePosted.Value > DateTime.Today)
                return Result.Failure<VendorInvoice>(DateInvalidMessage);

            return Result.Success(new VendorInvoice(
                vendor, status, documentType, total, vendorInvoiceNumbers, invoiceNumber, date, datePosted, lineItems, payments, taxes));
        }

        public Result<Vendor> SetVendor(Vendor vendor)
        {
            if (vendor is null)
                return Result.Failure<Vendor>(RequiredMessage);

            return Result.Success(Vendor = vendor);
        }

        public Result<VendorInvoiceStatus> SetVendorInvoiceStatus(VendorInvoiceStatus status)
        {
            if (!Enum.IsDefined(typeof(VendorInvoiceStatus), status))
                return Result.Failure<VendorInvoiceStatus>(RequiredMessage);

            return Result.Success(Status = status);
        }

        public Result<VendorInvoiceDocumentType> SetVendorInvoiceDocumentType(VendorInvoiceDocumentType documentType)
        {
            if (!Enum.IsDefined(typeof(VendorInvoiceDocumentType), documentType))
                return Result.Failure<VendorInvoiceDocumentType>(RequiredMessage);

            return Result.Success(DocumentType = documentType);
        }

        public Result<string> SetInvoiceNumber(string invoiceNumber, IReadOnlyList<string> vendorInvoiceNumbers)
        {
            if (invoiceNumber is null)
                return Result.Failure<string>(RequiredMessage);

            invoiceNumber = (invoiceNumber ?? string.Empty).Trim();

            if (invoiceNumber.Length > InvoiceNumberMaximumLength)
                return Result.Failure<string>(InvoiceNumberMaximumLengthMessage);

            if (!InvoiceNumberIsUnique(Vendor, vendorInvoiceNumbers, invoiceNumber))
                return Result.Failure<string>(NonuniqueMessage);

            return Result.Success(InvoiceNumber = invoiceNumber);
        }

        public Result<double> SetTotal(double total)
        {
            if (total < MinimumValue)
                return Result.Failure<double>(MinimumValueMessage);

            return Result.Success(Total = total);
        }

        public Result<DateTime?> SetDate(DateTime? date)
        {
            if (date is null)
                return Result.Failure<DateTime?>(DateInvalidMessage);

            if (date.HasValue && date.Value > DateTime.Today)
                return Result.Failure<DateTime?>(DateInvalidMessage);

            return Result.Success(Date = date.Value);
        }

        public void ClearDate() => Date = null;

        public Result<DateTime?> SetDatePosted(DateTime? datePosted)
        {
            if (datePosted is null)
                return Result.Failure<DateTime?>(DateInvalidMessage);

            if (datePosted.HasValue && datePosted.Value > DateTime.Today)
                return Result.Failure<DateTime?>(DateInvalidMessage);

            return Result.Success(DatePosted = datePosted.Value);
        }

        public void ClearDatePosted() => DatePosted = null;

        public void AddLineItem(VendorInvoiceLineItem lineItem)
        {
            LineItems.Add(lineItem);
        }

        public void RemoveLineItem(VendorInvoiceLineItem lineItem)
        {
            LineItems.Remove(lineItem);
        }

        public void AddPayment(VendorInvoicePayment payment)
        {
            Payments.Add(payment);
        }

        public void RemovePayment(VendorInvoicePayment payment)
        {
            Payments.Remove(payment);
        }

        public void AddTax(VendorInvoiceTax tax)
        {
            Taxes.Add(tax);
        }

        public void RemoveTax(VendorInvoiceTax tax)
        {
            Taxes.Remove(tax);
        }

        private static bool InvoiceNumberIsUnique(Vendor vendor, IReadOnlyList<string> vendorInvoiceNumbers, string invoiceNumber)
        {
            return !string.IsNullOrWhiteSpace(invoiceNumber) && !vendorInvoiceNumbers.Contains(invoiceNumber);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected VendorInvoice() { }

        #endregion
    }
}
