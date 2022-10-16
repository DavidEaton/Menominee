using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using Menominee.Common.Enums;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class InventoryItemTireShould
    {
        [Fact]
        public void Create_InventoryItemTire()
        {
            // Arrange
            var fractional = false;
            int width = InventoryItemTire.MaximumWidth;
            int aspectRatio = Utilities.RandomNonZeroInteger(InventoryItemTire.AspectRatioLength);
            TireConstructionType constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;

            // Act
            var resultOrError = InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameter,
                InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumValue, SkillLevel.A).Value,
                fractional);

            // Assert
            // TODO: Shoudn't test confirm values used to create are correct in the new object?
            resultOrError.Value.Should().BeOfType<InventoryItemTire>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Create_InventoryItemTire_With_Optional_Values()
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            string subLineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            int width = InventoryItemTire.MinimumWidth;
            int aspectRatio = Utilities.RandomNonZeroInteger(InventoryItemTire.AspectRatioLength);
            TireConstructionType constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;
            string type = Utilities.RandomCharacters(InventoryItemTire.MaximumTypeLength);
            int? loadIndex = InstallablePart.MinimumMoneyAmount;
            string speedRating = Utilities.RandomCharacters(InventoryItemTire.MaximumSpeedRatingLength);

            var resultOrError = InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameter,
                list: InstallablePart.MaximumMoneyAmount,
                cost: InstallablePart.MaximumMoneyAmount,
                core: InstallablePart.MaximumMoneyAmount,
                retail: InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumValue, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode,
                type, loadIndex, speedRating);

            resultOrError.Value.Should().BeOfType<InventoryItemTire>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Not_Create_InventoryItemTire_With_Overrange_LineCode()
        {
            var fractional = false;
            string lineCodeOVERRANGE = Utilities.RandomCharacters(InventoryItemTire.MaximumLength + 1);
            string subLineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            int width = InventoryItemTire.MaximumWidth;
            int aspectRatio = Utilities.RandomNonZeroInteger(InventoryItemTire.AspectRatioLength);
            TireConstructionType constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;
            string type = Utilities.RandomCharacters(InventoryItemTire.MaximumTypeLength);
            int? loadIndex = InstallablePart.MaximumMoneyAmount;
            string speedRating = Utilities.RandomCharacters(InventoryItemTire.MaximumSpeedRatingLength);

            var list = InstallablePart.MaximumMoneyAmount;
            var cost = InstallablePart.MaximumMoneyAmount;
            var core = InstallablePart.MaximumMoneyAmount;
            var retail = InstallablePart.MaximumMoneyAmount;

            var resultOrError = InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameter,
                list,
                cost,
                core,
                retail,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumValue, SkillLevel.A).Value,
                fractional,
                lineCodeOVERRANGE, subLineCode,
                type, loadIndex, speedRating);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_InventoryItemTire_With_Overrange_SubLineCode()
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            string subLineCodeOVERRANGE = Utilities.RandomCharacters(InventoryItemTire.MaximumLength + 1);
            int width = InventoryItemTire.MaximumWidth;
            int aspectRatio = Utilities.RandomNonZeroInteger(InventoryItemTire.AspectRatioLength);
            TireConstructionType constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;
            string type = Utilities.RandomCharacters(InventoryItemTire.MaximumTypeLength);
            int? loadIndex = InstallablePart.MaximumMoneyAmount;
            string speedRating = Utilities.RandomCharacters(InventoryItemTire.MaximumSpeedRatingLength);

            var list = InstallablePart.MaximumMoneyAmount;
            var cost = InstallablePart.MaximumMoneyAmount;
            var core = InstallablePart.MaximumMoneyAmount;
            var retail = InstallablePart.MaximumMoneyAmount;

            var resultOrError = InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameter,
                list,
                cost,
                core,
                retail,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumValue, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCodeOVERRANGE,
                type, loadIndex, speedRating);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }
        [Fact]
        public void Not_Create_InventoryItemTire_With_Overrange_LoadIndex()
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            string subLineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            int width = InventoryItemTire.MinimumWidth;
            int aspectRatio = Utilities.RandomNonZeroInteger(InventoryItemTire.AspectRatioLength);
            TireConstructionType constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;
            string type = Utilities.RandomCharacters(InventoryItemTire.MaximumTypeLength);
            int? loadIndexOVERRANGE = InstallablePart.MaximumMoneyAmount + 1;
            string speedRating = Utilities.RandomCharacters(InventoryItemTire.MaximumSpeedRatingLength);

            var list = InstallablePart.MaximumMoneyAmount;
            var cost = InstallablePart.MaximumMoneyAmount;
            var core = InstallablePart.MaximumMoneyAmount;
            var retail = InstallablePart.MaximumMoneyAmount;

            var resultOrError = InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameter,
                list,
                cost,
                core,
                retail,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumValue, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode,
                type, loadIndexOVERRANGE, speedRating);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_InventoryItemTire_With_Overrange_List()
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            string subLineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            int width = InventoryItemTire.MinimumWidth;
            int aspectRatio = Utilities.RandomNonZeroInteger(InventoryItemTire.AspectRatioLength);
            TireConstructionType constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;
            string type = Utilities.RandomCharacters(InventoryItemTire.MaximumTypeLength);
            int? loadIndex = InstallablePart.MaximumMoneyAmount;
            string speedRating = Utilities.RandomCharacters(InventoryItemTire.MaximumSpeedRatingLength);

            var listOVERRANGE = InstallablePart.MaximumMoneyAmount + 1;
            var cost = InstallablePart.MaximumMoneyAmount;
            var core = InstallablePart.MaximumMoneyAmount;
            var retail = InstallablePart.MaximumMoneyAmount;

            var resultOrError = InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameter,
                listOVERRANGE,
                cost,
                core,
                retail,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumValue, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode,
                type, loadIndex, speedRating);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_InventoryItemTire_With_Overrange_Cost()
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            string subLineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            int width = InventoryItemTire.MinimumWidth;
            int aspectRatio = Utilities.RandomNonZeroInteger(InventoryItemTire.AspectRatioLength);
            TireConstructionType constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;
            string type = Utilities.RandomCharacters(InventoryItemTire.MaximumTypeLength);
            int? loadIndex = InstallablePart.MaximumMoneyAmount;
            string speedRating = Utilities.RandomCharacters(InventoryItemTire.MaximumSpeedRatingLength);

            var list = InstallablePart.MaximumMoneyAmount;
            var costOVERRANGE = InstallablePart.MaximumMoneyAmount + 1;
            var core = InstallablePart.MaximumMoneyAmount;
            var retail = InstallablePart.MaximumMoneyAmount;

            var resultOrError = InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameter,
                list,
                costOVERRANGE,
                core,
                retail,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumValue, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode,
                type, loadIndex, speedRating);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_InventoryItemTire_With_Overrange_Core()
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            string subLineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            int width = InventoryItemTire.MinimumWidth;
            int aspectRatio = Utilities.RandomNonZeroInteger(InventoryItemTire.AspectRatioLength);
            TireConstructionType constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;
            string type = Utilities.RandomCharacters(InventoryItemTire.MaximumTypeLength);
            int? loadIndex = InstallablePart.MaximumMoneyAmount;
            string speedRating = Utilities.RandomCharacters(InventoryItemTire.MaximumSpeedRatingLength);

            var list = InstallablePart.MaximumMoneyAmount;
            var cost = InstallablePart.MaximumMoneyAmount;
            var coreOVERRANGE = InstallablePart.MaximumMoneyAmount + 1;
            var retail = InstallablePart.MaximumMoneyAmount;

            var resultOrError = InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameter,
                list,
                cost,
                coreOVERRANGE,
                retail,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumValue, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode,
                type, loadIndex, speedRating);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }
        [Fact]
        public void Not_Create_InventoryItemTire_With_Overrange_Retail()
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            string subLineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            int width = InventoryItemTire.MinimumWidth;
            int aspectRatio = Utilities.RandomNonZeroInteger(InventoryItemTire.AspectRatioLength);
            TireConstructionType constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;
            string type = Utilities.RandomCharacters(InventoryItemTire.MaximumTypeLength);
            int? loadIndex = InstallablePart.MaximumMoneyAmount;
            string speedRating = Utilities.RandomCharacters(InventoryItemTire.MaximumSpeedRatingLength);

            var list = InstallablePart.MaximumMoneyAmount;
            var cost = InstallablePart.MaximumMoneyAmount;
            var core = InstallablePart.MaximumMoneyAmount;
            var retailOVERRANGE = InstallablePart.MaximumMoneyAmount + 1;

            var resultOrError = InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameter,
                list,
                cost,
                core,
                retailOVERRANGE,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumValue, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode,
                type, loadIndex, speedRating);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetType()
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            string type = Utilities.RandomCharacters(InventoryItemTire.MaximumTypeLength);

            var resultOrError = tire.SetType(type);

            resultOrError.IsFailure.Should().BeFalse();
            tire.Type.Should().Be(type);
        }

        [Theory]
        [MemberData(nameof(TestData.DataIntegerInvalidTypeLength), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Type(int invalidLength)
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            string type = Utilities.RandomCharacters(invalidLength);

            var resultOrError = tire.SetType(type);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetWidth()
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            int width = InventoryItemTire.MaximumWidth;

            var resultOrError = tire.SetWidth(width);

            resultOrError.IsFailure.Should().BeFalse();
            tire.Width.Should().Be(width);
        }

        [Theory]
        [MemberData(nameof(TestData.DataIntegerInvalidWidth), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Width(int invalidWidth)
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            int width = invalidWidth;

            var resultOrError = tire.SetWidth(width);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetAspectRatio()
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            int aspectRatio = Utilities.RandomNonZeroInteger(InventoryItemTire.AspectRatioLength);
            var resultOrError = tire.SetAspectRatio(aspectRatio);

            resultOrError.IsFailure.Should().BeFalse();
            tire.AspectRatio.Should().Be(aspectRatio);
        }

        [Fact]
        public void SetConstructionType()
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            string type = Utilities.RandomCharacters(InventoryItemTire.MaximumTypeLength);
            var resultOrError = tire.SetType(type);

            resultOrError.IsFailure.Should().BeFalse();
            tire.Type.Should().Be(type);
        }

        [Fact]
        public void SetDiameter()
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            int diameter = InventoryItemTire.MaximumDiameter;
            var resultOrError = tire.SetDiameter(diameter);

            resultOrError.IsFailure.Should().BeFalse();
            tire.Diameter.Should().Be(diameter);
        }

        [Fact]
        public void SetLoadIndex()
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            int loadIndex = 123;
            var resultOrError = tire.SetLoadIndex(loadIndex);

            resultOrError.IsFailure.Should().BeFalse();
            tire.LoadIndex.Should().Be(loadIndex);
        }

        [Fact]
        public void SetSpeedRating()
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            string speedRating = Utilities.RandomCharacters(InventoryItemTire.MaximumSpeedRatingLength);
            var resultOrError = tire.SetSpeedRating(speedRating);

            resultOrError.IsFailure.Should().BeFalse();
            tire.SpeedRating.Should().Be(speedRating);
        }

        private static InventoryItemTire CreateInventoryItemTire()
        {
            var fractional = false;
            int width = InventoryItemTire.MinimumWidth;
            int aspectRatio = Utilities.RandomNonZeroInteger(InventoryItemTire.AspectRatioLength);
            TireConstructionType constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;

            return InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameter,
                InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumValue, SkillLevel.A).Value,
                fractional).Value;
        }

        internal class TestData
        {
            public static IEnumerable<object[]> DataIntegerInvalidTypeLength
            {
                get
                {
                    yield return new object[] { InventoryItemTire.MaximumTypeLength + 1 };
                    yield return new object[] { InventoryItemTire.MinimumLength - 1 };
                }
            }

            public static IEnumerable<object[]> DataIntegerInvalidWidth
            {
                get
                {
                    yield return new object[] { InventoryItemTire.MaximumWidth + 1 };
                    yield return new object[] { InventoryItemTire.MinimumWidth - 1 };
                }
            }

            public static IEnumerable<object[]> DataDouble
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