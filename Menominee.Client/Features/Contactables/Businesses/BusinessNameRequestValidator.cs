using FluentValidation;
using Menominee.Domain.ValueObjects;
using Menominee.Shared.Models.Businesses;

namespace Menominee.Client.Features.Contactables.Businesses
{
    public class BusinessNameRequestValidator : AbstractValidator<BusinessNameRequest>
    {
        public BusinessNameRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(name => name.Name)
                .NotEmpty()
                .WithMessage(BusinessName.RequiredMessage)
                .Length(BusinessName.MinimumLength, BusinessName.MaximumLength)
                .WithMessage(BusinessName.InvalidLengthMessage);
        }
    }
}