using FluentValidation;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.SellingPriceNames;

namespace Menominee.Api.Features.SellingPriceNames
{
    public class SellingPriceNameValidator : AbstractValidator<SellingPriceNameToWrite>
    {
        public SellingPriceNameValidator()
        {
            RuleFor(sellingPriceName => sellingPriceName)
                .MustBeEntity(sellingPriceName =>
                    SellingPriceName.Create(sellingPriceName.Name));
        }
    }
}