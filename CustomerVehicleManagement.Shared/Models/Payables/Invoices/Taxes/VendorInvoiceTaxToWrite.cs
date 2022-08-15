using CustomerVehicleManagement.Shared.Models.Taxes;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes
{
    public class VendorInvoiceTaxToWrite
    {
        public SalesTaxToWrite SalesTax { get; set; }
        public int Order { get; set; } = 0;
        public int TaxId { get; set; } = 0;
        public string TaxName { get; set; } = string.Empty;
        public double Amount { get; set; } = 0.0;
    }
}
