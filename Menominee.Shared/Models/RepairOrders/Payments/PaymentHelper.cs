using Menominee.Domain.Entities.RepairOrders;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.RepairOrders.Payments
{
    public class PaymentHelper
    {
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

        public static List<RepairOrderPaymentToWrite> ConvertToWriteDtos(List<RepairOrderPayment> payments)
        {
            return payments?.Select(
                payment =>
                new RepairOrderPaymentToWrite()
                {
                    Id = payment.Id,
                    Amount = payment.Amount,
                    PaymentMethod = payment.PaymentMethod
                }).ToList()
            ?? new List<RepairOrderPaymentToWrite>();
        }

        public static List<RepairOrderPaymentToWrite> ConvertReadToWriteDtos(List<RepairOrderPaymentToRead> payments)
        {
            return payments?.Select(
                payment =>
                new RepairOrderPaymentToWrite()
                {
                    Id = payment.Id,
                    Amount = payment.Amount,
                    PaymentMethod = payment.PaymentMethod
                }).ToList()
            ?? new List<RepairOrderPaymentToWrite>();
        }
    }
}
