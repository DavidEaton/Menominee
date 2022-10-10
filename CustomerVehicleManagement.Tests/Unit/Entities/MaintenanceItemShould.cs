using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class MaintenanceItemShould
    {
        [Fact]
        public void Create_MaintenanceItem()
        {
            // Arrange
            Manufacturer manufacturer = CreateManufacturer();
            ProductCode productCode = CreateProductCode();
            InventoryItemPart part = CreateInventoryItemPart();
            InventoryItem item = CreateInventoryItem(manufacturer, productCode, part);

            // Act
            var resultOrError = MaintenanceItem.Create(1, item);


            // Assert
            resultOrError.Value.Should().BeOfType<MaintenanceItem>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        private InventoryItem CreateInventoryItem(Manufacturer manufacturer, ProductCode productCode, InventoryItemPart part)
        {
            return InventoryItem.Create(
                manufacturer,
                "001",
                "a description",
                productCode,
                InventoryItemType.Part,
                part: part).Value;
        }

        private static InventoryItemPart CreateInventoryItemPart()
        {
            return InventoryItemPart.Create(
                10, 1, 1, 15,
                TechAmount.Create(ItemLaborType.Flat, 20, SkillLevel.A).Value,
                fractional: false).Value;
        }

        private static ProductCode CreateProductCode()
        {
            return new ProductCode()
            {
                Name = "A Product",
                Code = "P1"
            };
        }

        private static Manufacturer CreateManufacturer()
        {
            return Manufacturer.Create("Manufacturer One", "M1", "V1").Value;
        }
    }
}
