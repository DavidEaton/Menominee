using FluentValidation;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems.Labor;

namespace Menominee.Api.Features.Inventory.InventoryItems.Labor
{
    public class TechAmountValidator : AbstractValidator<TechAmountToWrite>
    {
        public TechAmountValidator()
        {
            RuleFor(techAmount => techAmount)
                .MustBeValueObject(
                    techAmount => TechAmount.Create(
                        techAmount.PayType,
                        techAmount.Amount,
                        techAmount.SkillLevel));
        }
    }
}
