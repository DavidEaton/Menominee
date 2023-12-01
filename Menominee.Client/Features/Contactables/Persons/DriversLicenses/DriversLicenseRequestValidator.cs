using FluentValidation;
using Menominee.Common.ValueObjects;
using Menominee.Shared.Models.Persons.DriversLicenses;

namespace Menominee.Client.Features.Contactables.Persons.DriversLicenses
{
    public class DriversLicenseRequestValidator : AbstractValidator<DriversLicenseToWrite>
    {
        public DriversLicenseRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(driversLicense => driversLicense.Number)
                .NotEmpty().WithMessage(DriversLicense.RequiredMessage)
                .MaximumLength(DriversLicense.MaximumLength)
                .WithMessage(DriversLicense.OverMaximumLengthMessage)
                .MinimumLength(DriversLicense.MinimumLength)
                .WithMessage(DriversLicense.UnderMinimumLengthMessage);

            RuleFor(driversLicense => driversLicense.State)
                .IsInEnum().WithMessage(DriversLicense.StateInvalidMessage);

            RuleFor(driversLicense => driversLicense)
                .Must(ValidDateRange)
                .WithMessage(DriversLicense.DateRangeInvalidMessage);
        }

        private bool ValidDateRange(DriversLicenseToWrite driversLicense) =>
            driversLicense.Issued != DateTime.MinValue
            &&
            driversLicense.Expiry != DateTime.MinValue
            &&
            driversLicense.Issued <= driversLicense.Expiry;
    }
}
