using FluentValidation;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems.Part;

namespace Menominee.Api.Features.Inventory.InventoryItems.Part
{
    public class InventoryItemPartValidator : AbstractValidator<InventoryItemPartToWrite>
    {
        public InventoryItemPartValidator()
        {
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
                            partDto.TechAmount.SkillLevel)
                        .Value,
                        partDto.Fractional,
                        partDto.LineCode,
                        partDto.SubLineCode));
        }
    }
}
