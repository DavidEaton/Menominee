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
            // All validation should be done inside domain class: keep
            // the act of validation together with the act of object
            // creation. We do that in every case here except for 
            // organization.Note. Even tho in the next RuleFor(), we
            // are not trimming the Note before validating, which DOES
            // separate validation from object creation, it will only
            // reject a few edge cases. We did not create a separate
            // value object for organization.Note because it only has
            // a single invariant: Length. So we adhere to the guide-
            // line: create a value object class if its invariants > 1.
            // This is a minor concession, and acceptible compromise.

            // Optional members are validated seperately from the parent class
            // Since the transformation is extremely simple, do it here.
            Transform(organization => organization.Note, note => note.Trim());
            // Does organization.Note remain trimmed within successive rules?
            RuleFor(organization => organization.Note)
                .Length(0, 10000)
                .When(organization => organization.Note is not null);

            // Optional members are validated seperately from the parent class
            RuleFor(organization => organization.Contact)
                .SetValidator(new PersonValidator())
                .When(organization => organization.Contact is not null);

            // Optional members are validated seperately from the parent class
            RuleFor(organization => organization.Address)
                .SetValidator(new AddressValidator())
                .When(organization => organization.Address is not null);

            // Optional members are validated seperately from the parent class
            RuleFor(organization => organization.Emails)
                .SetValidator(new EmailsValidator())
                .When(organization => organization.Emails is not null);

            // Optional members are validated seperately from the parent class
            RuleFor(organization => organization.Phones)
                .SetValidator(new PhonesValidator())
                .When(organization => organization.Phones is not null);

            // Don't check name VO here when we create it in next rule.
            // It's duplicated effort.
            //RuleFor(organization => organization.Name)
            //    .MustBeValueObject(OrganizationName.Create);

            RuleFor(organization => organization)
                .MustBeEntity(
                    organization => Organization.Create(
                        OrganizationName.Create(organization.Name).Value,
                        organization.Note));
        }
    }
}
