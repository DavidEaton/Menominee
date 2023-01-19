using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Payments
{
    public class RepairOrderPaymentToRead
    {
        public long Id { get; set; }
        public long RepairOrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public double Amount { get; set; }
    }
}
