using CSharpFunctionalExtensions;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoicePayment : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly double InvalidValue = 0;
        public static readonly string InvalidValueMessage = $"Value cannot be zero.";

        public VendorInvoicePaymentMethod PaymentMethod { get; private set; }
        public double Amount { get; private set; } // <> 0, can be negative

        private VendorInvoicePayment(VendorInvoicePaymentMethod paymentMethod, double amount)
        {
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

        #region ORM

        // EF requires an empty constructor
        public VendorInvoicePayment() { }

        #endregion
    }
}
