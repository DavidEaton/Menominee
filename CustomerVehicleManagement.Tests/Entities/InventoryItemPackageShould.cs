using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentAssertions;
using System.Collections.Generic;
using TestingHelperLibrary;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class InventoryItemPackageShould
    {
        private const string InvalidOverMaximumLength = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in"; // 256 characters
        private const string InvalidZeroLengthString = "";
        private const string InvalidUnderMinimumLength = "1";
        private const int InvalidMinimumAmount = -1;
        private const int InvalidMaximumAmount = 100000;

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

        [Theory]
        [InlineData(InvalidZeroLengthString)]
        [InlineData(InvalidUnderMinimumLength)]
        [InlineData(InvalidOverMaximumLength)]
        public void Not_Create_InventoryItemPackage_With_Invalid_Script(string script)
        {
            double basePartsAmount = InventoryItemPackage.MinimumAmount;
            double baseLaborAmount = InventoryItemPackage.MinimumAmount - .01;
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
            InventoryItemPackage package = InventoryItemTestHelper.CreateInventoryItemPackage();
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
            InventoryItemPackage package = InventoryItemTestHelper.CreateInventoryItemPackage();
            double invalidBaseLaborAmount = InventoryItemPackage.MaximumAmount + .01;

            var resultOrError = package.SetBaseLaborAmount(invalidBaseLaborAmount);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetBasePartsAmount()
        {
            InventoryItemPackage package = InventoryItemTestHelper.CreateInventoryItemPackage();
            var originalAmount = package.BasePartsAmount;
            double newBasePartsAmount = InventoryItemPackage.MinimumAmount + .01;

            var resultOrError = package.SetBasePartsAmount(newBasePartsAmount);

            resultOrError.IsFailure.Should().BeFalse();
            package.BasePartsAmount.Should().Be(newBasePartsAmount);
            package.BasePartsAmount.Should().NotBe(originalAmount);
        }

        [Theory]
        [InlineData(InvalidMinimumAmount)]
        [InlineData(InvalidMaximumAmount)]
        public void Not_Set_Invalid_BasePartsAmount(double basePartsAmount)
        {
            InventoryItemPackage package = InventoryItemTestHelper.CreateInventoryItemPackage();

            var resultOrError = package.SetBasePartsAmount(basePartsAmount);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Be(InventoryItemPackage.InvalidAmountMessage);
        }

        [Fact]
        public void SetScript()
        {
            InventoryItemPackage package = InventoryItemTestHelper.CreateInventoryItemPackage();
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
            InventoryItemPackage package = InventoryItemTestHelper.CreateInventoryItemPackage();
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
            InventoryItemPackage package = InventoryItemTestHelper.CreateInventoryItemPackage();
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
            var package = InventoryItemTestHelper.CreateInventoryItemPackage();
            var item = InventoryItemTestHelper.CreateInventoryItemPackageItem();
            package.Items.Count.Should().Be(0);

            var resultOrError = package.AddItem(item);

            resultOrError.IsFailure.Should().BeFalse();
            package.Items.Count.Should().Be(1);
        }

        [Fact]
        public void Not_Add_Invalid_Item()
        {
            var package = InventoryItemTestHelper.CreateInventoryItemPackage();
            InventoryItemPackageItem invalidItem = null;
            package.Items.Count.Should().Be(0);

            var resultOrError = package.AddItem(invalidItem);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Be(InventoryItemPackage.RequiredMessage);
        }

        [Fact]
        public void RemoveItem()
        {
            InventoryItemPackage package = InventoryItemTestHelper.CreateInventoryItemPackage();
            InventoryItemPackageItem itemOne = InventoryItemTestHelper.CreateInventoryItemPackageItem();
            InventoryItemPackageItem itemTwo = InventoryItemTestHelper.CreateInventoryItemPackageItem();
            InventoryItemPackageItem itemThree = InventoryItemTestHelper.CreateInventoryItemPackageItem();
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
        public void Not_Remove_Invalid_Item()
        {
            InventoryItemPackage package = InventoryItemTestHelper.CreateInventoryItemPackage();
            InventoryItemPackageItem invalidItem = null;
            InventoryItemPackageItem itemTwo = InventoryItemTestHelper.CreateInventoryItemPackageItem();
            InventoryItemPackageItem itemThree = InventoryItemTestHelper.CreateInventoryItemPackageItem();
            package.Items.Count.Should().Be(0);
            package.AddItem(itemTwo);
            package.AddItem(itemThree);
            package.Items.Count.Should().Be(2);

            var resultOrError = package.RemoveItem(invalidItem);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Be(InventoryItemPackage.RequiredMessage);
        }

        [Fact]
        public void AddPlaceholder()
        {
            var package = InventoryItemTestHelper.CreateInventoryItemPackage();
            var placeholder = InventoryItemTestHelper.CreateInventoryItemPackagePlaceholder();
            package.Placeholders.Count.Should().Be(0);

            var resultOrError = package.AddPlaceholder(placeholder);

            resultOrError.IsFailure.Should().BeFalse();
            package.Placeholders.Count.Should().Be(1);
        }

        [Fact]
        public void Not_Add_Invalid_Placeholder()
        {
            var package = InventoryItemTestHelper.CreateInventoryItemPackage();
            InventoryItemPackagePlaceholder invalidPlaceholder = null;
            package.Placeholders.Count.Should().Be(0);

            var resultOrError = package.AddPlaceholder(invalidPlaceholder);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Be(InventoryItemPackage.RequiredMessage);
        }

        [Fact]
        public void RemovePlaceholder()
        {
            var package = InventoryItemTestHelper.CreateInventoryItemPackage();
            var placeholderOne = InventoryItemTestHelper.CreateInventoryItemPackagePlaceholder();
            var placeholderTwo = InventoryItemTestHelper.CreateInventoryItemPackagePlaceholder();
            var placeholderThree = InventoryItemTestHelper.CreateInventoryItemPackagePlaceholder();
            package.Placeholders.Count.Should().Be(0);
            InventoryItemPackageItem itemTwo = InventoryItemTestHelper.CreateInventoryItemPackageItem();
            InventoryItemPackageItem itemThree = InventoryItemTestHelper.CreateInventoryItemPackageItem();
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

        [Fact]
        public void Not_Remove_Invalid_Placeholder()
        {
            var package = InventoryItemTestHelper.CreateInventoryItemPackage();
            var placeholderOne = InventoryItemTestHelper.CreateInventoryItemPackagePlaceholder();
            var placeholderTwo = InventoryItemTestHelper.CreateInventoryItemPackagePlaceholder();
            InventoryItemPackagePlaceholder invalidPlaceholder = null;
            package.Placeholders.Count.Should().Be(0);
            package.AddPlaceholder(placeholderOne);
            package.AddPlaceholder(placeholderTwo);
            package.Placeholders.Count.Should().Be(2);

            var resultOrError = package.RemovePlaceholder(invalidPlaceholder);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Be(InventoryItemPackage.RequiredMessage);
        }

    }
}
