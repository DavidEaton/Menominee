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
            var resultOrError = LaborAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount - 1);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("Invalid");
        }

        [Fact]
        public void Not_Create_LaborAmount_With_Invalid_ItemLaborType()
        {
            var resultOrError = LaborAmount.Create((ItemLaborType)(-1),
                LaborAmount.MinimumAmount);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Equate_Two_Instances_Having_Same_Values()
        {
            var laborAmountOne = LaborAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount);
            var laborAmountTwo = LaborAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount);

            laborAmountOne.Should().Be(laborAmountTwo);
        }

        [Fact]
        public void Not_Equate_Two_Instances_Having_Differing_Values()
        {
            var laborAmountOne = LaborAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount);
            var laborAmountTwo = LaborAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount + 1);

            laborAmountOne.Should().NotBe(laborAmountTwo);
        }
    }
}
