using Menominee.Common;
using System;
using System.Collections.Generic;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoice : Entity
    {
        public virtual Vendor Vendor { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DatePosted { get; set; }
        public VendorInvoiceStatus Status { get; set; }
        public string InvoiceNumber { get; set; }
        public double Total { get; set; }

        public virtual IList<VendorInvoiceItem> LineItems { get; set; } = new List<VendorInvoiceItem>();
        public virtual IList<VendorInvoicePayment> Payments { get; set; } = new List<VendorInvoicePayment>();
        public virtual IList<VendorInvoiceTax> Taxes { get; set; } = new List<VendorInvoiceTax>();

        public void AddItem(VendorInvoiceItem item)
        {
            LineItems.Add(item);
        }

        public void RemoveItem(VendorInvoiceItem item)
        {
            LineItems.Remove(item);
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

        public void SetItems(IList<VendorInvoiceItem> items)
        {
            if (items.Count > 0)
            {
                LineItems.Clear();
                foreach (var item in items)
                    AddItem(item);
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
