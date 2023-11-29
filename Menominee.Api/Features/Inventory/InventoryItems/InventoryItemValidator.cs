using FluentValidation;
using Menominee.Api.Features.Inventory.InventoryItems.Inspection;
using Menominee.Api.Features.Inventory.InventoryItems.Labor;
using Menominee.Api.Features.Inventory.InventoryItems.Package;
using Menominee.Api.Features.Inventory.InventoryItems.Part;
using Menominee.Api.Features.Inventory.InventoryItems.Tire;
using Menominee.Api.Features.Inventory.InventoryItems.Warranty;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems;

namespace Menominee.Api.Features.Inventory.InventoryItems
{
    public class InventoryItemValidator : AbstractValidator<InventoryItemToWrite>
    {
        public InventoryItemValidator()
        {
            // TODO: inject DbContext to get
            // those entities, which when successful, validates the ManufacturerToRead
            // and ProductCodeToRead dtos, for aggregate root validation completeness.
            // HOWEVER, that would create a circular dependency from this project to API
            // and back again. So we miss an edge case if the client somehow sends a
            // Manufacturer or ProductCode read dto that represents a non-existent entity

            // Validate aggregate root entity, omitting all but
            // the first optional member (to facilitate testing) 
            RuleFor(itemDto => itemDto)
                .MustBeEntity(
                    itemDto => InventoryItem.Create(
                        // TODO: ask dave how to proceed with this
                        ValidatorHelper.CreateManufacturer(itemDto.Manufacturer),
                        itemDto.ItemNumber,
                        itemDto.Description,
                        ValidatorHelper.CreateProductCode(itemDto.Manufacturer, itemDto.ProductCode),
                        itemDto.ItemType,

                        // TODO: FIND A SOLUTION THAT AVOIDS CODE POLLUTION: "...(to facilitate testing)"
                        part: ValidatorHelper.CreateInventoryItemPart()));

            // Validate optional members
            RuleFor(itemDto => itemDto.Part)
                .SetValidator(new InventoryItemPartValidator())
                .When(itemDto => itemDto.Part is not null);

            RuleFor(itemDto => itemDto.Labor)
                .SetValidator(new InventoryItemLaborValidator())
                .When(itemDto => itemDto.Labor is not null);

            RuleFor(itemDto => itemDto.Tire)
                .SetValidator(new InventoryItemTireValidator())
                .When(itemDto => itemDto.Tire is not null);

            RuleFor(itemDto => itemDto.Package)
                .SetValidator(new InventoryItemPackageValidator())
                .When(itemDto => itemDto.Package is not null);

            RuleFor(itemDto => itemDto.Inspection)
                .SetValidator(new InventoryItemInspectionValidator())
                .When(itemDto => itemDto.Inspection is not null);

            RuleFor(itemDto => itemDto.Warranty)
                .SetValidator(new InventoryItemWarrantyValidator())
                .When(itemDto => itemDto.Warranty is not null);
        }
    }
}
