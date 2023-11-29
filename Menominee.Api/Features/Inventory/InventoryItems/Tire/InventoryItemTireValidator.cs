using FluentValidation;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems.Tire;

namespace Menominee.Api.Features.Inventory.InventoryItems.Tire
{
    public class InventoryItemTireValidator : AbstractValidator<InventoryItemTireToWrite>
    {
        public InventoryItemTireValidator()
        {
            RuleFor(tireDto => tireDto)
                .MustBeEntity(
                    itemDto => InventoryItemTire.Create(
                        itemDto.Width,
                        itemDto.AspectRatio,
                        itemDto.ConstructionType,
                        itemDto.Diameter,
                        itemDto.List,
                        itemDto.Cost,
                        itemDto.Core,
                        itemDto.Retail,
                        TechAmount.Create(
                            itemDto.TechAmount.PayType,
                            itemDto.TechAmount.Amount,
                            itemDto.TechAmount.SkillLevel)
                            .Value,
                        itemDto.Fractional,
                        itemDto.LineCode,
                        itemDto.SubLineCode,
                        itemDto.Type,
                        itemDto.LoadIndex,
                        itemDto.SpeedRating));
        }
    }
}
