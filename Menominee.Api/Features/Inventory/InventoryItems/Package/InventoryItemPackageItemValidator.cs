using FluentValidation;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems.Package;

namespace Menominee.Api.Features.Inventory.InventoryItems.Package
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
