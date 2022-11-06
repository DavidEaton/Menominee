using FluentValidation;
using Menominee.Common.Enums;
using System;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems
{
    public class TestValidator : AbstractValidator<InventoryItemToWrite>
    {
        public TestValidator()
        {
            RuleFor(itemDto => itemDto)
                .Custom((itemDto, context) =>
                {
                    if (Enum.TryParse(itemDto.ItemType, out InventoryItemType part)
                        && part == InventoryItemType.Part
                        && itemDto.Part is null)
                        context.AddFailure("Part is required.");

                    if (Enum.TryParse(itemDto.ItemType, out InventoryItemType labor)
                        && labor == InventoryItemType.Labor
                        && itemDto.Labor is null)
                        context.AddFailure("Labor is required.");

                    if (Enum.TryParse(itemDto.ItemType, out InventoryItemType tire)
                        && tire == InventoryItemType.Tire
                        && itemDto.Tire is null)
                        context.AddFailure("Tire is required.");

                    if (Enum.TryParse(itemDto.ItemType, out InventoryItemType package)
                        && package == InventoryItemType.Package
                        && itemDto.Package is null)
                        context.AddFailure("Package is required.");

                    if (Enum.TryParse(itemDto.ItemType, out InventoryItemType inspection)
                        && inspection == InventoryItemType.Inspection
                        && itemDto.Inspection is null)
                        context.AddFailure("Inspection is required.");

                    if (Enum.TryParse(itemDto.ItemType, out InventoryItemType warranty)
                        && warranty == InventoryItemType.Warranty
                        && itemDto.Warranty is null)
                        context.AddFailure("Warranty is required.");
                });
        }
    }
}
