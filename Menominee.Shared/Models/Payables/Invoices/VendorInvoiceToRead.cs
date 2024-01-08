using Menominee.Domain.Enums;
using Menominee.Shared.Models.Payables.Invoices.LineItems;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Menominee.Shared.Models.Payables.Invoices.Taxes;
using Menominee.Shared.Models.Payables.Vendors;
using System;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Payables.Invoices
{
    public class VendorInvoiceToRead
    {
        public long Id { get; set; }
        public VendorToRead Vendor { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DatePosted { get; set; }
        public VendorInvoiceStatus Status { get; set; }
        public VendorInvoiceDocumentType DocumentType { get; set; } = VendorInvoiceDocumentType.Invoice;
        public string InvoiceNumber { get; set; }
        public double Total { get; set; }
        public IReadOnlyList<VendorInvoiceLineItemToRead> LineItems { get; set; } = new List<VendorInvoiceLineItemToRead>();
        public IReadOnlyList<VendorInvoicePaymentToRead> Payments { get; set; } = new List<VendorInvoicePaymentToRead>();
        public IReadOnlyList<VendorInvoiceTaxToRead> Taxes { get; set; } = new List<VendorInvoiceTaxToRead>();
    }
}
