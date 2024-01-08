using FluentValidation;
using Menominee.Domain.Entities.Payables;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Payables.Invoices.Payments;

namespace Menominee.Client.Components.Settings
{
    public class VendorInvoicePaymentMethodRequestValidator : AbstractValidator<VendorInvoicePaymentMethodRequest>
    {
        public VendorInvoicePaymentMethodRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(vendorInvoicePaymentMethod => vendorInvoicePaymentMethod.Name)
                .NotEmpty()
                .Must(name => name.Length >= VendorInvoicePaymentMethod.MinimumLength && name.Length <= VendorInvoicePaymentMethod.MaximumLength)
                    .WithMessage(VendorInvoicePaymentMethod.InvalidLengthMessage);

            RuleFor(vendorInvoicePaymentMethod => vendorInvoicePaymentMethod.PaymentType)
                .Must(paymentType => Enum.IsDefined(typeof(VendorInvoicePaymentMethodType), paymentType))
                    .WithMessage(VendorInvoicePaymentMethod.RequiredMessage);

            RuleFor(vendorInvoicePaymentMethod => vendorInvoicePaymentMethod.ReconcilingVendor)
                .NotNull()
                .When(vendorInvoicePaymentMethod => vendorInvoicePaymentMethod.ReconcilingVendor is not null)
                .WithMessage(VendorInvoicePaymentMethod.RequiredMessage);
        }
    }
}
