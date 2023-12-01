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

            var resultName = RuleFor(business => business.Name)
                .NotEmpty()
                .WithMessage(Contactable.RequiredMessage)
                .DependentRules(() =>
                {
                    RuleFor(business => business.Name)
                        .SetValidator(new BusinessNameRequestValidator());
                });

            var resultContact = RuleFor(business => business.Contact)
                .SetValidator(new PersonRequestValidator())
                .When(business => business.Contact is not null);

            var resultAddress = RuleFor(business => business.Address)
                .SetValidator(new AddressRequestValidator())
                .When(business => business.Address is not null);

            var resultEmails = RuleFor(business => business.Emails)
                .SetValidator(new EmailsRequestValidator())
                .When(business => business.Emails is not null);

            var resultPhones = RuleFor(business => business.Phones)
                .SetValidator(new PhonesRequestValidator())
                .When(business => business.Phones is not null);

            var resultNotes = RuleFor(business => business.Notes)
                .Length(1, Contactable.NoteMaximumLength)
                .WithMessage(Contactable.NoteMaximumLengthMessage)
                .When(business => !string.IsNullOrWhiteSpace(business.Notes));
        }
    }
}
