using FluentValidation;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Persons;

namespace Menominee.Shared.Models.Customers
{
    public class CustomerValidator : AbstractValidator<CustomerToWrite>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.EntityType)
                .IsInEnum();

            RuleFor(customer => customer.CustomerType)
                .IsInEnum();

            RuleFor(customer => customer.Person)
                .SetValidator(new PersonValidator())
                .When(customer => customer.EntityType == EntityType.Person);

            RuleFor(customer => customer.Business)
                .SetValidator(new BusinessValidator())
                .When(customer => customer.EntityType == EntityType.Business);

            RuleFor(customer => customer)
                 .MustBeEntity(customer =>
                     Customer.Create(
                         Person.Create(
                             PersonName.Create(
                                 customer.Person.Name.LastName, customer.Person.Name.FirstName).Value,
                                 customer.Person.Gender,
                                 customer.Person.Notes).Value,
                             customer.CustomerType))
                 .When(customer => customer.EntityType == EntityType.Person);

            RuleFor(customer => customer)
                .MustBeEntity(customer =>
                    Customer.Create(
                        Business.Create(
                            BusinessName.Create(customer.Business.Name).Value, customer.Business.Notes).Value,
                            customer.CustomerType))
                .When(customer => customer.EntityType == EntityType.Business);
        }
    }
}
