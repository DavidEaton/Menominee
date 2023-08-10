using Menominee.Domain.Entities;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Persons;
using FluentValidation;
using Menominee.Common.ValueObjects;

namespace Menominee.Shared.Models.Businesses
{
    public class BusinessValidator : AbstractValidator<BusinessToWrite>
    {
        public BusinessValidator()
        {
            // All validation should be done inside domain class: keep
            // the act of validation together with the act of object
            // creation. Aka parsing. Validation == parsing.

            // Optional members are validated seperately from the parent class
            RuleFor(business => business.Contact)
                .SetValidator(new PersonValidator())
                .When(business => business.Contact is not null);

            RuleFor(business => business.Address)
                .SetValidator(new AddressValidator())
                .When(business => business.Address is not null);

            RuleFor(business => business.Emails)
                .SetValidator(new EmailsValidator())
                .When(business => business.Emails is not null);

            RuleFor(business => business.Phones)
                .SetValidator(new PhonesValidator())
                .When(business => business.Phones is not null);

            RuleFor(business => business)
                .MustBeEntity(business =>
                    Business.Create(
                        BusinessName.Create(business.Name).Value,
                        business.Notes));
        }
    }
}
