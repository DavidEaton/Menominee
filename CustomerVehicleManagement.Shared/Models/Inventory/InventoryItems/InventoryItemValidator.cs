using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Inspection;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Package;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Part;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Tire;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Warranty;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentValidation;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems
{
    public class InventoryItemValidator : AbstractValidator<InventoryItemToWrite>
    {
        public InventoryItemValidator()
        {
            // May be better to inject Manufacturer and ProductCode respoitories to get
            // those entities, which when successful, validates the ManufacturerToRead
            // and ProductCodeToRead dtos, for aggregate root validation completeness.
            // However that would create a circular dependency from this project to API
            // and back again. So we miss an edge case if the client somehow sends a
            // Manufacturer or ProductCode read dto that represents a non-existent entity

            // Validate aggregate root entity, omitting all but
            // the first optional member (to facilitate testing)
            RuleFor(itemDto => itemDto)
                .MustBeEntity(
                    itemDto => InventoryItem.Create(
                        CreateManufacturer(),
                        itemDto.ItemNumber,
                        itemDto.Description,
                        CreateProductCode(),
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

        private static ProductCode CreateProductCode()
        {
            Manufacturer manufacturer = CreateManufacturer();
            SaleCode saleCode = CreateSaleCode();

            return ProductCode.Create(manufacturer, "A1", "A One", saleCode).Value;
        }

        private static SaleCode CreateSaleCode()
        {
            string name = Utilities.RandomCharacters(SaleCode.MinimumLength);
            string code = Utilities.RandomCharacters(SaleCode.MinimumLength);
            double laborRate = SaleCode.MinimumValue;
            double desiredMargin = SaleCode.MinimumValue;
            SaleCodeShopSupplies shopSupplies = new();

            return SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies).Value;
        }

        private static Manufacturer CreateManufacturer()
        {
            return Manufacturer.Create("Manufacturer One", "M1", "V1").Value;
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
