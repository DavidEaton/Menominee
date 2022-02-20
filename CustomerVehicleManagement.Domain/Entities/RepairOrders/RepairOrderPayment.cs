using Menominee.Common;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class RepairOrderPayment : Entity
    {
        public long RepairOrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public double Amount { get; set; }

        #region ORM

        // EF requires an empty constructor
        public RepairOrderPayment() { }

        #endregion
    }
}
