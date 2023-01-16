using CustomerVehicleManagement.Domain.Entities.Inventory;
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
            var fractional = false;

            // Act
            var resultOrError = InventoryItemPart.Create(
                InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional);

            // Assert
            resultOrError.Value.Should().BeOfType<InventoryItemPart>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Create_InventoryItemPart_With_Optional_Line_Codes()
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemPart.MaximumLength);
            string subLineCode = Utilities.RandomCharacters(InventoryItemPart.MaximumLength);

            var resultOrError = InventoryItemPart.Create(
                InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode);

            resultOrError.Value.Should().BeOfType<InventoryItemPart>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_InventoryItem_With_Invalid_Money_Values(double invalidValue)
        {
            var resultOrError = InventoryItemPart.Create(
                invalidValue, invalidValue, invalidValue, invalidValue,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional: false);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_InventoryItem_With_Invalid_Line_Codes()
        {
            var resultOrError = InventoryItemPart.Create(
                InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,

                lineCode: Utilities.RandomCharacters(InventoryItemPart.MaximumLength + 1),
                subLineCode: Utilities.RandomCharacters(InventoryItemPart.MaximumLength + 1),

                fractional: false);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetList()
        {
            var part = CreateInventoryItemPart();
            var value = InstallablePart.MinimumMoneyAmount + 1.01;

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
            var value = InstallablePart.MinimumMoneyAmount + 1.01;

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
            var value = InstallablePart.MinimumMoneyAmount + 1.01;

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
            var value = InstallablePart.MinimumMoneyAmount + 1.01;

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
            var value = TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value;

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
            var value = Utilities.RandomCharacters(InventoryItemPart.MaximumLength);

            var resultOrError = part.SetLineCode(value);

            resultOrError.IsFailure.Should().BeFalse();
            part.LineCode.Should().Be(value);
        }

        [Fact]
        public void Not_Set_Invalid_LineCode()
        {
            var part = CreateInventoryItemPart();

            var resultOrError = part.SetLineCode(Utilities.RandomCharacters(InventoryItemPart.MaximumLength + 1));

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetSubLineCode()
        {
            var part = CreateInventoryItemPart();
            var value = Utilities.RandomCharacters(InventoryItemPart.MaximumLength);

            var resultOrError = part.SetSubLineCode(value);

            resultOrError.IsFailure.Should().BeFalse();
            part.SubLineCode.Should().Be(value);
        }

        [Fact]
        public void Not_Set_Invalid_SubLineCode()
        {
            var part = CreateInventoryItemPart();

            var resultOrError = part.SetSubLineCode(Utilities.RandomCharacters(InventoryItemPart.MaximumLength + 1));

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Theory]
        [MemberData(nameof(TestData.DataBoolean), MemberType = typeof(TestData))]
        public void SetFractional(bool value)
        {
            var part = CreateInventoryItemPart();

            var resultOrError = part.SetFractional(value);

            resultOrError.IsFailure.Should().BeFalse();
            part.Fractional.Should().Be(value);
        }


        private static InventoryItemPart CreateInventoryItemPart()
        {
            return InventoryItemPart.Create(
                InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional: false).Value;
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { InstallablePart.MinimumMoneyAmount - .01 };
                    yield return new object[] { InstallablePart.MaximumMoneyAmount + .01 };
                }
            }
            public static IEnumerable<object[]> DataBoolean
            {
                get
                {
                    yield return new object[] { true };
                    yield return new object[] { false };
                }
            }
        }
    }
}
