using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class ProductCode : Entity
    {
        public virtual Manufacturer Manufacturer { get; set; }
        public string Code { get; set; }
        public virtual SaleCode SaleCode { get; set; }
        public string Name { get; set; }

        #region ORM

        // EF requires an empty constructor
        public ProductCode() { }

        #endregion    
    }
}
