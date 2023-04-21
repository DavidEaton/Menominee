using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class InventoryItemWarrantyShould
    {
        [Fact]
        public void Create_InventoryItemWarranty()
        {
            // Arrange
            // Act
            var resultOrError = InventoryItemWarranty.Create(
                InventoryItemWarrantyPeriod.Create(
                    InventoryItemWarrantyPeriodType.Years, 3)
                .Value);

            // Assert
            resultOrError.Value.Should().BeOfType<InventoryItemWarranty>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Not_Create_InventoryItemWarranty_With_Null_WarrantyPeriod()
        {
            // Arrange
            // Act
            var resultOrError = InventoryItemWarranty.Create(null);

            // Assert
            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetWarrantyPeriod()
        {
            var warranty = InventoryItemWarranty.Create(
                InventoryItemWarrantyPeriod.Create(
                    InventoryItemWarrantyPeriodType.Years, 3)
                .Value).Value;
            var originalWarrantyPeriod = warranty.WarrantyPeriod;
            var newWarrantyPeriod = InventoryItemWarrantyPeriod.Create(
                    InventoryItemWarrantyPeriodType.Months,
                    3)
                .Value;
            warranty.WarrantyPeriod.Should().Be(originalWarrantyPeriod);


            var resultOrError = warranty.SetWarrantyPeriod(newWarrantyPeriod);

            resultOrError.IsFailure.Should().BeFalse();
            warranty.WarrantyPeriod.Should().Be(newWarrantyPeriod);
            warranty.WarrantyPeriod.Should().NotBe(originalWarrantyPeriod);
        }

        [Fact]
        public void Not_Set_WarrantyPeriod_With_Null_WarrantyPeriod()
        {
            var warranty = InventoryItemWarranty.Create(
                InventoryItemWarrantyPeriod.Create(
                    InventoryItemWarrantyPeriodType.Years, 3)
                .Value).Value;
            var originalWarrantyPeriod = warranty.WarrantyPeriod;
            warranty.WarrantyPeriod.Should().Be(originalWarrantyPeriod);


            var resultOrError = warranty.SetWarrantyPeriod(null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }
    }
}
