using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.SaleCodes;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems.Items
{
    public class VendorInvoiceItemToWrite
    {
        public string PartNumber { get; set; } = string.Empty;
        public ManufacturerToWrite Manufacturer { get; set; }
        public string Description { get; set; } = string.Empty;
        public SaleCodeToWrite SaleCode { get; set; }
    }
}