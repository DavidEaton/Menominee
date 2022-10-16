using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.ValueObjects
{
    public class LaborAmountShould
    {
        [Fact]
        public void Create_LaborAmount()
        {
            // Arrange
            // Act
            var resultOrError = LaborAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount);

            // Assert
            resultOrError.Value.Should().BeOfType<LaborAmount>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Not_Create_LaborAmount_With_Invalid_Amount()
        {
            // Arrange
            // Act
            var resultOrError = LaborAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount - 1);

            // Assert
            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("Invalid");
        }

        [Fact]
        public void Not_Create_LaborAmount_With_Invalid_ItemLaborType()
        {
            // Arrange
            // Act
            var resultOrError = LaborAmount.Create((ItemLaborType)(-1),
                LaborAmount.MinimumAmount);

            // Assert
            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }
    }
}
