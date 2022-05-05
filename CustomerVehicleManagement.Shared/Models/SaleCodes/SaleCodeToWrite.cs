namespace CustomerVehicleManagement.Shared.Models.SaleCodes
{
    public class SaleCodeToWrite
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double LaborRate { get; set; }
        public double DesiredMargin { get; set; }
        public SaleCodeShopSuppliesToWrite ShopSupplies { get; set; }
    }
}
