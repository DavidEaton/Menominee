using CSharpFunctionalExtensions;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoicePayment : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly double MinimumValue = 0;
        public static readonly string MinimumValueMessage = $"Value(s) cannot be negative.";

        public VendorInvoicePaymentMethod PaymentMethod { get; private set; }
        public double Amount { get; private set; }

        private VendorInvoicePayment(VendorInvoicePaymentMethod paymentMethod, double amount)
        {
            PaymentMethod = paymentMethod;
            Amount = amount;
        }

        public static Result<VendorInvoicePayment> Create(VendorInvoicePaymentMethod paymentMethod, double amount)
        {
            if (paymentMethod is null)
                return Result.Failure<VendorInvoicePayment>(RequiredMessage);

            if (amount < MinimumValue)
                return Result.Failure<VendorInvoicePayment>(MinimumValueMessage);

            return Result.Success(new VendorInvoicePayment(paymentMethod, amount));

        }

        #region ORM

        // EF requires an empty constructor
        public VendorInvoicePayment() { }

        #endregion
    }
}
