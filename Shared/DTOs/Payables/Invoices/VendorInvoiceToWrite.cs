using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Items;
using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Payments;
using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Taxes;
using MenomineePlayWASM.Shared.Entities.Payables.Enums;
using System;
using System.Collections.Generic;

namespace MenomineePlayWASM.Shared.Dtos.Payables.Invoices
{
    public class VendorInvoiceToWrite
    {
        public long Id { get; set; } = 0;
        public long VendorId { get; set; } = 0;
        public DateTime? Date { get; set; }
        public DateTime? DatePosted { get; set; }
        public VendorInvoiceStatus Status { get; set; } = VendorInvoiceStatus.Unknown;
        public string InvoiceNumber { get; set; } = string.Empty;
        public double Total { get; set; } = 0;
        public IList<VendorInvoiceItemToWrite> LineItems { get; set; } = new List<VendorInvoiceItemToWrite>();
        public IList<VendorInvoicePaymentToWrite> Payments { get; set; } = new List<VendorInvoicePaymentToWrite>();
        public IList<VendorInvoiceTaxToWrite> Taxes { get; set; } = new List<VendorInvoiceTaxToWrite>();
    }
}
