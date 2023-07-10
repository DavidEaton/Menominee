using Menominee.Domain.Entities;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Persons;
using FluentValidation;
using Menominee.Common.ValueObjects;

namespace Menominee.Shared.Models.Organizations
{
    public class OrganizationValidator : AbstractValidator<OrganizationToWrite>
    {
        public OrganizationValidator()
        {
            // All validation should be done inside domain class: keep
            // the act of validation together with the act of object
            // creation. Aka parsing. Validation == parsing.

            // Optional members are validated seperately from the parent class
            RuleFor(organization => organization.Contact)
                .SetValidator(new PersonValidator())
                .When(organization => organization.Contact is not null);

            RuleFor(organization => organization.Address)
                .SetValidator(new AddressValidator())
                .When(organization => organization.Address is not null);

            RuleFor(organization => organization.Emails)
                .SetValidator(new EmailsValidator())
                .When(organization => organization.Emails is not null);

            RuleFor(organization => organization.Phones)
                .SetValidator(new PhonesValidator())
                .When(organization => organization.Phones is not null);

            RuleFor(organization => organization)
                .MustBeEntity(organization =>
                    Organization.Create(
                        OrganizationName.Create(organization.Name).Value,
                        organization.Notes));
        }
    }
}
