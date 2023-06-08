using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorValidator : AbstractValidator<VendorToWrite>
    {
        public VendorValidator()
        {
            // Optional members are validated seperately from the parent class
            RuleFor(vendor => vendor.Address)
                .SetValidator(new AddressValidator())
                .When(vendor => vendor.Address is not null);

            RuleFor(vendor => vendor.Emails)
                .SetValidator(new EmailsValidator())
                .When(vendor => vendor.Emails is not null);

            RuleFor(vendor => vendor.Phones)
                .SetValidator(new PhonesValidator())
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
