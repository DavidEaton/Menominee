using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Inventory.MaintenanceItems
{
    public class MaintenanceItemValidator : AbstractValidator<MaintenanceItemToWrite>
    {
        public MaintenanceItemValidator()
        {
            RuleFor(maintenanceItemDto => maintenanceItemDto.DisplayOrder)
                .GreaterThanOrEqualTo(MaintenanceItem.MinimumValue);

            RuleFor(maintenanceItemDto => maintenanceItemDto.Item)
                .SetValidator(new InventoryItemValidator());
        }
    }
}
