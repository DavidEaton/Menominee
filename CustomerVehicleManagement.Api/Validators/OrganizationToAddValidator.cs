using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Api.Validators
{
    public class OrganizationToAddValidator : AbstractValidator<OrganizationToAdd>
    {
        public OrganizationToAddValidator()
        {
            RuleFor(organization => organization.Name)
                                                .NotEmpty()
                                                .Length(2, 255);

            RuleFor(organization => organization.Name)
                                                .MustBeValueObject(OrganizationName.Create)
                                                .When(organization => organization.Name != null);

            //RuleFor(organization => organization.Address)
            //                                    .MustBeValueObject(x => Address.Create(x.AddressLine, x.City, x.State, x.PostalCode))
            //                                    .When(address => address != null);

            RuleFor(organization => organization.Note)
                                                .Length(0, 10000)
                                                .When(organization => organization.Note != null);

            RuleFor(organization => organization.Emails)
                .SetValidator(new EmailsValidator());
            RuleFor(organization => organization.Phones)
                .SetValidator(new PhonesValidator());
        }
    }
}
