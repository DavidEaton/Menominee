using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Payments
{
    public class RepairOrderPaymentToWrite
    {
        public long Id { get; set; } = 0;
        public long RepairOrderId { get; set; } = 0;
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
        public double Amount { get; set; } = 0.0;
    }
}
