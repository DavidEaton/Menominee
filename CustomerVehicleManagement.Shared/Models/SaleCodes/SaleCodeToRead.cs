namespace CustomerVehicleManagement.Shared.Models.SaleCodes
{
    public class SaleCodeToRead
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double LaborRate { get; set; }
        public double DesiredMargin { get; set; }
        public SaleCodeShopSuppliesToRead ShopSupplies { get; set; }
    }
}
