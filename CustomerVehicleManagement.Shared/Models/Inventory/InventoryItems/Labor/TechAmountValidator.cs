using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor
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
