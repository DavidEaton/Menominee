using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
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
            var resultOrError = TechAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount - 1, SkillLevel.A);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("Invalid");
        }

        [Fact]
        public void Not_Create_TechAmount_With_Invalid_SkillLevel()
        {
            var resultOrError = TechAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount, (SkillLevel)(-1));

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Equate_Two_Instances_Having_Same_Values()
        {
            var techAmountOne = TechAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount, SkillLevel.A);
            var techAmountTwo = TechAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount, SkillLevel.A);

            techAmountOne.Should().Be(techAmountTwo);
        }

        [Fact]
        public void Not_Equate_Two_Instances_Having_Differing_Values()
        {
            var techAmountOne = TechAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount, SkillLevel.A);
            var techAmountTwo = TechAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount, SkillLevel.E);

            techAmountOne.Should().NotBe(techAmountTwo);
        }
    }
}
