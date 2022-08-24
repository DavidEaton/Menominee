using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Validators;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorValidator : AbstractValidator<VendorToWrite>
    {
        public VendorValidator()
        {
            RuleFor(vendor => vendor)
                .MustBeEntity(
                    vendor => Vendor.Create(
                        vendor.Name,
                        vendor.VendorCode));
        }
    }
}
