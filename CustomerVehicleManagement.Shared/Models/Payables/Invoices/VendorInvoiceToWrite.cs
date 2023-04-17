using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices
{
    public class VendorInvoiceToWrite
    {
        public long Id { get; set; }
        public VendorToRead Vendor { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DatePosted { get; set; }
        public VendorInvoiceStatus Status { get; set; } = VendorInvoiceStatus.Unknown;
        public VendorInvoiceDocumentType DocumentType { get; set; } = VendorInvoiceDocumentType.Invoice;
        public string InvoiceNumber { get; set; } = string.Empty;
        public double Total { get; set; } = 0;
        public IList<VendorInvoiceLineItemToWrite> LineItems { get; set; } = new List<VendorInvoiceLineItemToWrite>();
        public IList<VendorInvoicePaymentToWrite> Payments { get; set; } = new List<VendorInvoicePaymentToWrite>();
        public IList<VendorInvoiceTaxToWrite> Taxes { get; set; } = new List<VendorInvoiceTaxToWrite>();
    }
}
