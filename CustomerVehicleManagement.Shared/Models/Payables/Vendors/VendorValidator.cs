using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorValidator : AbstractValidator<VendorToWrite>
    {
        public VendorValidator()
        {
            // TODO: David, do we need to add any more fields here?  Notes, DefaultPaymentMethod, Address, etc.?
            RuleFor(vendor => vendor)
                .MustBeEntity(
                    vendor => Vendor.Create(
                        vendor.Name,
                        vendor.VendorCode));
        }
    }
}
