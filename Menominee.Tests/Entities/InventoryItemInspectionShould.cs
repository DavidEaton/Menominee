using Menominee.Domain.Entities.Inventory;
using FluentAssertions;
using Menominee.Common.Enums;
using TestingHelperLibrary;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class InventoryItemInspectionShould
    {
        [Fact]
        public void Create_InventoryItemInspection()
        {
            // Arrange

            var laborAmount = LaborAmount.Create(
                ItemLaborType.Flat,
                LaborAmount.MinimumAmount).Value;
            var techAmount = TechAmount.Create(
                ItemLaborType.Flat,
                LaborAmount.MinimumAmount,
                SkillLevel.A).Value;

            // Act
            var resultOrError = InventoryItemInspection.Create(
                laborAmount,
                techAmount,
                InventoryItemInspectionType.CourtesyCheck);

            // Assert
            resultOrError.Value.Should().BeOfType<InventoryItemInspection>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Not_Create_InventoryItemInspection_With_Invalid_InspectionType()
        {
            var laborAmount = LaborAmount.Create(
                ItemLaborType.Flat,
                LaborAmount.MinimumAmount).Value;
            var techAmount = TechAmount.Create(
                ItemLaborType.Flat,
                LaborAmount.MinimumAmount,
                SkillLevel.A).Value;
            var invalidInspectionType = (InventoryItemInspectionType)(-1);

            var resultOrError = InventoryItemInspection.Create(
                laborAmount,
                techAmount,
                invalidInspectionType);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetLaborAmount()
        {
            var inspection = InventoryItemTestHelper.CreateInventoryItemInspection();
            var originalLaborAmount = inspection.LaborAmount;
            var newLaborAmount = LaborAmount.Create(
                ItemLaborType.Time,
                LaborAmount.MinimumAmount).Value;
            inspection.LaborAmount.Should().Be(originalLaborAmount);


            var resultOrError = inspection.SetLaborAmount(newLaborAmount);

            resultOrError.IsFailure.Should().BeFalse();
            inspection.LaborAmount.Should().Be(newLaborAmount);
            inspection.LaborAmount.Should().NotBe(originalLaborAmount);
        }

        [Fact]
        public void Not_Set_Null_LaborAmount()
        {
            var inspection = InventoryItemTestHelper.CreateInventoryItemInspection();
            var originalLaborAmount = inspection.LaborAmount;
            inspection.LaborAmount.Should().Be(originalLaborAmount);


            var resultOrError = inspection.SetLaborAmount(null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetTechAmount()
        {
            var inspection = InventoryItemTestHelper.CreateInventoryItemInspection();
            var originalTechAmount = inspection.TechAmount;
            var newTechAmount = TechAmount.Create(
                ItemLaborType.Time,
                LaborAmount.MinimumAmount,
                SkillLevel.E).Value;
            inspection.TechAmount.Should().Be(originalTechAmount);


            var resultOrError = inspection.SetTechAmount(newTechAmount);

            resultOrError.IsFailure.Should().BeFalse();
            inspection.TechAmount.Should().Be(newTechAmount);
            inspection.TechAmount.Should().NotBe(originalTechAmount);
        }

        [Fact]
        public void Not_Set_Null_TechAmount()
        {
            var inspection = InventoryItemTestHelper.CreateInventoryItemInspection();
            var originalTechAmount = inspection.TechAmount;
            inspection.TechAmount.Should().Be(originalTechAmount);

            var resultOrError = inspection.SetTechAmount(null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetInspectionType()
        {
            var inspection = InventoryItemTestHelper.CreateInventoryItemInspection();
            var originalInspectionType = inspection.InspectionType;
            var newInspectionType = InventoryItemInspectionType.Paid;
            inspection.InspectionType.Should().Be(originalInspectionType);

            var resultOrError = inspection.SetInspectionType(newInspectionType);

            resultOrError.IsFailure.Should().BeFalse();
            inspection.InspectionType.Should().Be(newInspectionType);
            inspection.InspectionType.Should().NotBe(originalInspectionType);
        }

        [Fact]
        public void Not_Set_Invalid_InspectionType()
        {
            var inspection = InventoryItemTestHelper.CreateInventoryItemInspection();
            var originalInspectionType = inspection.InspectionType;
            var invalidInspectionType = (InventoryItemInspectionType)(-1);
            inspection.InspectionType.Should().Be(originalInspectionType);

            var resultOrError = inspection.SetInspectionType(invalidInspectionType);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }


    }
}
