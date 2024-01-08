using Bogus;
using FluentAssertions;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Domain.Enums;
using System;
using Xunit;

namespace Menominee.Tests.ValueObjects
{
    public class DiscountAmountShould
    {
        private static readonly Faker Faker = new();

        [Fact]
        public void Create_DiscountAmount()
        {
            // Arrange
            var type = Faker.PickRandom<ItemDiscountType>();
            var amount = (double)Math.Round(Faker.Random.Decimal(1, 1000), 2);

            // Act
            var result = DiscountAmount.Create(
                type,
                amount);

            // Assert
            result.IsFailure.Should().BeFalse();
            result.Value.Should().BeOfType<DiscountAmount>();
        }

        [InlineData(ItemDiscountType.Percent)]
        [InlineData(ItemDiscountType.Dollar)]
        [Theory]
        public void Return_Failure_On_Create_DiscountAmount_With_Inavlid_Amount(ItemDiscountType type)
        {
            var amount = -1;

            var result = DiscountAmount.Create(
                type,
                amount);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(DiscountAmount.InvalidAmountMessage);
        }

        [InlineData(ItemDiscountType.None, 0)]
        [InlineData(ItemDiscountType.Predefined, 0)]
        [InlineData(ItemDiscountType.None, -1)]
        [InlineData(ItemDiscountType.Predefined, -1)]
        [InlineData(ItemDiscountType.None, 100)]
        [InlineData(ItemDiscountType.Predefined, 100)]
        [Theory]
        public void Create_Zero_DiscountAmount_With_Type_None_Or_Predefined(ItemDiscountType type, double amount)
        {
            var result = DiscountAmount.Create(
                type,
                amount);

            result.IsFailure.Should().BeFalse();
            result.Value.Amount.Should().Be(0);
        }

        [Fact]
        public void Return_Failure_On_Create_DiscountAmount_With_Inavlid_ItemDiscountType()
        {
            var type = (ItemDiscountType)(-1);
            var amount = (double)Math.Round(Faker.Random.Decimal(1, 1000), 2);

            var result = DiscountAmount.Create(
                type,
                amount);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(DiscountAmount.RequiredMessage);
        }

        [InlineData(ItemDiscountType.Percent, 0)]
        [InlineData(ItemDiscountType.Dollar, 0)]
        [InlineData(ItemDiscountType.Percent, 100)]
        [InlineData(ItemDiscountType.Dollar, 100)]
        [Theory]
        public void Return_New_DiscountAmount_On_NewDiscountAmount(ItemDiscountType type, double amount)
        {
            var discount = DiscountAmount.Create(
                type,
                amount).Value;

            var result = discount.NewDiscountAmount(amount);

            result.IsFailure.Should().BeFalse();
            result.Value.Should().BeOfType<DiscountAmount>();
            result.Value.Amount.Should().Be(amount);
        }

        [InlineData(ItemDiscountType.None)]
        [InlineData(ItemDiscountType.Predefined)]
        [InlineData(ItemDiscountType.Percent)]
        [InlineData(ItemDiscountType.Dollar)]
        [Theory]
        public void Return_New_DiscountAmount_On_NewDiscountType(ItemDiscountType type)
        {
            var discount = DiscountAmount.Create(
                ItemDiscountType.Dollar,
                10).Value;

            var result = discount.NewDiscountType(type);

            result.IsFailure.Should().BeFalse();
            result.Value.Should().BeOfType<DiscountAmount>();
            result.Value.Type.Should().Be(type);
        }

        [InlineData(-1)]
        [Theory]
        public void Return_Failure_On_NewDiscountAmount_With_Inavlid_Amount(double amount)
        {
            var discount = DiscountAmount.Create(
                ItemDiscountType.Dollar,
                10).Value;

            var result = discount.NewDiscountAmount(amount);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(DiscountAmount.InvalidAmountMessage);
        }

        [InlineData(0)]
        [InlineData(100)]
        [Theory]
        public void Return_Failure_On_NewDiscountType_With_Inavlid_ItemDiscountType(double amount)
        {
            var discount = DiscountAmount.Create(
                ItemDiscountType.Dollar,
                amount).Value;

            var type = (ItemDiscountType)(-1);
            var result = discount.NewDiscountType(type);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(DiscountAmount.RequiredMessage);
        }

    }
}
