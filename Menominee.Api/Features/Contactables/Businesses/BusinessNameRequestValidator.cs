using FluentValidation;
using Menominee.Common.ValueObjects;
using Menominee.Shared.Models.Businesses;

namespace Menominee.Api.Features.Contactables.Businesses
{
    public class BusinessNameRequestValidator : AbstractValidator<BusinessNameRequest>
    {
        public BusinessNameRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(businessName => businessName)
                .NotEmpty()
                .MustBeValueObject(
                    businessName => BusinessName.Create(
                    businessName.Name));
        }
    }
}
