using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.ValueObjects
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
        public void SetPeriodType()
        {
            var duration = 1;
            var warrantyPeriod = InventoryItemWarrantyPeriod.Create(
                InventoryItemWarrantyPeriodType.Years, duration).Value;
            var originalPeriodType = warrantyPeriod.PeriodType;
            var newPeriodType = InventoryItemWarrantyPeriodType.Months;
            warrantyPeriod.PeriodType.Should().Be(originalPeriodType);

            var resultOrError = warrantyPeriod.SetPeriodType(newPeriodType);

            resultOrError.IsFailure.Should().BeFalse();
            warrantyPeriod.PeriodType.Should().Be(newPeriodType);
            warrantyPeriod.PeriodType.Should().NotBe(originalPeriodType);
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

            var resultOrError = warrantyPeriod.SetPeriodType(invalidPeriodType);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetDuration()
        {
            var duration = 1;
            var warrantyPeriod = InventoryItemWarrantyPeriod.Create(
                InventoryItemWarrantyPeriodType.Years, duration).Value;
            var originalDuration = warrantyPeriod.Duration;
            var newDuration = 2;
            warrantyPeriod.Duration.Should().Be(originalDuration);

            var resultOrError = warrantyPeriod.SetDuration(newDuration);

            resultOrError.IsFailure.Should().BeFalse();
            warrantyPeriod.Duration.Should().Be(newDuration);
            warrantyPeriod.Duration.Should().NotBe(originalDuration);
        }

        [Fact]
        public void Not_Set_Invalid_Duration()
        {
            var duration = 1;
            var warrantyPeriod = InventoryItemWarrantyPeriod.Create(
                InventoryItemWarrantyPeriodType.Years, duration).Value;
            var invalidDuration = -1;

            var resultOrError = warrantyPeriod.SetDuration(invalidDuration);

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
