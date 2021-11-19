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
                                                .MustBeValueObject(OrganizationName.Create);

            RuleFor(organization => organization.Address)
                                                .NotEmpty()
                                                .MustBeValueObject(address => Address.Create(address.AddressLine,
                                                                                             address.City,
                                                                                             address.State,
                                                                                             address.PostalCode))
                                                .When(organization => organization.Address != null);

            RuleFor(organization => organization.Note)
                                                  .NotEmpty()
                                                  .Length(0, 10000)
                                                  .When(organization => organization.Note != null);

            RuleFor(organization => organization.Emails)
                .SetValidator(new EmailsToAddValidator());

            RuleFor(organization => organization.Phones)
                .SetValidator(new PhonesToAddValidator());

            RuleFor(organization => organization.Contact)
                .SetValidator(new PersonToAddValidator())
                .When(organization => organization.Contact != null);
        }
    }
}
