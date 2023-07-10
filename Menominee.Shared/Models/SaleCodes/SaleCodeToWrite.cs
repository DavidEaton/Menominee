namespace Menominee.Shared.Models.SaleCodes
{
    public class SaleCodeToWrite
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double LaborRate { get; set; }
        public double DesiredMargin { get; set; }
        public SaleCodeShopSuppliesToWrite ShopSupplies { get; set; } = new();
    }
}
