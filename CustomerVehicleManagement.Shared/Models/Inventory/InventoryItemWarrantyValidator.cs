using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Inventory
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
