using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemPartValidator : AbstractValidator<InventoryItemPartToWrite>
    {
        public InventoryItemPartValidator()
        {
            RuleFor(partDto => partDto.TechAmount)
                .SetValidator(new TechAmountValidator());

            RuleFor(partDto => partDto)
                .MustBeEntity(
                    partDto => InventoryItemPart.Create(
                        partDto.List,
                        partDto.Cost,
                        partDto.Core,
                        partDto.Retail,
                        TechAmount.Create(
                        partDto.TechAmount.PayType,
                        partDto.TechAmount.Amount,
                        partDto.TechAmount.SkillLevel).Value,
                        partDto.Fractional,
                        partDto.LineCode,
                        partDto.SubLineCode));
        }
    }
}
