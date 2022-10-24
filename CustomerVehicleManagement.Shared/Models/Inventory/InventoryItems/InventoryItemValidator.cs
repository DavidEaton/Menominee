using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Inspection;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Part;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Tire;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Warranty;
using FluentValidation;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems
{
    public class InventoryItemValidator : AbstractValidator<InventoryItemToWrite>
    {
        // May be better to inject Manufacturer and ProductCode respoitories to get
        // those entities, which when successful, validates the ManufacturerToRead
        // and ProductCodeToRead dtos, for aggregate root validation completeness.
        // However that would create a circular dependency from this project to API
        // and back again. So we miss an edge case if the client somehow sends a
        // Manufacturer or ProductCode read dto that represents a non-existent entity
        private readonly Manufacturer validManufacturer = Manufacturer.Create("Manufacturer One", "M1", "V1").Value;
        private readonly ProductCode validProductCode = new()
        {
            Name = "A Product",
            Code = "P1"
        };

        public InventoryItemValidator()
        {
            // Validate aggregate root entity, omitting all but
            // the first optional member (to facilitate testing)
            RuleFor(itemDto => itemDto)
                .MustBeEntity(
                    itemDto => InventoryItem.Create(
                        validManufacturer,
                        itemDto.ItemNumber,
                        itemDto.Description,
                        validProductCode,
                        itemDto.ItemType,
                        CreateInventoryItemPart()));

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

        private static InventoryItemPart CreateInventoryItemPart()
        {
            return InventoryItemPart.Create(
                InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional: false).Value;
        }
    }
}
