using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class RepairOrderTech : Entity
    {
        public long RepairOrderServiceId { get; set; }
        public long TechnicianId { get; set; }

        #region ORM

        // EF requires a parameterless constructor
        public RepairOrderTech() { }

        #endregion
    }
}
