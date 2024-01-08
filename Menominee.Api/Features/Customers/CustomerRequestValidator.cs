using CSharpFunctionalExtensions;
using FluentValidation;
using Menominee.Api.Data;
using Menominee.Api.Features.Contactables.Businesses;
using Menominee.Api.Features.Contactables.Persons;
using Menominee.Api.Features.Vehicles;
using Menominee.Domain.Entities;
using Menominee.Domain.Enums;
using Menominee.Domain.Interfaces;
using Menominee.Domain.ValueObjects;
using Menominee.Shared.Models.Customers;
using System;

namespace Menominee.Api.Features.Customers;

public class CustomerRequestValidator : AbstractValidator<CustomerToWrite>
{
    private readonly ApplicationDbContext context;
    public CustomerRequestValidator(ApplicationDbContext context)
    {
        this.context = context ??
                throw new ArgumentNullException(nameof(context));

        ClassLevelCascadeMode = CascadeMode.Continue;

        RuleFor(customer => customer.EntityType)
            .IsInEnum()
            .WithMessage(Customer.UnknownEntityTypeMessage);

        RuleFor(customer => customer.CustomerType)
            .IsInEnum()
            .WithMessage(Customer.UnknownCustomerTypeMessage);

        RuleFor(customer => customer.Person)
            .SetValidator(new PersonRequestValidator(context))
            .When(customer => customer.EntityType == EntityType.Person && customer.Person is not null);

        RuleFor(customer => customer.Business)
            .SetValidator(new BusinessRequestValidator(context))
            .When(customer => customer.EntityType == EntityType.Business && customer.Business is not null);

        RuleFor(customer => customer.Code)
            .MaximumLength(Customer.MaximumCodeLength);

        RuleFor(customer => customer.Vehicles)
            .SetValidator(new VehiclesRequestValidator())
            .When(customer => customer.Vehicles is not null);

        RuleFor(customer => customer)
            .MustBeEntity(customer =>
                {
                    ICustomerEntity entity = null;

                    if (customer.EntityType == EntityType.Person && customer.Person is not null)
                    {
                        var personNameResult = PersonName.Create(customer.Person?.Name?.LastName, customer.Person?.Name?.FirstName, customer.Person?.Name?.MiddleName);
                        if (personNameResult.IsFailure)
                        {
                            return Result.Failure<Customer>(personNameResult.Error);
                        }

                        var personResult = Person.Create(
                            personNameResult.Value,
                            customer.Person.Notes,
                            customer.Person.Birthday);

                        if (personResult.IsFailure)
                        {
                            return Result.Failure<Customer>(personResult.Error);
                        }

                        entity = personResult.Value;
                    }
                    else if (customer.EntityType == EntityType.Business && customer.Business is not null)
                    {
                        var businessNameResult = BusinessName.Create(customer.Business?.Name?.Name);
                        if (businessNameResult.IsFailure)
                        {
                            return Result.Failure<Customer>(businessNameResult.Error);
                        }

                        var businessResult = Business.Create(
                            businessNameResult.Value,
                            customer.Business.Notes);

                        if (businessResult.IsFailure)
                        {
                            return Result.Failure<Customer>(businessResult.Error);
                        }

                        entity = businessResult.Value;
                    }

                    return Customer.Create(entity, customer.CustomerType, customer.Code);
                });
    }
}
