namespace CustomerVehicleManagement.Shared.Models.SaleCodes
{
    public class SaleCodeShopSuppliesToReadInList
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Percentage { get; set; }
        public double MinimumJobAmount { get; set; }
        public double MinimumCharge { get; set; }
        public double MaximumCharge { get; set; }
        public bool IncludeParts { get; set; }
        public bool IncludeLabor { get; set; }
        public string DisplayText
        {
            get
            {
                return Code + " - " + Name;
            }
        }
    }
}
