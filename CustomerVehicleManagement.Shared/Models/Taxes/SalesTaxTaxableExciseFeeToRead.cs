namespace CustomerVehicleManagement.Shared.Models.Taxes
{
    public class SalesTaxTaxableExciseFeeToRead
    {
        public long Id { get; set; }
        public SalesTaxToRead SalesTax { get; set; }
        public ExciseFeeToRead ExciseFee { get; set; }
    }
}
