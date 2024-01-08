using FluentAssertions;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Enums;
using System.Collections.Generic;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class InventoryItemTireShould
    {
        [Fact]
        public void Create_InventoryItemTire()
        {
            // Arrange
            var fractional = false;
            int width = InventoryItemTire.MaximumWidth;
            int aspectRatio = 65;
            TireConstructionType constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;

            // Act
            var resultOrError = InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameter,
                InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
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
            int aspectRatio = 65;
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
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
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
            int aspectRatio = 65;
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
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
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
            int aspectRatio = 65;
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
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
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
            int aspectRatio = 65;
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
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
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
            int aspectRatio = 65;
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
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
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
            int aspectRatio = 65;
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
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
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
            int aspectRatio = 65;
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
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
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
            int aspectRatio = 65;
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
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode,
                type, loadIndex, speedRating);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_InventoryItemTire_With_Overrange_Type()
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            string subLineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            int width = InventoryItemTire.MinimumWidth;
            int aspectRatio = 65;
            TireConstructionType constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;
            string typeOVERRANGE = Utilities.RandomCharacters(InventoryItemTire.MaximumTypeLength + 1);
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
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode,
                typeOVERRANGE, loadIndex, speedRating);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_InventoryItemTire_With_Invalid_AsectRatio()
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            string subLineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            int width = InventoryItemTire.MinimumWidth;
            int aspectRatioInvalid = 6;
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
                width, aspectRatioInvalid, constructionType, diameter,
                list,
                cost,
                core,
                retail,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode,
                type, loadIndex, speedRating);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Be(InventoryItemTire.InvalidAspectRatioMessage);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        public void Not_Create_InventoryItemTire_With_Overrange_Width(int width)
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            string subLineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            int aspectRatio = 65;
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
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode,
                type, loadIndex, speedRating);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_InventoryItemTire_With_Invalid_ConstructionType()
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            string subLineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            int width = InventoryItemTire.MinimumWidth;
            int aspectRatio = 65;
            var constructionTypeInvalid = (TireConstructionType)(-1);
            int diameter = InventoryItemTire.MaximumDiameter;
            string type = Utilities.RandomCharacters(InventoryItemTire.MaximumTypeLength);
            int? loadIndex = InstallablePart.MaximumMoneyAmount;
            string speedRating = Utilities.RandomCharacters(InventoryItemTire.MaximumSpeedRatingLength);

            var list = InstallablePart.MaximumMoneyAmount;
            var cost = InstallablePart.MaximumMoneyAmount;
            var core = InstallablePart.MaximumMoneyAmount;
            var retail = InstallablePart.MaximumMoneyAmount;

            var resultOrError = InventoryItemTire.Create(
                width, aspectRatio, constructionTypeInvalid, diameter,
                list,
                cost,
                core,
                retail,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode,
                type, loadIndex, speedRating);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Create_InventoryItemTire_With_Invalid_Diameter()
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            string subLineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            int width = InventoryItemTire.MinimumWidth;
            int aspectRatio = 65;
            var constructionType = TireConstructionType.R;
            int diameterInvalid = InventoryItemTire.MaximumDiameter + 1;
            string type = Utilities.RandomCharacters(InventoryItemTire.MaximumTypeLength);
            int? loadIndex = InstallablePart.MaximumMoneyAmount;
            string speedRating = Utilities.RandomCharacters(InventoryItemTire.MaximumSpeedRatingLength);

            var list = InstallablePart.MaximumMoneyAmount;
            var cost = InstallablePart.MaximumMoneyAmount;
            var core = InstallablePart.MaximumMoneyAmount;
            var retail = InstallablePart.MaximumMoneyAmount;

            var resultOrError = InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameterInvalid,
                list,
                cost,
                core,
                retail,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode,
                type, loadIndex, speedRating);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Be(InventoryItemTire.InvalidDiameterMessage);
        }
        [Fact]
        public void Not_Create_InventoryItemTire_With_Invalid_SpeedRating()
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            string subLineCode = Utilities.RandomCharacters(InventoryItemTire.MaximumLength);
            int width = InventoryItemTire.MinimumWidth;
            int aspectRatio = 65;
            var constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;
            string type = Utilities.RandomCharacters(InventoryItemTire.MaximumTypeLength);
            int? loadIndex = InstallablePart.MaximumMoneyAmount;
            string speedRatingInvalid = Utilities.RandomCharacters(InventoryItemTire.MaximumSpeedRatingLength + 1);

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
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode,
                type, loadIndex, speedRatingInvalid);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Be(InventoryItemTire.InvalidSpeedRatingMessage);
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

        [Fact]
        public void Not_Set_Invalid_Type()
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            int invalidLength = InventoryItemTire.MaximumTypeLength + 1;
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
            int aspectRatio = 65;
            var resultOrError = tire.SetAspectRatio(aspectRatio);

            resultOrError.IsFailure.Should().BeFalse();
            tire.AspectRatio.Should().Be(aspectRatio);
        }

        [Theory]
        [MemberData(nameof(TestData.DataIntegerInvalidAspectRatio), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_AspectRatio(int invalidAspectRatio)
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            var resultOrError = tire.SetAspectRatio(invalidAspectRatio);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetConstructionType()
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            TireConstructionType constructionType = TireConstructionType.F;
            var resultOrError = tire.SetConstructionType(constructionType);

            resultOrError.IsFailure.Should().BeFalse();
            tire.ConstructionType.Should().Be(constructionType);
        }

        [Fact]
        public void Not_Set_Invalid_ConstructionType()
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            TireConstructionType constructionType = (TireConstructionType)(-1);
            var resultOrError = tire.SetConstructionType(constructionType);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
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

        [Theory]
        [MemberData(nameof(TestData.DataIntegerInvalidDiameter), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Diameter(int invalidDiameter)
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            var resultOrError = tire.SetDiameter(invalidDiameter);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
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
        public void Not_Set_Invalid_LoadIndex()
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            int invalidLoadIndex = 123456;
            var resultOrError = tire.SetLoadIndex(invalidLoadIndex);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
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

        [Fact]
        public void Not_Set_Invalid_SpeedRating()
        {
            InventoryItemTire tire = CreateInventoryItemTire();
            string invalidSpeedRating = Utilities.RandomCharacters(InventoryItemTire.MaximumSpeedRatingLength + 1);
            var resultOrError = tire.SetSpeedRating(invalidSpeedRating);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        private static InventoryItemTire CreateInventoryItemTire()
        {
            var fractional = false;
            int width = InventoryItemTire.MinimumWidth;
            int aspectRatio = 65;
            TireConstructionType constructionType = TireConstructionType.R;
            int diameter = InventoryItemTire.MaximumDiameter;

            var resultOrError = InventoryItemTire.Create(
                width, aspectRatio, constructionType, diameter,
                InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional);

            if (resultOrError.IsSuccess)
                return resultOrError.Value;

            return null;
        }

        internal static class TestData
        {
            public static IEnumerable<object[]> DataIntegerInvalidWidth
            {
                get
                {
                    yield return new object[] { InventoryItemTire.MaximumWidth + 1 };
                    yield return new object[] { InventoryItemTire.MinimumWidth - 1 };
                }
            }

            public static IEnumerable<object[]> DataIntegerInvalidDiameter
            {
                get
                {
                    yield return new object[] { InventoryItemTire.MaximumDiameter + 1 };
                    yield return new object[] { InventoryItemTire.MinimumDiameter - 1 };
                }
            }

            public static IEnumerable<object[]> DataIntegerInvalidAspectRatio
            {
                get
                {
                    yield return new object[] { 1 };
                    yield return new object[] { 111 };
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