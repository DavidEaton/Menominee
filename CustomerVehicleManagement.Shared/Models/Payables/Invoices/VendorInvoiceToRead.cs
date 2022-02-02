using CustomerVehicleManagement.Shared.CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Items;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices
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
