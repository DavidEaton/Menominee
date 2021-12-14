using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Shared.Validators
{
    public class DriversLicenseValidator : AbstractValidator<DriversLicenseToWrite>
    {
        public DriversLicenseValidator()
        {

            RuleFor(driversLicense => driversLicense)
                .NotEmpty()
                .MustBeValueObject(driversLicense => DriversLicense.Create(driversLicense.Number,
                                                                           driversLicense.State,
                                                                           DateTimeRange.Create(driversLicense.Issued,
                                                                                                driversLicense.Expiry).Value));
        }
    }
}
