using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Items;
using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Payments;
using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Taxes;
using MenomineePlayWASM.Shared.Dtos.Payables.Vendors;
using System;
using System.Collections.Generic;

namespace MenomineePlayWASM.Shared.Dtos.Payables.Invoices
{
    public class VendorInvoiceToRead
    {
        public long Id { get; set; }
        public VendorToRead Vendor { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DatePosted { get; set; }
        public string Status { get; set; }
        public string InvoiceNumber { get; set; }
        public double Total { get; set; }
        public IReadOnlyList<VendorInvoiceItemToRead> LineItems { get; set; } = new List<VendorInvoiceItemToRead>();
        public IReadOnlyList<VendorInvoicePaymentToRead> Payments { get; set; } = new List<VendorInvoicePaymentToRead>();
        public IReadOnlyList<VendorInvoiceTaxToRead> Taxes { get; set; } = new List<VendorInvoiceTaxToRead>();
    }
}
