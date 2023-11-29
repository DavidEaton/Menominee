using FluentValidation;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems.Package;

namespace Menominee.Api.Features.Inventory.InventoryItems.Package
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
