using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentValidation;
using Menominee.Common.Enums;
using System;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackagePlaceholderValidator : AbstractValidator<InventoryItemPackagePlaceholderToWrite>
    {
        public InventoryItemPackagePlaceholderValidator()
        {
            RuleFor(placeholder => placeholder)
                .MustBeEntity(placeholder =>
                    InventoryItemPackagePlaceholder.Create(
                    (PackagePlaceholderItemType)Enum.Parse(
                        typeof(PackagePlaceholderItemType),
                        placeholder.ItemType),
                        placeholder.Description,
                        placeholder.DisplayOrder,
                        InventoryItemPackageDetails.Create(
                            placeholder.Details.Quantity,
                            placeholder.Details.PartAmountIsAdditional,
                            placeholder.Details.LaborAmountIsAdditional,
                            placeholder.Details.ExciseFeeIsAdditional).Value));
        }
    }
}
