using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class TechAmountValidator : AbstractValidator<TechAmountToWrite>
    {
        public TechAmountValidator()
        {
            RuleFor(techAmount => techAmount)
                .NotEmpty()
                .MustBeValueObject(
                    techAmount => TechAmount.Create(
                        techAmount.PayType,
                        techAmount.Amount,
                        techAmount.SkillLevel));
        }
    }
}
