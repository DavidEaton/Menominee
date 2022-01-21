using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Items;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices
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
