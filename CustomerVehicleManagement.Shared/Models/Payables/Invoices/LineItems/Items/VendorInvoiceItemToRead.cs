using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.SaleCodes;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems.Items
{
    public class VendorInvoiceItemToRead
    {
        public string PartNumber { get; set; } = string.Empty;
        public ManufacturerToRead Manufacturer { get; set; }
        public string Description { get; set; } = string.Empty;
        public SaleCodeToRead SaleCode { get; set; }

    }
}