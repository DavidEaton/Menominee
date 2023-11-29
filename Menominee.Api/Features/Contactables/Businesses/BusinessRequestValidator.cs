using CSharpFunctionalExtensions;
using FluentValidation;
using Menominee.Api.Data;
using Menominee.Api.Features.Contactables.Addresses;
using Menominee.Api.Features.Contactables.Emails;
using Menominee.Api.Features.Contactables.Persons;
using Menominee.Api.Features.Contactables.Phones;
using Menominee.Common.ValueObjects;
using Menominee.Domain.BaseClasses;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Businesses;
using System;

namespace Menominee.Api.Features.Contactables.Businesses
{
    public class BusinessRequestValidator : AbstractValidator<BusinessToWrite>
    {
        private readonly ApplicationDbContext context;
        public BusinessRequestValidator(ApplicationDbContext context)
        {
            this.context = context ??
                throw new ArgumentNullException(nameof(context));

            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(business => business.Name)
                .SetValidator(new BusinessNameRequestValidator());

            RuleFor(business => business.Contact)
                .SetValidator(new PersonRequestValidator(context))
                .When(business => business.Contact is not null && business.Contact.IsNotEmpty);

            RuleFor(business => business.Address)
                .SetValidator(new AddressRequestValidator())
                .When(business => business.Address is not null);

            RuleFor(business => business.Emails)
                .SetValidator(new EmailsRequestValidator(context))
                .When(business => business.Emails is not null);

            RuleFor(business => business.Phones)
                .SetValidator(new PhonesRequestValidator())
                .When(business => business.Phones is not null);

            RuleFor(business => business)
                .MustBeEntity(business =>
                {
                    if (business.Name is null)
                    {
                        return Result.Failure<Business>(Contactable.RequiredMessage);
                    }

                    var nameResult = BusinessName.Create(business.Name.Name);

                    return nameResult.IsFailure
                        ? Result.Failure<Business>(nameResult.Error)
                        : Business.Create(nameResult.Value, business.Notes);
                });
        }
    }
}
