using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackagePlaceholderValidator : AbstractValidator<InventoryItemPackagePlaceholderToWrite>
    {
        public InventoryItemPackagePlaceholderValidator()
        {
            RuleFor(placeholder => placeholder)
                .MustBeEntity(placeholder =>
                    InventoryItemPackagePlaceholder.Create(
                    placeholder.ItemType,
                    placeholder.Description,
                    placeholder.DisplayOrder,
                    InventoryItemPackageDetails.Create(
                        placeholder.Details.Quantity,
                        placeholder.Details.PartAmountIsAdditional,
                        placeholder.Details.LaborAmountIsAdditional,
                        placeholder.Details.ExciseFeeIsAdditional)
                    .Value));
        }
    }
}
