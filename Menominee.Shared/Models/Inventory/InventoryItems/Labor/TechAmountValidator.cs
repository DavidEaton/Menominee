using Menominee.Domain.Entities.Inventory;
using FluentValidation;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Labor
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
