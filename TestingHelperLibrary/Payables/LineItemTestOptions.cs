using Menominee.Domain.Enums;
using Menominee.Shared.Models.Payables.Invoices.LineItems.Items;

namespace TestingHelperLibrary.Payables
{
    public class LineItemTestOptions
    {
        private readonly Random random = new();
        public LineItemTestOptions()
        {
            RowCount = random.Next(1, 10);
            Type = (VendorInvoiceLineItemType)random.Next(0, 7);
            Item = new()
            {
                PartNumber = Utilities.LoremIpsum(7),
                Description = Utilities.LoremIpsum(100)
            };
            Quantity = random.Next(1, 10);
            Cost = random.Next(1, 10) * .01 + random.Next(1, 10);
            Core = random.Next(1, 10) * .01 + random.Next(1, 10);
            PONumber = ";";
            TransactionDate = 1 == random.Next(0, 1) ? DateTime.Today : null;
        }

        public int RowCount { get; set; } = 1;
        public VendorInvoiceLineItemType Type { get; set; } = VendorInvoiceLineItemType.Purchase;
        public VendorInvoiceItemToWrite Item { get; set; }
        public double Quantity { get; set; } = 0.0;
        public double Cost { get; set; } = 0.0;
        public double Core { get; set; } = 0.0;
        public string PONumber { get; set; } = string.Empty;
        public DateTime? TransactionDate { get; set; } = DateTime.Today;
    }
}
