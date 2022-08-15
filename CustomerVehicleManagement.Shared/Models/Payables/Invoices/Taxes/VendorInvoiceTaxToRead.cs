using CustomerVehicleManagement.Shared.Models.Taxes;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes
{
    public class VendorInvoiceTaxToRead
    {
        public long Id { get; set; }
        public SalesTaxToRead SalesTax { get; set; }
        public int Order { get; set; }
        public int TaxId { get; set; }
        public string TaxName { get; set; }
        public double Amount { get; set; }
    }
}
