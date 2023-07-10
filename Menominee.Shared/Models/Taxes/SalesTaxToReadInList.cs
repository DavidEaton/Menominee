using Menominee.Common.Enums;

namespace Menominee.Shared.Models.Taxes
{
    public class SalesTaxToReadInList
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public SalesTaxType TaxType { get; set; }
        public int Order { get; set; }
        public bool? IsAppliedByDefault { get; set; }
        public bool? IsTaxable { get; set; }
        public string TaxIdNumber { get; set; }
        public double PartTaxRate { get; set; }
        public double LaborTaxRate { get; set; }
        public string PartTaxRateDisplay
        {
            get
            {
                return PartTaxRate.ToString("#0.00#'%'");
            }
        }
        public string LaborTaxRateDisplay
        {
            get
            {
                return LaborTaxRate.ToString("#0.00#'%'");
            }
        }
    }
}
