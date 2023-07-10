using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.SaleCodes;

namespace Menominee.Shared.Models.Payables.Invoices.LineItems.Items
{
    public class VendorInvoiceItemToRead
    {
        public string PartNumber { get; set; } = string.Empty;
        public ManufacturerToRead Manufacturer { get; set; }
        public string Description { get; set; } = string.Empty;
        public SaleCodeToRead SaleCode { get; set; }

    }
}