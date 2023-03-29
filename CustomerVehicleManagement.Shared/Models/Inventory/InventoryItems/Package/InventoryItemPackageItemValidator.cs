using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentValidation;
using Menominee.Common.Enums;

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

            RuleFor(packageItemDto => packageItemDto)
                .MustBeValueObject(
                    details => InventoryItemPackageDetails.Create(
                        details.Quantity,
                        details.PartAmountIsAdditional,
                        details.LaborAmountIsAdditional,
                        details.LaborAmountIsAdditional));

            RuleFor(packageItemDto => packageItemDto)
                .MustBeEntity(
                    package => InventoryItemPackageItem.Create(
                        package.DisplayOrder,
                        InventoryItem.Create(
                            ValidatorHelper.CreateManufacturer(package.Item.Manufacturer),
                            package.Item.ItemNumber,
                            package.Item.Description,
                            ValidatorHelper.CreateProductCode(package.Item.Manufacturer, package.Item.ProductCode),
                            package.Item.ItemType)
                        .Value,
                        InventoryItemPackageDetails.Create(
                        package.Quantity,
                        package.PartAmountIsAdditional,
                        package.LaborAmountIsAdditional,
                        package.LaborAmountIsAdditional)
                        .Value));
        }
    }
}
