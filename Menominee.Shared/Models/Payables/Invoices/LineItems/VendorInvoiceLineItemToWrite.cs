using Menominee.Shared.Models.Payables.Invoices.LineItems.Items;
using Menominee.Common.Enums;
using System;

namespace Menominee.Shared.Models.Payables.Invoices.LineItems
{
    public class VendorInvoiceLineItemToWrite
    {
        public long Id { get; set; }
        public VendorInvoiceLineItemType Type { get; set; } = VendorInvoiceLineItemType.Purchase;
        public VendorInvoiceItemToWrite Item { get; set; } = new();
        public double Quantity { get; set; } = 0.0;
        public double Cost { get; set; } = 0.0;
        public double Core { get; set; } = 0.0;
        public string PONumber { get; set; } = string.Empty;
        public DateTime? TransactionDate { get; set; } = DateTime.Today;
    }
}
