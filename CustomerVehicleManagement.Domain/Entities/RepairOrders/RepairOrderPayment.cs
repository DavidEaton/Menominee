using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    // TODO: DDD: Rename this class to TicketPayment
    public class RepairOrderPayment : Entity
    {
        public static readonly string RequiredMessage = "Please include all required items.";

        public PaymentMethod PaymentMethod { get; private set; }
        public double Amount { get; private set; }

        private RepairOrderPayment(PaymentMethod paymentMethod, double amount)
        {
            PaymentMethod = paymentMethod;
            Amount = amount;
        }

        public static Result<RepairOrderPayment> Create(PaymentMethod paymentMethod, double amount)
        {
            if (!Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
                return Result.Failure<RepairOrderPayment>(RequiredMessage);

            return Result.Success(new RepairOrderPayment(paymentMethod, amount));
        }

        public Result<PaymentMethod> SetPaymentMethod(PaymentMethod paymentMethod)
        {
            if (!Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
                return Result.Failure<PaymentMethod>(RequiredMessage);

            return Result.Success(PaymentMethod = paymentMethod);
        }

        public Result<double> SetAmount(double amount)
        {
            return Result.Success(Amount = amount);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected RepairOrderPayment() { }

        #endregion
    }
}
