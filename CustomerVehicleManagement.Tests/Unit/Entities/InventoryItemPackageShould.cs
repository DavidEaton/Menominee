﻿using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Tests.Unit.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class InventoryItemPackageShould
    {
        [Fact]
        public void Create_InventoryItemPackage()
        {
            // Arrange
            double basePartsAmount = InventoryItemPackage.MinimumAmount;
            double baseLaborAmount = InventoryItemPackage.MinimumAmount;
            string script = Utilities.LoremIpsum(InventoryItemPackage.ScriptMaximumLength);
            bool isDiscountable = true;
            List<InventoryItemPackageItem> items = new();
            List<InventoryItemPackagePlaceholder> placeholders = new();

            // Act
            var resultOrError = InventoryItemPackage.Create(
                basePartsAmount,
                baseLaborAmount,
                script,
                isDiscountable,
                items,
                placeholders);

            // Assert
            resultOrError.Value.Should().BeOfType<InventoryItemPackage>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Not_Create_InventoryItemPackage_With_Invalid_BasePartsAmount()
        {
            double basePartsAmount = InventoryItemPackage.MinimumAmount - .01;
            double baseLaborAmount = InventoryItemPackage.MinimumAmount;
            string script = Utilities.LoremIpsum(InventoryItemPackage.ScriptMaximumLength);
            bool isDiscountable = true;
            List<InventoryItemPackageItem> items = new();
            List<InventoryItemPackagePlaceholder> placeholders = new();

            var resultOrError = InventoryItemPackage.Create(
                basePartsAmount,
                baseLaborAmount,
                script,
                isDiscountable,
                items,
                placeholders);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_InventoryItemPackage_With_Invalid_BaseLaborAmount()
        {
            double basePartsAmount = InventoryItemPackage.MinimumAmount;
            double baseLaborAmount = InventoryItemPackage.MinimumAmount - .01;
            string script = Utilities.LoremIpsum(InventoryItemPackage.ScriptMaximumLength);
            bool isDiscountable = true;
            List<InventoryItemPackageItem> items = new();
            List<InventoryItemPackagePlaceholder> placeholders = new();

            var resultOrError = InventoryItemPackage.Create(
                basePartsAmount,
                baseLaborAmount,
                script,
                isDiscountable,
                items,
                placeholders);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Create_InventoryItemPackage_With_Truncated_Script()
        {
            double basePartsAmount = InventoryItemPackage.MinimumAmount;
            double baseLaborAmount = InventoryItemPackage.MinimumAmount;
            string script = Utilities.LoremIpsum(InventoryItemPackage.ScriptMaximumLength + 1000);
            bool isDiscountable = true;
            List<InventoryItemPackageItem> items = new();
            List<InventoryItemPackagePlaceholder> placeholders = new();

            var resultOrError = InventoryItemPackage.Create(
                basePartsAmount,
                baseLaborAmount,
                script,
                isDiscountable,
                items,
                placeholders);

            resultOrError.Value.Should().BeOfType<InventoryItemPackage>();
            resultOrError.IsFailure.Should().BeFalse();
            resultOrError.Value.Script.Length.Should().Be(InventoryItemPackage.ScriptMaximumLength);
        }

        [Fact]
        public void SetBaseLaborAmount()
        {
            InventoryItemPackage package = InventoryItemHelper.CreateInventoryItemPackage();
            var originalAmount = package.BaseLaborAmount;
            double newBaseLaborAmount = InventoryItemPackage.MinimumAmount + .01;

            var resultOrError = package.SetBaseLaborAmount(newBaseLaborAmount);

            resultOrError.IsFailure.Should().BeFalse();
            package.BaseLaborAmount.Should().Be(newBaseLaborAmount);
            package.BaseLaborAmount.Should().NotBe(originalAmount);
        }

        [Fact]
        public void Not_Set_Invalid_BaseLaborAmount()
        {
            InventoryItemPackage package = InventoryItemHelper.CreateInventoryItemPackage();
            double invalidBaseLaborAmount = InventoryItemPackage.MaximumAmount + .01;

            var resultOrError = package.SetBaseLaborAmount(invalidBaseLaborAmount);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetBasePartsAmount()
        {
            InventoryItemPackage package = InventoryItemHelper.CreateInventoryItemPackage();
            var originalAmount = package.BasePartsAmount;
            double newBasePartsAmount = InventoryItemPackage.MinimumAmount + .01;

            var resultOrError = package.SetBasePartsAmount(newBasePartsAmount);

            resultOrError.IsFailure.Should().BeFalse();
            package.BasePartsAmount.Should().Be(newBasePartsAmount);
            package.BasePartsAmount.Should().NotBe(originalAmount);
        }

        [Fact]
        public void Not_Set_Invalid_BasePartsAmount()
        {
            InventoryItemPackage package = InventoryItemHelper.CreateInventoryItemPackage();
            double invalidBasePartsAmount = InventoryItemPackage.MaximumAmount + .01;

            var resultOrError = package.SetBaseLaborAmount(invalidBasePartsAmount);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetScript()
        {
            InventoryItemPackage package = InventoryItemHelper.CreateInventoryItemPackage();
            var originalScript = package.Script;
            string newScript = Utilities.LoremIpsum(InventoryItemPackage.ScriptMaximumLength - 10);

            var resultOrError = package.SetScript(newScript);

            resultOrError.IsFailure.Should().BeFalse();
            package.Script.Should().Be(newScript);
            package.Script.Should().NotBe(originalScript);
        }

        [Fact]
        public void Truncate_Lengthy_Script_And_SetScript()
        {
            InventoryItemPackage package = InventoryItemHelper.CreateInventoryItemPackage();
            var originalScript = package.Script;
            string lLengthyScript = Utilities.LoremIpsum(InventoryItemPackage.ScriptMaximumLength + 1000);

            var resultOrError = package.SetScript(lLengthyScript);
            string truncateScript = lLengthyScript.Substring(0, InventoryItemPackage.ScriptMaximumLength);
            resultOrError.IsFailure.Should().BeFalse();
            package.Script.Should().Be(
                truncateScript);
            package.Script.Should().NotBe(originalScript);
        }

        [Fact]
        public void SetIsDiscountable()
        {
            InventoryItemPackage package = InventoryItemHelper.CreateInventoryItemPackage();
            var originalIsDiscountable = package.IsDiscountable;
            var newIsDiscountable = !originalIsDiscountable;

            var resultOrError = package.SetIsDiscountable(newIsDiscountable);

            resultOrError.IsFailure.Should().BeFalse();
            package.IsDiscountable.Should().Be(newIsDiscountable);
            package.IsDiscountable.Should().NotBe(originalIsDiscountable);
        }

        [Fact]
        public void AddItem()
        {
            var package = InventoryItemHelper.CreateInventoryItemPackage();
            var item = InventoryItemHelper.CreateInventoryItemPackageItem();
            package.Items.Count.Should().Be(0);

            var resultOrError = package.AddItem(item);

            resultOrError.IsFailure.Should().BeFalse();
            package.Items.Count.Should().Be(1);
        }

        [Fact]
        public void RemoveItem()
        {
            InventoryItemPackage package = InventoryItemHelper.CreateInventoryItemPackage();
            InventoryItemPackageItem itemOne = InventoryItemHelper.CreateInventoryItemPackageItem();
            InventoryItemPackageItem itemTwo = InventoryItemHelper.CreateInventoryItemPackageItem();
            InventoryItemPackageItem itemThree = InventoryItemHelper.CreateInventoryItemPackageItem();
            package.Items.Count.Should().Be(0);
            package.AddItem(itemOne);
            package.AddItem(itemTwo);
            package.AddItem(itemThree);
            package.Items.Count.Should().Be(3);

            var resultOrError = package.RemoveItem(itemOne);

            resultOrError.IsFailure.Should().BeFalse();
            package.Items.Count.Should().Be(2);
            package.Items.Should().NotContain(itemOne);
            package.Items.Should().Contain(itemTwo);
            package.Items.Should().Contain(itemThree);
        }

        [Fact]
        public void AddPlaceholder()
        {
            var package = InventoryItemHelper.CreateInventoryItemPackage();
            var placeholder = InventoryItemHelper.CreateInventoryItemPackagePlaceholder();
            package.Placeholders.Count.Should().Be(0);

            var resultOrError = package.AddPlaceholder(placeholder);

            resultOrError.IsFailure.Should().BeFalse();
            package.Placeholders.Count.Should().Be(1);
        }

        [Fact]
        public void RemovePlaceholder()
        {
            var package = InventoryItemHelper.CreateInventoryItemPackage();
            var placeholderOne = InventoryItemHelper.CreateInventoryItemPackagePlaceholder();
            var placeholderTwo = InventoryItemHelper.CreateInventoryItemPackagePlaceholder();
            var placeholderThree = InventoryItemHelper.CreateInventoryItemPackagePlaceholder();
            package.Placeholders.Count.Should().Be(0);
            InventoryItemPackageItem itemTwo = InventoryItemHelper.CreateInventoryItemPackageItem();
            InventoryItemPackageItem itemThree = InventoryItemHelper.CreateInventoryItemPackageItem();
            package.AddPlaceholder(placeholderOne);
            package.AddPlaceholder(placeholderTwo);
            package.AddPlaceholder(placeholderThree);
            package.Placeholders.Count.Should().Be(3);

            var resultOrError = package.RemovePlaceholder(placeholderOne);

            resultOrError.IsFailure.Should().BeFalse();
            package.Placeholders.Count.Should().Be(2);
            package.Placeholders.Should().NotContain(placeholderOne);
            package.Placeholders.Should().Contain(placeholderTwo);
            package.Placeholders.Should().Contain(placeholderThree);
        }

    }
}