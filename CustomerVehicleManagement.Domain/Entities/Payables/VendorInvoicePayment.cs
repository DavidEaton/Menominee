using CSharpFunctionalExtensions;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoicePayment : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly double InvalidValue = 0;
        public static readonly string InvalidValueMessage = $"Value cannot be zero.";

        public VendorInvoicePaymentMethod PaymentMethod { get; private set; }
        public double Amount { get; private set; }

        private VendorInvoicePayment(VendorInvoicePaymentMethod paymentMethod, double amount)
        {
            if (paymentMethod is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (amount == InvalidValue)
                throw new ArgumentOutOfRangeException(InvalidValueMessage);

            PaymentMethod = paymentMethod;
            Amount = amount;
        }

        public static Result<VendorInvoicePayment> Create(VendorInvoicePaymentMethod paymentMethod, double amount)
        {
            if (paymentMethod is null)
                return Result.Failure<VendorInvoicePayment>(RequiredMessage);

            if (amount == InvalidValue)
                return Result.Failure<VendorInvoicePayment>(InvalidValueMessage);

            return Result.Success(new VendorInvoicePayment(paymentMethod, amount));
        }

        public Result<VendorInvoicePaymentMethod> SetPaymentMethod(VendorInvoicePaymentMethod paymentMethod)
        {
            if (paymentMethod is null)
                return Result.Failure<VendorInvoicePaymentMethod>(RequiredMessage);

            return Result.Success(PaymentMethod = paymentMethod);
        }

        public Result<double> SetAmount(double amount)
        {
            if (amount == InvalidValue)
                return Result.Failure<double>(InvalidValueMessage);

            return Result.Success(Amount = amount);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected VendorInvoicePayment() { }

        #endregion
    }
}
