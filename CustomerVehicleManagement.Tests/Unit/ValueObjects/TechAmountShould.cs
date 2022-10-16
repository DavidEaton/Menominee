using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.ValueObjects
{
    public class TechAmountShould
    {
        [Fact]
        public void Create_TechAmount()
        {
            // Arrange
            // Act
            var resultOrError = TechAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount, SkillLevel.A);

            // Assert
            resultOrError.Value.Should().BeOfType<TechAmount>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Not_Create_TechAmount_With_Invalid_Amount()
        {
            // Arrange
            // Act
            var resultOrError = TechAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount - 1, SkillLevel.A);

            // Assert
            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("Invalid");
        }

        [Fact]
        public void Not_Create_TechAmount_With_Invalid_SkillLevel()
        {
            // Arrange
            // Act
            var resultOrError = TechAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount, (SkillLevel)(-1));

            // Assert
            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

    }
}
