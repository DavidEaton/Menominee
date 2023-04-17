using CustomerVehicleManagement.Domain.Entities.Payables;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.BaseClasses
{
    public class VendorInvoiceCollections : AppValueObject
    {
        public IReadOnlyList<VendorInvoiceLineItem> LineItems { get; }
        public IReadOnlyList<VendorInvoicePayment> Payments { get; }
        public IReadOnlyList<VendorInvoiceTax> Taxes { get; }

        public VendorInvoiceCollections(
            IReadOnlyList<VendorInvoiceLineItem> lineItems,
            IReadOnlyList<VendorInvoicePayment> payments,
            IReadOnlyList<VendorInvoiceTax> taxes)
        {
            if (lineItems == null || payments == null || taxes == null)
                throw new Exception("LineItems or Payments or Taxes are null");

            LineItems = lineItems;
            Payments = payments;
            Taxes = taxes;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            foreach (var lineItem in LineItems)
                yield return lineItem;

            foreach (var payment in Payments)
                yield return payment;

            foreach (var tax in Taxes)
                yield return tax;
        }
    }
}
