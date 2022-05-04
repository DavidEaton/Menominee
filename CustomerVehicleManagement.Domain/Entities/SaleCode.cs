using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class SaleCode : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double LaborRate { get; set; }
        public double DesiredMargin { get; set; }

        //public virtual SaleCodeShopSupplies ShopSupplies { get; set; }
        public double ShopSuppliesPercentage { get; set; }
        public double ShopSuppliesMinimumJobAmount { get; set; }
        public double ShopSuppliesMinimumCharge { get; set; }
        public double ShopSuppliesMaximumCharge { get; set; }
        public bool ShopSuppliesIncludeParts { get; set; }
        public bool ShopSuppliesIncludeLabor { get; set; }


        // TODO - Should royalty be split out???

        #region ORM

        // EF requires an empty constructor
        public SaleCode() { }

        #endregion 
    }
}
