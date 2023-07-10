using Menominee.Domain.Entities.Inventory;
using FluentValidation;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Package
{
    public class InventoryItemPackageValidator : AbstractValidator<InventoryItemPackageToWrite>
    {
        public InventoryItemPackageValidator()
        {
            RuleFor(packageDto => packageDto)
                .MustBeEntity(
                    packageDto => InventoryItemPackage.Create(
                        packageDto.BasePartsAmount,
                        packageDto.BaseLaborAmount,
                        packageDto.Script,
                        packageDto.IsDiscountable));

            RuleFor(packageDto => packageDto.Items)
                .ForEach(item => item.SetValidator(new InventoryItemPackageItemValidator()));

            RuleFor(packageDto => packageDto.Placeholders)
                .ForEach(item => item.SetValidator(new InventoryItemPackagePlaceholderValidator()));
        }
    }
}
