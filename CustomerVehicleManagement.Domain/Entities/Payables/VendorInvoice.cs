using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
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
        public static readonly int InvoiceNumberMaximumLength = 255;
        public static readonly string InvoiceNumberMaximumLengthMessage = $"Invoice Number cannot be over {InvoiceNumberMaximumLength} characters in length.";
        public static readonly double MinimumValue = 0;
        public static readonly string MinimumValueMessage = $"Value(s) cannot be negative.";

        public Vendor Vendor { get; private set; }
        public DateTime? Date { get; private set; }
        public DateTime? DatePosted { get; private set; }
        public VendorInvoiceStatus Status { get; private set; }
        public string InvoiceNumber { get; private set; }
        public double Total { get; private set; }

        public IList<VendorInvoiceLineItem> LineItems { get; private set; }
        public IList<VendorInvoicePayment> Payments { get; private set; }
        public IList<VendorInvoiceTax> Taxes { get; private set; }

        private VendorInvoice(
            Vendor vendor, 
            DateTime? date,
            DateTime? datePosted,
            VendorInvoiceStatus status,
            string invoiceNumber,
            double total,
            IList<VendorInvoiceLineItem> lineItems,
            IList<VendorInvoicePayment> payments,
            IList<VendorInvoiceTax> taxes)
        {
            Vendor = vendor;
            Date = date;
            DatePosted = datePosted;
            Status = status;
            InvoiceNumber = invoiceNumber;
            Total = total;
            LineItems = lineItems ?? new List<VendorInvoiceLineItem>();
            Payments = payments ?? new List<VendorInvoicePayment>();
            Taxes = taxes ?? new List<VendorInvoiceTax>();
        }

        public static Result<VendorInvoice> Create(
            Vendor vendor,
            DateTime? date,
            DateTime? datePosted,
            VendorInvoiceStatus status,
            string invoiceNumber,
            double total,
            IList<VendorInvoiceLineItem> lineItems,
            IList<VendorInvoicePayment> payments,
            IList<VendorInvoiceTax> taxes)
        {
            if (vendor is null)
                return Result.Failure<VendorInvoice>(RequiredMessage);

            if (date.HasValue && date.Value > DateTime.Today)
                return Result.Failure<VendorInvoice>(DateInvalidMessage);

            if (datePosted.HasValue && datePosted.Value > DateTime.Today)
                return Result.Failure<VendorInvoice>(DateInvalidMessage);

            if (!Enum.IsDefined(typeof(VendorInvoiceStatus), status))
                return Result.Failure<VendorInvoice>(RequiredMessage);

            invoiceNumber = (invoiceNumber ?? string.Empty).Trim();

            if (invoiceNumber.Length > InvoiceNumberMaximumLength)
                return Result.Failure<VendorInvoice>(InvoiceNumberMaximumLengthMessage);

            if (total < MinimumValue)
                return Result.Failure<VendorInvoice>(MinimumValueMessage);

            return Result.Success(new VendorInvoice(
                vendor, date, datePosted, status, invoiceNumber, total, lineItems, payments, taxes));
        }

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

        public void SetItems(IList<VendorInvoiceLineItem> items)
        {
            if (items.Count > 0)
            {
                LineItems.Clear();
                foreach (var item in items)
                    AddLineItem(item);
            }
        }

        public void SetPayments(IList<VendorInvoicePayment> payments)
        {
            if (payments.Count > 0)
            {
                Payments.Clear();
                foreach (var payment in payments)
                    AddPayment(payment);
            }
        }

        public void SetTaxes(IList<VendorInvoiceTax> taxes)
        {
            if (taxes.Count > 0)
            {
                Taxes.Clear();
                foreach (var tax in taxes)
                    AddTax(tax);
            }
        }

        #region ORM

        // EF requires an empty constructor
        public VendorInvoice() { }

        #endregion
    }
}
