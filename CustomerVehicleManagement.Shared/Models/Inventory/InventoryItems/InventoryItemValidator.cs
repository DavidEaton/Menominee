using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Inspection;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Part;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Tire;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Warranty;
using FluentValidation;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems
{
    public class InventoryItemValidator : AbstractValidator<InventoryItemToWrite>
    {
        // May be better to inject Manufacturer and ProductCode respoitories to get
        // those entities, which when successful, validates the ManufacturerToRead
        // and ProductCodeToRead dtos, for aggregate root validation completeness.
        private readonly Manufacturer validManufacturer = Manufacturer.Create("Manufacturer One", "M1", "V1").Value;
        private readonly ProductCode validProductCode = new()
        {
            Name = "A Product",
            Code = "P1"
        };

        public InventoryItemValidator()
        {
            // Validate aggregate root entity, omitting optional members
            RuleFor(itemDto => itemDto)
                .MustBeEntity(
                    itemDto => InventoryItem.Create(
                        validManufacturer,
                        itemDto.ItemNumber,
                        itemDto.Description,
                        validProductCode,
                        itemDto.ItemType));

            // TODO: enforce invariant: one and only one optional mamber
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
