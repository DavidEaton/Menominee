using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class Manufacturer : Entity
    {
        public string Code { get; set; }
        public string Prefix { get; set; }
        public string Name { get; set; }
        //public xxx Country { get; set; }
        //public xxx Franchise { get; set; }

        #region ORM

        // EF requires an empty constructor
        public Manufacturer() { }

        #endregion

    }
}
