using FluentValidation;
using Menominee.Api.Features.Inventory.InventoryItems;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.MaintenanceItems;

namespace Menominee.Api.Features.Inventory.MaintenanceItems
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
