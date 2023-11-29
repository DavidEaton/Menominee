using FluentValidation;
using Menominee.Shared.Models.Inventory.InventoryItems.Labor;

namespace Menominee.Api.Features.Inventory.InventoryItems.Labor
{
    public class InventoryItemLaborValidator : AbstractValidator<InventoryItemLaborToWrite>
    {
        public InventoryItemLaborValidator()
        {
            RuleFor(laborDto => laborDto.LaborAmount)
                .SetValidator(new LaborAmountValidator());

            RuleFor(laborDto => laborDto.TechAmount)
                .SetValidator(new TechAmountValidator());
        }
    }
}
