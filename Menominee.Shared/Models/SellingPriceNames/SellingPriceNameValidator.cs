using FluentValidation;
using Menominee.Domain.Entities.Inventory;

namespace Menominee.Shared.Models.SellingPriceNames;

public class SellingPriceNameValidator : AbstractValidator<SellingPriceNameToWrite>
{
    public SellingPriceNameValidator()
    {
        RuleFor(sellingPriceName => sellingPriceName)
            .MustBeEntity(sellingPriceName =>
                SellingPriceName.Create(sellingPriceName.Name));
    }
}
