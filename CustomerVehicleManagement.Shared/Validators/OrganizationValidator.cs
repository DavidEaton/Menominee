using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using CustomerVehicleManagement.Shared.Models.Persons;
using FluentValidation;
using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Shared.Validators
{
    public class OrganizationValidator : AbstractValidator<OrganizationToWrite>
    {
        public OrganizationValidator()
        {
            RuleFor(organization => organization.Name)
                .MustBeValueObject(OrganizationName.Create);

            RuleFor(organization => organization.Address)
                .SetValidator(new AddressValidator())
                .When(organization => organization.Address is not null);

            RuleFor(organization => organization.Note)
                .Length(0, 10000)
                .When(organization => organization.Note is not null);

            RuleFor(organization => organization.Emails)
                .NotNull()
                .SetValidator(new EmailsValidator());

            RuleFor(organization => organization.Phones)
                .NotNull()
                .SetValidator(new PhonesValidator());

            RuleFor(organization => organization.Contact)
                .SetValidator(new PersonValidator())
                .When(organization => organization.Contact is not null);

            RuleFor(organization => organization)
                .MustBeEntity(
                    organization => Organization.Create(
                        OrganizationName.Create(organization.Name).Value,
                        organization.Note));
        }
    }
}
