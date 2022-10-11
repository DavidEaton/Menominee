using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using Menominee.Common.Enums;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class InventoryItemPartShould
    {
        [Fact]
        public void Create_InventoryItemPart()
        {
            // Arrange
            // Act
            var resultOrError = InventoryItemPart.Create(
                10, 1, 1, 15,
                TechAmount.Create(ItemLaborType.Flat, 20, SkillLevel.A).Value,
                fractional: false);

            // Assert
            resultOrError.Value.Should().BeOfType<InventoryItemPart>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_InventoryItem_With_Invalid_Values(double invalidValue)
        {
            var resultOrError = InventoryItemPart.Create(
                invalidValue, invalidValue, invalidValue, invalidValue,
                TechAmount.Create(ItemLaborType.Flat, 20, SkillLevel.A).Value,
                fractional: false);

            // Assert
            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetList()
        {
            var part = CreateInventoryItemPart();
            var value = InstallablePart.MinimumValue + 1.01;

            var resultOrError = part.SetList(value);

            resultOrError.IsFailure.Should().BeFalse();
            part.List.Should().Be(value);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_List(double invalidValue)
        {
            var part = CreateInventoryItemPart();

            var resultOrError = part.SetList(invalidValue);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetCost()
        {
            var part = CreateInventoryItemPart();
            var value = InstallablePart.MinimumValue + 1.01;

            var resultOrError = part.SetCost(value);

            resultOrError.IsFailure.Should().BeFalse();
            part.Cost.Should().Be(value);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Cost(double invalidValue)
        {
            var part = CreateInventoryItemPart();

            var resultOrError = part.SetCost(invalidValue);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetCore()
        {
            var part = CreateInventoryItemPart();
            var value = InstallablePart.MinimumValue + 1.01;

            var resultOrError = part.SetCore(value);

            resultOrError.IsFailure.Should().BeFalse();
            part.Core.Should().Be(value);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Core(double invalidValue)
        {
            var part = CreateInventoryItemPart();

            var resultOrError = part.SetCore(invalidValue);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetRetail()
        {
            var part = CreateInventoryItemPart();
            var value = InstallablePart.MinimumValue + 1.01;

            var resultOrError = part.SetRetail(value);

            resultOrError.IsFailure.Should().BeFalse();
            part.Retail.Should().Be(value);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Retail(double invalidValue)
        {
            var part = CreateInventoryItemPart();

            var resultOrError = part.SetRetail(invalidValue);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetTechAmount()
        {
            var part = CreateInventoryItemPart();
            var value = TechAmount.Create(ItemLaborType.Flat, 26.88, SkillLevel.A).Value;

            var resultOrError = part.SetTechAmount(value);

            resultOrError.IsFailure.Should().BeFalse();
            part.TechAmount.Should().Be(value);
        }

        [Fact]
        public void Not_Set_Null_TechAmount()
        {
            var part = CreateInventoryItemPart();

            var resultOrError = part.SetTechAmount(null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetLineCode()
        {
            var part = CreateInventoryItemPart();
            var value = "moops code";

            var resultOrError = part.SetLineCode(value);

            resultOrError.IsFailure.Should().BeFalse();
            part.LineCode.Should().Be(value);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestDataInteger))]
        public void Not_Set_Invalid_LineCode(int invalidValue)
        {
            var part = CreateInventoryItemPart();

            var resultOrError = part.SetLineCode(Utilities.RandomCharacters(invalidValue));

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("mustt");
        }

        [Fact]
        public void SetSubLineCode()
        {
            var part = CreateInventoryItemPart();
            var value = "moops code";

            var resultOrError = part.SetSubLineCode(value);

            resultOrError.IsFailure.Should().BeFalse();
            part.SubLineCode.Should().Be(value);
        }

        [Fact]
        public void SetFractional()
        {
            var part = CreateInventoryItemPart();
            var value = false;

            var resultOrError = part.SetFractional(value);

            resultOrError.IsFailure.Should().BeFalse();
            part.Fractional.Should().Be(value);
        }


        private static InventoryItemPart CreateInventoryItemPart()
        {
            return InventoryItemPart.Create(
                10, 1, 1, 15,
                TechAmount.Create(ItemLaborType.Flat, 20, SkillLevel.A).Value,
                fractional: false).Value;
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { InventoryItemPart.MinimumValue - .01 };
                    yield return new object[] { InventoryItemPart.MaximumValue + .01 };
                }
            }
        }

        internal class TestDataInteger
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { InventoryItemPart.MinimumLength - 1 };
                    yield return new object[] { InventoryItemPart.MaximumLength + 1 };
                }
            }
        }

    }
}
