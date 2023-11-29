using FluentValidation;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems.Labor;

namespace Menominee.Api.Features.Inventory.InventoryItems.Labor
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
