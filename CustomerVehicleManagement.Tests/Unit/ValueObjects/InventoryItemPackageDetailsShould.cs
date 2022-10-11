using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.ValueObjects
{
    public class InventoryItemPackageDetailsShould
    {
        [Fact]
        public void Create_InventoryItemPackageDetails()
        {
            // Arrange
            // Act
            var resultOrError = InventoryItemPackageDetails.Create(InventoryItemPackageDetails.MinimumValue + .1, true, true, true);

            // Assert
            resultOrError.Value.Should().BeOfType<InventoryItemPackageDetails>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_InventoryItemPackageDetails_With_Invalid_Quantity(double invalidValue)
        {
            var resultOrError = InventoryItemPackageDetails.Create(invalidValue, true, true, true);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetQuantity()
        {
            var package = InventoryItemPackageDetails.Create(InventoryItemPackageDetails.MinimumValue + .1, true, true, true).Value;
            double quantity = 10;
            package.Quantity.Should().NotBe(quantity);

            var resultOrError = package.SetQuantity(quantity);

            resultOrError.IsFailure.Should().BeFalse();
            resultOrError.Value.Quantity.Should().Be(quantity);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set__Invalid_Quantity(double invalidValue)
        {
            var package = InventoryItemPackageDetails.Create(InventoryItemPackageDetails.MinimumValue + .1, true, true, true).Value;
            double quantity = 10;
            package.Quantity.Should().NotBe(quantity);

            var resultOrError = package.SetQuantity(invalidValue);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { InventoryItemPackageDetails.MinimumValue - .01};
                    yield return new object[] { InventoryItemPackageDetails.MaximumValue + .01 };
                }
            }
        }
    }
}
