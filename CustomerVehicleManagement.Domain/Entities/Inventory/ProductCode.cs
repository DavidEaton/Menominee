using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class ProductCode : Entity
    {
        public Manufacturer Manufacturer { get; set; }
        public long ManufacturerId { get; set; }
        public string Code { get; set; }
        public SaleCode SaleCode { get; set; }
        public long SaleCodeId { get; set; }
        public string Name { get; set; }

        #region ORM

        // EF requires a parameterless constructor
        public ProductCode() { }

        #endregion    
    }
}
