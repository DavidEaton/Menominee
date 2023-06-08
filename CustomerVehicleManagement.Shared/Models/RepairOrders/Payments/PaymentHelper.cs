using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Payments
{
    public class PaymentHelper
    {
        public static List<RepairOrderPaymentToWrite> CovertReadToWriteDtos(List<RepairOrderPaymentToRead> payments)
        {
            return payments?.Select(
                payment =>
                new RepairOrderPaymentToWrite()
                {
                    Id = payment.Id,
                    PaymentMethod = payment.PaymentMethod,
                    Amount = payment.Amount

                }).ToList()
            ?? new List<RepairOrderPaymentToWrite>();
        }

        public static List<RepairOrderPaymentToRead> ConvertToReadDtos(IReadOnlyList<RepairOrderPayment> payments)
        {
            return payments?.Select(
                payment =>
                new RepairOrderPaymentToRead()
                {
                    Amount = payment.Amount,
                    Id = payment.Id,
                    PaymentMethod = payment.PaymentMethod
                }).ToList()
            ?? new List<RepairOrderPaymentToRead>();
        }

        public static List<RepairOrderPayment> ConvertWriteDtosToEntities(IReadOnlyList<RepairOrderPaymentToWrite> payments)
        {
            return payments?.Select(
                payment =>
                RepairOrderPayment.Create(
                    payment.PaymentMethod,
                    payment.Amount).Value)
                .ToList()
            ?? new List<RepairOrderPayment>();
        }
    }
}
