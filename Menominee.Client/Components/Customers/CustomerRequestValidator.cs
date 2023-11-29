using FluentValidation;
using Menominee.Client.Components.Vehicles;
using Menominee.Client.Features.Contactables.Businesses;
using Menominee.Client.Features.Contactables.Persons;
using Menominee.Common.Enums;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Customers;

namespace Menominee.Client.Components.Customers
{
    public class CustomerRequestValidator : AbstractValidator<CustomerToWrite>
    {
        public CustomerRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(customer => customer.EntityType)
                .IsInEnum();

            RuleFor(customer => customer.CustomerType)
                .IsInEnum();

            RuleFor(customer => customer.Person)
                .SetValidator(new PersonRequestValidator())
                .When(customer => customer.EntityType == EntityType.Person);

            RuleFor(customer => customer.Business)
                .SetValidator(new BusinessRequestValidator())
                .When(customer => customer.EntityType == EntityType.Business);

            RuleFor(customer => customer.Code)
                .MaximumLength(Customer.MaximumCodeLength);

            RuleFor(customer => customer.Vehicles)
                .SetValidator(new VehiclesRequestValidator())
                .When(customer => customer.Vehicles is not null);
        }
    }
}
