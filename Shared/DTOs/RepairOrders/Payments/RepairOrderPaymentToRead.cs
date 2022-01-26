using MenomineePlayWASM.Shared.Entities.RepairOrders;
using MenomineePlayWASM.Shared.Entities.RepairOrders.Enums;
using System.Collections.Generic;
using System.Linq;

namespace MenomineePlayWASM.Shared.Dtos.RepairOrders.Payments
{
    public class RepairOrderPaymentToRead
    {
        public long Id { get; set; }
        public long RepairOrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public double Amount { get; set; }

        public static IReadOnlyList<RepairOrderPaymentToRead> ConvertToDto(IList<RepairOrderPayment> payments)
        {
            return payments
                .Select(payment =>
                        ConvertToDto(payment))
                .ToList();
        }

        private static RepairOrderPaymentToRead ConvertToDto(RepairOrderPayment payment)
        {
            if (payment != null)
            {
                return new RepairOrderPaymentToRead()
                {
                    Id = payment.Id,
                    RepairOrderId = payment.RepairOrderId,
                    PaymentMethod = payment.PaymentMethod,
                    Amount = payment.Amount
                };
            }

            return null;
        }
    }
}
