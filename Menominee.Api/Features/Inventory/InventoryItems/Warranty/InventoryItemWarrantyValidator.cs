using FluentValidation;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems.Warranty;

namespace Menominee.Api.Features.Inventory.InventoryItems.Warranty
{
    public class InventoryItemWarrantyValidator : AbstractValidator<InventoryItemWarrantyToWrite>
    {
        public InventoryItemWarrantyValidator()
        {
            RuleFor(warrantyDto => warrantyDto)
                .MustBeEntity(
                    warrantyDto => InventoryItemWarranty.Create(
                        InventoryItemWarrantyPeriod.Create(
                            warrantyDto.PeriodType,
                            warrantyDto.Duration)
                        .Value));
        }
    }
}
