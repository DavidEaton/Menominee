using CustomerVehicleManagement.Domain.Entities.Payables;
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
