using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class InventoryItemShould
    {
        [Fact]
        public void Create_InventoryItem()
        {
            // Arrange
            Manufacturer manufacturer = CreateManufacturer();
            ProductCode productCode = CreateProductCode();
            InventoryItemPart part = CreateInventoryItemPart();

            // Act
            var resultOrError = InventoryItem.Create(manufacturer, "001", "a description", productCode, InventoryItemType.Part, part: part);

            // Assert
            resultOrError.Value.Should().BeOfType<InventoryItem>();
            resultOrError.IsFailure.Should().BeFalse();
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
