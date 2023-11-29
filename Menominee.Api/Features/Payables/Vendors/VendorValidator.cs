using FluentValidation;
using Menominee.Api.Data;
using Menominee.Api.Features.Contactables.Addresses;
using Menominee.Api.Features.Contactables.Emails;
using Menominee.Api.Features.Contactables.Phones;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Vendors;
using System;

namespace Menominee.Api.Features.Payables.Vendors
{
    public class VendorValidator : AbstractValidator<VendorToWrite>
    {
        private readonly ApplicationDbContext context;

        public VendorValidator(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));

            ClassLevelCascadeMode = CascadeMode.Continue;

            // Optional members are validated seperately from the parent class
            RuleFor(vendor => vendor.Address)
                .SetValidator(new AddressRequestValidator())
                .When(vendor => vendor.Address is not null);

            RuleFor(vendor => vendor.Emails)
                .SetValidator(new EmailsRequestValidator(context))
                .When(vendor => vendor.Emails is not null);

            RuleFor(vendor => vendor.Phones)
                .SetValidator(new PhonesRequestValidator())
                .When(vendor => vendor.Phones is not null);

            // May be better to inject DefaultPaymentMethod respoitory to get
            // that entity, which when successful, validates the DefaultPaymentMethodToRead
            // dto, for aggregate root validation completeness.
            // HOWEVER, that would create a circular dependency from this project to API
            // and back again. So we miss an edge case if the client somehow sends a
            // DefaultPaymentMethod read dto that represents a non-existent entity
            DefaultPaymentMethod defaultPaymentMethod = null;

            RuleFor(vendor => vendor)
                .MustBeEntity(
                    vendor => Vendor.Create(
                        vendor.Name,
                        vendor.VendorCode,
                        vendor.VendorRole,
                        vendor.Notes,
                        defaultPaymentMethod
                        ));
        }
    }
}
