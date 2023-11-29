using FluentValidation;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using System.Collections.Generic;

namespace Menominee.Api.Features.Payables.PaymentMethods
{
    public class VendorInvoicePaymentsValidator : AbstractValidator<IList<VendorInvoicePaymentToWrite>>
    {
        private const string notEmptyMessage = "Payment must not be empty.";

        public VendorInvoicePaymentsValidator()
        {
            IReadOnlyList<string> paymentMethodNames = new List<string>();

            RuleFor(payments => payments)
                .NotEmpty()
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
