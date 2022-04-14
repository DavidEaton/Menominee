using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class SaleCode : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        #region ORM

        // EF requires an empty constructor
        public SaleCode() { }

        #endregion 
    }
}
