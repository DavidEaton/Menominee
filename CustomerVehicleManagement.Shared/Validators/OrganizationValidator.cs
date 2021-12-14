using CustomerVehicleManagement.Shared.Models;
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

            //APPLICATION SHOULD USE MustBeValueObject TO VALIDATE ADDRESS VALUEOBJECT
            // MustBeValueObject VALIDATOR WORKS GREAT WITH API BUT BLAZOR ValidationMessage IS MISSING
            RuleFor(organization => organization.Address)
                                                .NotEmpty()
                                                .MustBeValueObject(address => Address.Create(address.AddressLine,
                                                                                             address.City,
                                                                                             address.State,
                                                                                             address.PostalCode))
                                                .When(organization => organization.Address != null);

            //RuleFor(organization => organization.Address)
            //    .SetValidator(new AddressToWriteValidator())
            //    .When(organization => organization.Address != null);

            RuleFor(organization => organization.Note)
                .Length(0, 10000)
                .When(organization => organization.Note != null);

            RuleFor(organization => organization.Emails)
                .NotNull()
                .SetValidator(new EmailsValidator());

            RuleFor(organization => organization.Phones)
                .NotNull()
                .SetValidator(new PhonesValidator());

            RuleFor(organization => organization.Contact)
                .SetValidator(new PersonValidator())
                .When(organization => organization.Contact != null);
        }
    }

}
