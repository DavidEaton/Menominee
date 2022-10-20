using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using CustomerVehicleManagement.Shared.Models.Persons;
using FluentValidation;
using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Shared.Models.Organizations
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

            // All validation should be done inside domain class: keep
            // the act of validation together with the act of object
            // creation. We do that in every case here excpet for 
            // organization.Note. Even tho in the next RuleFor(), we
            // are not trimming the Note before validating, which DOES
            // separate validation from object creation, it will only
            // reject a few edge cases. We did not create a separate
            // value object for organization.Note because it only has
            // a single invariant: Length. So we adhere to the guide-
            // line: create a value object class if invariants > 1.
            // This is a minor concession, and acceptible compromise.
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
