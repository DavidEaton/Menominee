﻿using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Tests.Unit.Helpers;
using FluentAssertions;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class InventoryItemPackageItemShould
    {
        [Fact]
        public void Create_InventoryItemPackageItem()
        {
            // Arrange
            var details = InventoryItemHelper.CreateInventoryItemPackageDetails();

            // Act
            var resultOrError = InventoryItemPackageItem.Create(
                InventoryItemPackageItem.MinimumValue + 1,
                InventoryItemHelper.CreateInventoryItem(),
                details);

            resultOrError.Value.Should().BeOfType<InventoryItemPackageItem>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Not_Create_InventoryItemPackageItem_With_Invalid_DisplayOrder()
        {
            var details = InventoryItemHelper.CreateInventoryItemPackageDetails();

            var resultOrError = InventoryItemPackageItem.Create(
                InventoryItemPackageItem.MinimumValue - 1,
                InventoryItemHelper.CreateInventoryItem(),
                details);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_InventoryItemPackageItem_With_Null_InventoryItem()
        {
            var details = InventoryItemHelper.CreateInventoryItemPackageDetails();

            var resultOrError = InventoryItemPackageItem.Create(
                InventoryItemPackageItem.MinimumValue + 1,
                null,
                details);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Create_InventoryItemPackageItem_With_Null_InventoryItemPackageDetails()
        {
            var resultOrError = InventoryItemPackageItem.Create(
                InventoryItemPackageItem.MinimumValue + 1,
                InventoryItemHelper.CreateInventoryItem(),
                null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetInventoryItem()
        {
            var details = InventoryItemHelper.CreateInventoryItemPackageDetails();
            var itemOne = InventoryItemHelper.CreateInventoryItem();
            var itemTwo = InventoryItemHelper.CreateInventoryItem();
            var packageItem = InventoryItemPackageItem.Create(
                InventoryItemPackageItem.MinimumValue + 1,
                itemOne,
                details).Value;
            packageItem.Item.Should().Be(itemOne);

            var resultOrError = packageItem.SetInventoryItem(itemTwo);

            resultOrError.IsFailure.Should().BeFalse();
            packageItem.Item.Should().Be(itemTwo);
            packageItem.Item.Should().NotBe(itemOne);
        }

        [Fact]
        public void Not_Set_Null_InventoryItem()
        {
            var details = InventoryItemHelper.CreateInventoryItemPackageDetails();
            var packageItem = InventoryItemPackageItem.Create(
                InventoryItemPackageItem.MinimumValue + 1,
                InventoryItemHelper.CreateInventoryItem(),
                details).Value;

            var resultOrError = packageItem.SetInventoryItem(null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetDisplayOrder()
        {
            var details = InventoryItemHelper.CreateInventoryItemPackageDetails();
            var itemOne = InventoryItemHelper.CreateInventoryItem();
            var displayOrderOne = 1;
            var displayOrderTwo = 2;

            var packageItem = InventoryItemPackageItem.Create(
                displayOrderOne,
                itemOne,
                details).Value;
            packageItem.Item.Should().Be(itemOne);

            var resultOrError = packageItem.SetDisplayOrder(displayOrderTwo);

            resultOrError.IsFailure.Should().BeFalse();
            packageItem.DisplayOrder.Should().Be(displayOrderTwo);
            packageItem.DisplayOrder.Should().NotBe(displayOrderOne);
        }

        [Fact]
        public void Not_Set_Invalid_DisplayOrder()
        {
            var details = InventoryItemHelper.CreateInventoryItemPackageDetails();
            var validDisplayOrder = InventoryItemPackageItem.MinimumValue + 1;
            var invalidDisplayOrder = InventoryItemPackageItem.MinimumValue - 1;

            var packageItem = InventoryItemPackageItem.Create(
                validDisplayOrder,
                InventoryItemHelper.CreateInventoryItem(),
                details).Value;
            packageItem.DisplayOrder.Should().Be(validDisplayOrder);

            var resultOrError = packageItem.SetDisplayOrder(invalidDisplayOrder);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetDetails()
        {
            var detailsOne = InventoryItemHelper.CreateInventoryItemPackageDetails();
            var detailsIwo = InventoryItemHelper.CreateInventoryItemPackageDetails();
            detailsIwo = detailsIwo.SetQuantity(InventoryItemPackageDetails.MinimumValue + 11).Value;

            var packageItem = InventoryItemPackageItem.Create(
                InventoryItemPackageItem.MinimumValue + 1,
                InventoryItemHelper.CreateInventoryItem(),
                detailsOne).Value;
            packageItem.Details.Should().Be(detailsOne);

            var resultOrError = packageItem.SetDetails(detailsIwo);

            resultOrError.IsFailure.Should().BeFalse();
            packageItem.Details.Should().Be(detailsIwo);
            packageItem.Details.Should().NotBe(detailsOne);
        }

        [Fact]
        public void Not_Set_Null_Details()
        {
            var details = InventoryItemHelper.CreateInventoryItemPackageDetails();
            var packageItem = InventoryItemPackageItem.Create(
                InventoryItemPackageItem.MinimumValue + 1,
                InventoryItemHelper.CreateInventoryItem(),
                details).Value;
            packageItem.Details.Should().Be(details);

            var resultOrError = packageItem.SetDetails(null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }
    }
}