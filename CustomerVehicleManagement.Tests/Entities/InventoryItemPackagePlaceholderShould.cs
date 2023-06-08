using CustomerVehicleManagement.Domain.Entities.Inventory;
using FluentAssertions;
using Menominee.Common.Enums;
using System.Collections.Generic;
using TestingHelperLibrary;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class InventoryItemPackagePlaceholderShould
    {
        [Fact]
        public void Create_InventoryItemPackagePlaceholder()
        {
            // Arrange
            var details = InventoryItemTestHelper.CreateInventoryItemPackageDetails();

            // Act
            var resultOrError = InventoryItemPackagePlaceholder.Create(
                PackagePlaceholderItemType.Part,
                "description",
                InventoryItemPackagePlaceholder.DescriptionMinimumLength + 1,
                details);

            // Assert
            resultOrError.Value.Should().BeOfType<InventoryItemPackagePlaceholder>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Not_Create_InventoryItemPackagePlaceholder_With_Invalid_ItemType()
        {
            var details = InventoryItemTestHelper.CreateInventoryItemPackageDetails();
            var invalidItemType = (PackagePlaceholderItemType)(-1);

            var resultOrError = InventoryItemPackagePlaceholder.Create(
                invalidItemType,
                "description",
                InventoryItemPackagePlaceholder.DescriptionMinimumLength + 1,
                details);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_InventoryItemPackagePlaceholder_With_Invalid_Description(int invalidDescriptionLength)
        {
            var details = InventoryItemTestHelper.CreateInventoryItemPackageDetails();

            var resultOrError = InventoryItemPackagePlaceholder.Create(
                PackagePlaceholderItemType.Part,
                Utilities.LoremIpsum(invalidDescriptionLength),
                InventoryItemPackagePlaceholder.DescriptionMinimumLength + 1,
                details);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_InventoryItemPackagePlaceholder_With_Invalid_DisplayOrder()
        {
            var details = InventoryItemTestHelper.CreateInventoryItemPackageDetails();
            var invalidDisplayOrder = 0;
            var resultOrError = InventoryItemPackagePlaceholder.Create(
                PackagePlaceholderItemType.Part,
                "description",
                invalidDisplayOrder,
                details);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_InventoryItemPackagePlaceholder_With_Null_Details()
        {
            var resultOrError = InventoryItemPackagePlaceholder.Create(
                PackagePlaceholderItemType.Part,
                "description",
                InventoryItemPackagePlaceholder.DescriptionMinimumLength + 1,
                null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetItemType()
        {
            var placeholder = InventoryItemTestHelper.CreateInventoryItemPackagePlaceholder();
            var originalItemType = placeholder.ItemType;
            var newItemType = PackagePlaceholderItemType.Labor;
            placeholder.ItemType.Should().Be(originalItemType);
            placeholder.ItemType.Should().Be(PackagePlaceholderItemType.Part);

            var resultOrError = placeholder.SetItemType(newItemType);

            resultOrError.IsFailure.Should().BeFalse();
            placeholder.ItemType.Should().Be(newItemType);
            placeholder.ItemType.Should().Be(PackagePlaceholderItemType.Labor);
        }

        [Fact]
        public void Not_Set_Invalid_ItemType()
        {
            var placeholder = InventoryItemTestHelper.CreateInventoryItemPackagePlaceholder();
            var originalItemType = placeholder.ItemType;
            var invalidItemType = (PackagePlaceholderItemType)(-1);
            placeholder.ItemType.Should().Be(originalItemType);
            placeholder.ItemType.Should().Be(PackagePlaceholderItemType.Part);

            var resultOrError = placeholder.SetItemType(invalidItemType);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetDisplayOrder()
        {
            var placeholder = InventoryItemTestHelper.CreateInventoryItemPackagePlaceholder();
            var originalDisplayOrder = placeholder.DisplayOrder;
            var newDisplayOrder = originalDisplayOrder + 1;
            placeholder.DisplayOrder.Should().Be(originalDisplayOrder);

            var resultOrError = placeholder.SetDisplayOrder(newDisplayOrder);

            resultOrError.IsFailure.Should().BeFalse();
            placeholder.DisplayOrder.Should().Be(newDisplayOrder);
            placeholder.DisplayOrder.Should().NotBe(originalDisplayOrder);
        }

        [Fact]
        public void Not_Set_Invalid_DisplayOrder()
        {
            var placeholder = InventoryItemTestHelper.CreateInventoryItemPackagePlaceholder();
            var originalDisplayOrder = placeholder.DisplayOrder;
            var invalidDisplayOrder = 0;
            placeholder.DisplayOrder.Should().Be(originalDisplayOrder);

            var resultOrError = placeholder.SetDisplayOrder(invalidDisplayOrder);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetDescription()
        {
            var placeholder = InventoryItemTestHelper.CreateInventoryItemPackagePlaceholder();
            var originalDescription = placeholder.Description;
            var newDescription = Utilities.LoremIpsum(InventoryItemPackagePlaceholder.DescriptionMaximumLength - 10);
            placeholder.Description.Should().Be(originalDescription);

            var resultOrError = placeholder.SetDescription(newDescription);

            resultOrError.IsFailure.Should().BeFalse();
            placeholder.Description.Should().Be(newDescription);
            placeholder.Description.Should().NotBe(originalDescription);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Description(int invalidDescriptionLength)
        {
            var placeholder = InventoryItemTestHelper.CreateInventoryItemPackagePlaceholder();
            var originalDescription = placeholder.Description;
            var invalidDescription = Utilities.LoremIpsum(invalidDescriptionLength);
            placeholder.Description.Should().Be(originalDescription);

            var resultOrError = placeholder.SetDescription(invalidDescription);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetDetails()
        {
            var placeholder = InventoryItemTestHelper.CreateInventoryItemPackagePlaceholder();
            var originalDetails = placeholder.Details;
            var newDetails = InventoryItemTestHelper.CreateInventoryItemPackageDetails();
            newDetails = newDetails.NewQuantity(newDetails.Quantity + 1.0).Value;
            placeholder.Details.Should().Be(originalDetails);

            var resultOrError = placeholder.SetDetails(newDetails);

            resultOrError.IsFailure.Should().BeFalse();

            originalDetails.Quantity.Should().NotBe(newDetails.Quantity);

            placeholder.Details.Should().Be(newDetails);
            placeholder.Details.Should().NotBe(originalDetails);
        }

        [Fact]
        public void Not_Set_Null_Details()
        {
            var placeholder = InventoryItemTestHelper.CreateInventoryItemPackagePlaceholder();
            var originalDetails = placeholder.Details;
            placeholder.Details.Should().Be(originalDetails);

            var resultOrError = placeholder.SetDetails(null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { InventoryItemPackagePlaceholder.DescriptionMinimumLength - 1 };
                    yield return new object[] { InventoryItemPackagePlaceholder.DescriptionMaximumLength + 1 };
                }
            }
        }

    }
}
