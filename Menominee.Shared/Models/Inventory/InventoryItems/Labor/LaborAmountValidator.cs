using Menominee.Domain.Entities.Inventory;
using FluentValidation;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Labor
{
    public class LaborAmountValidator : AbstractValidator<LaborAmountToWrite>
    {
        public LaborAmountValidator()
        {
            RuleFor(laborAmount => laborAmount)
                .MustBeValueObject(
                    laborAmount => LaborAmount.Create(
                        laborAmount.PayType,
                        laborAmount.Amount));
        }
    }
}
