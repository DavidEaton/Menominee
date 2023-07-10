using FluentValidation;
using Menominee.Common.Enums;

namespace Menominee.Shared.Models.Inventory.InventoryItems
{
    public class TestValidator : AbstractValidator<InventoryItemToWrite>
    {
        public TestValidator()
        {
            RuleFor(itemDto => itemDto)
                .Custom((itemDto, context) =>
                {
                    if (itemDto.ItemType is InventoryItemType.Part
                        && itemDto.Part is null)
                        context.AddFailure("Part is required.");

                    if (itemDto.ItemType is InventoryItemType.Labor
                        && itemDto.Labor is null)
                        context.AddFailure("Labor is required.");

                    if (itemDto.ItemType is InventoryItemType.Tire
                        && itemDto.Tire is null)
                        context.AddFailure("Tire is required.");

                    if (itemDto.ItemType is InventoryItemType.Package
                        && itemDto.Package is null)
                        context.AddFailure("Package is required.");

                    if (itemDto.ItemType is InventoryItemType.Inspection
                        && itemDto.Inspection is null)
                        context.AddFailure("Inspection is required.");

                    if (itemDto.ItemType is InventoryItemType.Warranty
                        && itemDto.Warranty is null)
                        context.AddFailure("Warranty is required.");
                });
        }
    }
}
