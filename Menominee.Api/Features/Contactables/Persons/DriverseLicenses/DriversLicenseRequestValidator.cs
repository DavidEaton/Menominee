using CSharpFunctionalExtensions;
using FluentValidation;
using Menominee.Common.ValueObjects;
using Menominee.Shared.Models.Persons.DriversLicenses;

namespace Menominee.Api.Features.Contactables.Persons.DriverseLicenses
{
    public class DriversLicenseRequestValidator : AbstractValidator<DriversLicenseToWrite>
    {
        public DriversLicenseRequestValidator()
        {
            RuleFor(driversLicense => driversLicense)
                .NotEmpty()
                .MustBeValueObject(
                driversLicense =>
                {
                    var rangeResult = DateTimeRange.Create(
                        driversLicense.Issued,
                        driversLicense.Expiry);

                    return rangeResult.IsFailure
                        ? Result.Failure<DriversLicense>(rangeResult.Error)
                        : DriversLicense.Create(
                            driversLicense.Number,
                            driversLicense.State,
                            rangeResult.Value
                        );
                });
        }
    }
}
