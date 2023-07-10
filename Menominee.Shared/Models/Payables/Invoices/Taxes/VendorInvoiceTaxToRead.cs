using Menominee.Shared.Models.Taxes;

namespace Menominee.Shared.Models.Payables.Invoices.Taxes
{
    public class VendorInvoiceTaxToRead
    {
        public long Id { get; set; }
        public SalesTaxToRead SalesTax { get; set; }
        public double Amount { get; set; }
    }
}
