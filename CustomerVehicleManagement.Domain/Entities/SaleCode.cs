using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class SaleCode : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double LaborRate { get; set; }
        public double DesiredMargin { get; set; }

        public SaleCodeShopSupplies ShopSupplies { get; set; }

        // TODO - Should royalty be split out???

        #region ORM

        // EF requires an empty constructor
        public SaleCode() { }

        #endregion 
    }
}
