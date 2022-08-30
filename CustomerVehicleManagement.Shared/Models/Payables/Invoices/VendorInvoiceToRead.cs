using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using System;
using System.Collections.Generic;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices
{
    public class VendorInvoiceToRead
    {
        public long Id { get; set; }
        public VendorToRead Vendor { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DatePosted { get; set; }
        public string Status { get; set; }
        public string InvoiceNumber { get; set; }
        public double Total { get; set; }
        public IReadOnlyList<VendorInvoiceLineItemToRead> LineItems { get; set; } = new List<VendorInvoiceLineItemToRead>();
        public IReadOnlyList<VendorInvoicePaymentToRead> Payments { get; set; } = new List<VendorInvoicePaymentToRead>();
        public IReadOnlyList<VendorInvoiceTaxToRead> Taxes { get; set; } = new List<VendorInvoiceTaxToRead>();
    }
}
