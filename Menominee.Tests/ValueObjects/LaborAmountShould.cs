using Menominee.Domain.Entities.Inventory;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace Menominee.Tests.ValueObjects
{
    public class LaborAmountShould
    {
        [Theory]
        [InlineData(ItemLaborType.None, 0.0)]
        [InlineData(ItemLaborType.Flat, 100.0)]
        [InlineData(ItemLaborType.Time, 0.5)]
        public void Create_LaborAmount_With_ValidInput(ItemLaborType type, double amount)
        {
            var result = LaborAmount.Create(type, amount);

            result.IsSuccess.Should().BeTrue();
            result.Value.Type.Should().Be(type);
            result.Value.Amount.Should().Be(amount);
        }

        [Theory]
        [InlineData((ItemLaborType)(-1), 0.1)]
        [InlineData(ItemLaborType.Flat, -100.0)]
        [InlineData(ItemLaborType.Time, -0.5)]
        [InlineData((ItemLaborType)999, 100.0)]
        public void Not_Create_LaborAmount_With_Invalid_Input(ItemLaborType type, double amount)
        {
            var result = LaborAmount.Create(type, amount);

            result.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void Create_With_Zero_Amount_When_Type_Is_None()
        {
            var result = LaborAmount.Create(ItemLaborType.None,
                LaborAmount.MinimumAmount + 1);

            result.Value.Amount.Should().Be(0);
        }

        [Fact]
        public void Replace_With_New_On_NewPayType()
        {
            var result = LaborAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount);
            result.Value.Type.Should().Be(ItemLaborType.Flat);

            result = result.Value.NewPayType(ItemLaborType.Time);

            result.Value.Type.Should().Be(ItemLaborType.Time);
        }

        [Fact]
        public void Replace_With_New_On_NewDiscountAmount()
        {
            var result = LaborAmount.Create(ItemLaborType.Flat,
                LaborAmount.MinimumAmount + 1);
            result.Value.Amount.Should().Be(LaborAmount.MinimumAmount + 1);

            result = result.Value.NewAmount(LaborAmount.MinimumAmount + 10);

            result.Value.Amount.Should().Be(LaborAmount.MinimumAmount + 10);
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
