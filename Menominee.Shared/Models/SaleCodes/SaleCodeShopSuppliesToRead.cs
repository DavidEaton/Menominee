namespace Menominee.Shared.Models.SaleCodes
{
    public class SaleCodeShopSuppliesToRead
    {
        public long Id { get; set; }
        public double Percentage { get; set; }
        public double MinimumJobAmount { get; set; }
        public double MinimumCharge { get; set; }
        public double MaximumCharge { get; set; }
        public bool IncludeParts { get; set; }
        public bool IncludeLabor { get; set; }
    }
}
