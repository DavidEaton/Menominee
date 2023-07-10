using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems;
using FluentValidation;

namespace Menominee.Shared.Models.Inventory.MaintenanceItems
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
