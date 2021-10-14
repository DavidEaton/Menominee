using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Validators
{
    public class OrganizationToAddValidator : AbstractValidator<OrganizationToAdd>
    {
        public OrganizationToAddValidator()
        {
            RuleFor(organization => organization.Name).NotEmpty().Length(2, 255);
            RuleFor(organization => organization.Note).Length(0, 10000).When(organization => organization.Note != null);
            RuleFor(organization => organization.Address).SetValidator(new AddressValidator());
            RuleFor(organization => organization.Emails).SetValidator(new EmailToAddValidator());
            RuleFor(organization => organization.Phones).SetValidator(new PhonesValidator());

        }


    }
}
