using Menominee.Domain.Entities.Inventory;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace Menominee.Tests.ValueObjects
{
    public class InventoryItemWarrantyPeriodShould
    {
        [Fact]
        public void Create_InventoryItemWarrantyPeriod()
        {
            // Arrange
            var duration = 1;
            // Act
            var resultOrError = InventoryItemWarrantyPeriod.Create(
                    InventoryItemWarrantyPeriodType.Years,
                    duration);

            // Assert
            resultOrError.Value.Should().BeOfType<InventoryItemWarrantyPeriod>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Not_Create_InventoryItemWarrantyPeriod_With_Invalid_InventoryItemWarrantyPeriodType()
        {
            var invalidPeriodType = (InventoryItemWarrantyPeriodType)(-1);
            var duration = 1;

            var resultOrError = InventoryItemWarrantyPeriod.Create(
                    invalidPeriodType,
                    duration);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Create_InventoryItemWarrantyPeriod_With_Invalid_Duration()
        {
            var invalidDuration = -1;

            var resultOrError = InventoryItemWarrantyPeriod.Create(
                    InventoryItemWarrantyPeriodType.Years,
                    invalidDuration);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Return_New_On_NewPeriodType()
        {
            var duration = 1;
            var warrantyPeriod = InventoryItemWarrantyPeriod.Create(
                InventoryItemWarrantyPeriodType.Years, duration).Value;
            var originalPeriodType = warrantyPeriod.PeriodType;
            var newPeriodType = InventoryItemWarrantyPeriodType.Months;
            warrantyPeriod.PeriodType.Should().Be(originalPeriodType);

            var resultOrError = warrantyPeriod.NewPeriodType(newPeriodType);

            resultOrError.IsSuccess.Should().BeTrue();
            resultOrError.Value.PeriodType.Should().Be(newPeriodType);
        }

        [Fact]
        public void Not_Set_Invalid_PeriodType()
        {
            var duration = 1;
            var warrantyPeriod = InventoryItemWarrantyPeriod.Create(
                InventoryItemWarrantyPeriodType.Years, duration).Value;
            var originalPeriodType = warrantyPeriod.PeriodType;
            var invalidPeriodType = (InventoryItemWarrantyPeriodType)(-1);
            warrantyPeriod.PeriodType.Should().Be(originalPeriodType);

            var resultOrError = warrantyPeriod.NewPeriodType(invalidPeriodType);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Return_New_On_NewDuration()
        {
            var duration = 1;
            var warrantyPeriod = InventoryItemWarrantyPeriod.Create(
                InventoryItemWarrantyPeriodType.Years, duration).Value;
            var originalDuration = warrantyPeriod.Duration;
            var newDuration = 2;
            warrantyPeriod.Duration.Should().Be(originalDuration);

            var resultOrError = warrantyPeriod.NewDuration(newDuration);

            resultOrError.IsSuccess.Should().BeTrue();
            resultOrError.Value.Duration.Should().Be(newDuration);
        }

        [Fact]
        public void Not_Set_Invalid_Duration()
        {
            var duration = 1;
            var warrantyPeriod = InventoryItemWarrantyPeriod.Create(
                InventoryItemWarrantyPeriodType.Years, duration).Value;
            var invalidDuration = -1;

            var resultOrError = warrantyPeriod.NewDuration(invalidDuration);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("Must");
        }

        [Fact]
        public void Equate_Two_Instances_Having_Same_Values()
        {
            var duration = 1;
            var warrantyPeriodOne = InventoryItemWarrantyPeriod.Create(
                InventoryItemWarrantyPeriodType.Years, duration).Value;

            var warrantyPeriodTwo = InventoryItemWarrantyPeriod.Create(
                InventoryItemWarrantyPeriodType.Years, duration).Value;

            warrantyPeriodOne.Should().BeEquivalentTo(warrantyPeriodTwo);
        }

        [Fact]
        public void Not_Equate_Two_Instances_Having_Differing_Values()
        {
            var duration = 1;
            var warrantyPeriodOne = InventoryItemWarrantyPeriod.Create(
                InventoryItemWarrantyPeriodType.Years, duration).Value;

            var warrantyPeriodTwo = InventoryItemWarrantyPeriod.Create(
                InventoryItemWarrantyPeriodType.Years, duration + 2).Value;

            warrantyPeriodOne.Should().NotBeSameAs(warrantyPeriodTwo);
        }
    }
}
