using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class RepairOrderSerialNumber : Entity
    {
        // DDD Notes:
        // Rename this class to RepairOrderItemSerialNumber
        public long RepairOrderItemId { get; set; }
        public string SerialNumber { get; set; }

        #region ORM

        // EF requires a parameterless constructor
        public RepairOrderSerialNumber() { }

        #endregion
    }
}
