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
            //                                    .MustBeValueObject(Address.Create)
            //                                    .When(address => address != null);

            Transform(organization => organization.Note, organization => (organization ?? "")
                                                  .Trim())
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
