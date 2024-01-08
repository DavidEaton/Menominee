using Menominee.Domain.Enums;
using Menominee.Shared.Models.Payables.Invoices.LineItems.Items;
using System;

namespace Menominee.Shared.Models.Payables.Invoices.LineItems
{
    public class VendorInvoiceLineItemToRead
    {
        public long Id { get; set; }
        public VendorInvoiceLineItemType Type { get; set; }
        public VendorInvoiceItemToRead Item { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public string PONumber { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
