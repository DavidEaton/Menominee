using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentValidation;
using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackageItemValidator : AbstractValidator<InventoryItemPackageItemToWrite>
    {
        public InventoryItemPackageItemValidator()
        {
            RuleFor(packageItemDto => packageItemDto.DisplayOrder)
                .GreaterThanOrEqualTo(InventoryItemPackageItem.MinimumValue);

            RuleFor(packageItemDto => packageItemDto.Item)
                .SetValidator(new InventoryItemValidator());

            RuleFor(packageItemDto => packageItemDto.Details)
                .MustBeValueObject(
                    details => InventoryItemPackageDetails.Create(
                        details.Quantity,
                        details.PartAmountIsAdditional,
                        details.LaborAmountIsAdditional,
                        details.LaborAmountIsAdditional));
        }
    }
}
