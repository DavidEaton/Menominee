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

            //RuleFor(organization => organization.Address)
            //                                    .MustBeValueObject(x => Address.Create(x.AddressLine, x.City, x.State, x.PostalCode))
            //                                    .When(address => address != null);

            RuleFor(organization => organization.Note)
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
