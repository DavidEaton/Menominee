using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.ValueObjects
{
    public class LaborAmountShould
    {
        [Theory]
        [InlineData(ItemLaborType.None, 0.0)]
        [InlineData(ItemLaborType.Flat, 100.0)]
        [InlineData(ItemLaborType.Time, 0.5)]
        public void Create_LaborAmount_With_ValidInput(ItemLaborType payType, double amount)
        {
            var result = LaborAmount.Create(payType, amount);

            result.IsSuccess.Should().BeTrue();
            result.Value.PayType.Should().Be(payType);
            result.Value.Amount.Should().Be(amount);
        }

        [Theory]
        [InlineData((ItemLaborType)(-1), 0.1)]
        [InlineData(ItemLaborType.Flat, -100.0)]
        [InlineData(ItemLaborType.Time, -0.5)]
        [InlineData((ItemLaborType)999, 100.0)]
        public void Not_Create_LaborAmount_With_Invalid_Input(ItemLaborType payType, double amount)
        {
            var result = LaborAmount.Create(payType, amount);

            result.IsSuccess.Should().BeFalse();
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
