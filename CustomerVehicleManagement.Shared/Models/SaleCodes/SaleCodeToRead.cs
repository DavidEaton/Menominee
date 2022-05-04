namespace CustomerVehicleManagement.Shared.Models.SaleCodes
{
    public class SaleCodeToRead
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double LaborRate { get; set; }
        public double DesiredMargin { get; set; }

        public double ShopSuppliesPercentage { get; set; }
        public double ShopSuppliesMinimumJobAmount { get; set; }
        public double ShopSuppliesMinimumCharge { get; set; }
        public double ShopSuppliesMaximumCharge { get; set; }
        public bool ShopSuppliesIncludeParts { get; set; }
        public bool ShopSuppliesIncludeLabor { get; set; }
    }
}
