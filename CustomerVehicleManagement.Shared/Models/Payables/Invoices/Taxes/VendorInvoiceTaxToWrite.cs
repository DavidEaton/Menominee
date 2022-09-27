using CustomerVehicleManagement.Shared.Models.Taxes;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes
{
    public class VendorInvoiceTaxToWrite
    {
        public long Id { get; set; }
        public SalesTaxToRead SalesTax { get; set; }
        public double Amount { get; set; }
        public int TaxId { get; set; } = 0;
    }
}
