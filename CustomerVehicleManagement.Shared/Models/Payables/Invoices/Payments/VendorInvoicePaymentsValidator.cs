using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentsValidator : AbstractValidator<IList<VendorInvoicePaymentToWrite>>
    {
        private const string notEmptyMessage = "Payment must not be empty.";

        public VendorInvoicePaymentsValidator()
        {
            IList<string> paymentMethodNames = new List<string>();

            RuleFor(payments => payments)
                .NotNull()
                .ForEach(payment =>
                {
                    payment.NotEmpty().WithMessage(notEmptyMessage);
                    payment.MustBeEntity(
                        payment =>
                        VendorInvoicePayment.Create(
                            VendorInvoicePaymentMethod.Create(
                                paymentMethodNames,
                                payment.PaymentMethod.Name,
                                payment.PaymentMethod.IsActive,
                                payment.PaymentMethod.PaymentType,
                                payment.PaymentMethod.ReconcilingVendor is null
                                ? null
                                : Vendor.Create(
                                    payment.PaymentMethod.ReconcilingVendor.Name,
                                    payment.PaymentMethod.ReconcilingVendor.VendorCode,
                                    payment.PaymentMethod.ReconcilingVendor.VendorRole,
                                    payment.PaymentMethod.ReconcilingVendor.Notes)
                                    .Value).Value,
                            payment.Amount
                            ));
                });
        }
    }
}
