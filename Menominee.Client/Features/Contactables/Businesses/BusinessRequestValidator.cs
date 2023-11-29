using FluentValidation;
using Menominee.Client.Features.Contactables.Addresses;
using Menominee.Client.Features.Contactables.Emails;
using Menominee.Client.Features.Contactables.Persons;
using Menominee.Client.Features.Contactables.Phones;
using Menominee.Domain.BaseClasses;
using Menominee.Shared.Models.Businesses;

namespace Menominee.Client.Features.Contactables.Businesses
{
    public class BusinessRequestValidator : AbstractValidator<BusinessToWrite>
    {
        public BusinessRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(business => business.Name)
                .SetValidator(new BusinessNameRequestValidator());

            RuleFor(business => business.Contact)
                .SetValidator(new PersonRequestValidator())
                .When(business => business.Contact is not null);

            RuleFor(business => business.Address)
                .SetValidator(new AddressRequestValidator())
                .When(business => business.Address is not null);

            RuleFor(business => business.Emails)
                .SetValidator(new EmailsRequestValidator())
                .When(business => business.Emails is not null);

            RuleFor(business => business.Phones)
                .SetValidator(new PhonesRequestValidator())
                .When(business => business.Phones is not null);

            RuleFor(business => business.Notes)
                .Length(1, Contactable.NoteMaximumLength)
                .WithMessage(Contactable.NoteMaximumLengthMessage)
                .When(business => !string.IsNullOrWhiteSpace(business.Notes));
        }
    }
}
