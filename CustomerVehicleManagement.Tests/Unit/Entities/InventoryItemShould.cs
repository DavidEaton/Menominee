using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Tests.Unit.Helpers;
using FluentAssertions;
using Menominee.Common.Enums;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class InventoryItemShould
    {
        [Fact]
        public void Create_InventoryItem()
        {
            // Arrange
            Manufacturer manufacturer = InventoryItemTestHelper.CreateManufacturer();
            ProductCode productCode = InventoryItemTestHelper.CreateProductCode();
            InventoryItemPart part = InventoryItemTestHelper.CreateInventoryItemPart();

            // Act
            var resultOrError = InventoryItem.Create(manufacturer, "001", "a description", productCode, InventoryItemType.Part, part: part);

            // Assert
            resultOrError.Value.Should().BeOfType<InventoryItem>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Not_Create_InventoryItem_With_Null_Manufacturer()
        {
            ProductCode productCode = InventoryItemTestHelper.CreateProductCode();
            InventoryItemPart part = InventoryItemTestHelper.CreateInventoryItemPart();

            var resultOrError = InventoryItem.Create(null, "001", "a description", productCode, InventoryItemType.Part, part: part);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Create_InventoryItem_With_Null_ProductCode()
        {
            Manufacturer manufacturer = InventoryItemTestHelper.CreateManufacturer();
            InventoryItemPart part = InventoryItemTestHelper.CreateInventoryItemPart();

            var resultOrError = InventoryItem.Create(manufacturer, "001", "a description", null, InventoryItemType.Part, part: part);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_InventoryItem_With_Invalid_ItemNumber(int invalidLength)
        {
            Manufacturer manufacturer = InventoryItemTestHelper.CreateManufacturer();
            ProductCode productCode = InventoryItemTestHelper.CreateProductCode();
            InventoryItemPart part = InventoryItemTestHelper.CreateInventoryItemPart();
            string invalidItemNumber = Utilities.RandomCharacters(invalidLength);

            var resultOrError = InventoryItem.Create(manufacturer, invalidItemNumber, "a description", productCode, InventoryItemType.Part, part: part);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_InventoryItem_With_Invalid_Description(int invalidLength)
        {
            Manufacturer manufacturer = InventoryItemTestHelper.CreateManufacturer();
            ProductCode productCode = InventoryItemTestHelper.CreateProductCode();
            InventoryItemPart part = InventoryItemTestHelper.CreateInventoryItemPart();
            string invalidDescription = Utilities.RandomCharacters(invalidLength);

            var resultOrError = InventoryItem.Create(manufacturer, "001", invalidDescription, productCode, InventoryItemType.Part, part: part);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_InventoryItem_With_Invalid_InventoryItemType()
        {
            Manufacturer manufacturer = InventoryItemTestHelper.CreateManufacturer();
            ProductCode productCode = InventoryItemTestHelper.CreateProductCode();
            InventoryItemPart part = InventoryItemTestHelper.CreateInventoryItemPart();
            InventoryItemType invalid = (InventoryItemType)99;

            var resultOrError = InventoryItem.Create(manufacturer, "001", "a description", productCode, invalid, part: part);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Create_InventoryItem_With_Null_InventoryItemType_Part()
        {
            Manufacturer manufacturer = InventoryItemTestHelper.CreateManufacturer();
            ProductCode productCode = InventoryItemTestHelper.CreateProductCode();

            var resultOrError = InventoryItem.Create(manufacturer, "001", "a description", productCode, InventoryItemType.Part, part: null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Create_InventoryItem_With_Null_InventoryItemType_Labor()
        {
            Manufacturer manufacturer = InventoryItemTestHelper.CreateManufacturer();
            ProductCode productCode = InventoryItemTestHelper.CreateProductCode();

            var resultOrError = InventoryItem.Create(manufacturer, "001", "a description", productCode, InventoryItemType.Labor, labor: null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Create_InventoryItem_With_Null_InventoryItemType_Inspection()
        {
            Manufacturer manufacturer = InventoryItemTestHelper.CreateManufacturer();
            ProductCode productCode = InventoryItemTestHelper.CreateProductCode();

            var resultOrError = InventoryItem.Create(manufacturer, "001", "a description", productCode, InventoryItemType.Inspection, inspection: null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Create_InventoryItem_With_Null_InventoryItemType_Package()
        {
            Manufacturer manufacturer = InventoryItemTestHelper.CreateManufacturer();
            ProductCode productCode = InventoryItemTestHelper.CreateProductCode();

            var resultOrError = InventoryItem.Create(manufacturer, "001", "a description", productCode, InventoryItemType.Package, package: null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Create_InventoryItem_With_Null_InventoryItemType_Tire()
        {
            Manufacturer manufacturer = InventoryItemTestHelper.CreateManufacturer();
            ProductCode productCode = InventoryItemTestHelper.CreateProductCode();

            var resultOrError = InventoryItem.Create(manufacturer, "001", "a description", productCode, InventoryItemType.Tire, tire: null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Create_InventoryItem_With_Null_InventoryItemType_Warranty()
        {
            Manufacturer manufacturer = InventoryItemTestHelper.CreateManufacturer();
            ProductCode productCode = InventoryItemTestHelper.CreateProductCode();

            var resultOrError = InventoryItem.Create(manufacturer, "001", "a description", productCode, InventoryItemType.Warranty, warranty: null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetManufacturer()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            Manufacturer originalManufacturer = item.Manufacturer;
            Manufacturer newManufacturer = InventoryItemTestHelper.CreateManufacturer();
            item.Manufacturer.Should().Be(originalManufacturer);
            item.Manufacturer.Should().NotBe(newManufacturer);

            var resultOrError = item.SetManufacturer(newManufacturer);

            resultOrError.IsFailure.Should().BeFalse();
            item.Manufacturer.Should().Be(newManufacturer);
            item.Manufacturer.Should().NotBe(originalManufacturer);
        }

        [Fact]
        public void Not_Set_Null_Manufacturer()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            Manufacturer originalManufacturer = item.Manufacturer;
            Manufacturer newManufacturer = null;
            item.Manufacturer.Should().Be(originalManufacturer);
            item.Manufacturer.Should().NotBe(newManufacturer);

            var resultOrError = item.SetManufacturer(newManufacturer);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetProductCode()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            ProductCode originalProductCode = item.ProductCode;
            ProductCode newProductCode = InventoryItemTestHelper.CreateProductCode();
            item.ProductCode.Should().Be(originalProductCode);
            item.ProductCode.Should().NotBe(newProductCode);

            var resultOrError = item.SetProductCode(newProductCode);

            resultOrError.IsFailure.Should().BeFalse();
            item.ProductCode.Should().Be(newProductCode);
            item.ProductCode.Should().NotBe(originalProductCode);
        }

        [Fact]
        public void Not_Set_Null_ProductCode()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            ProductCode originalProductCode = item.ProductCode;
            ProductCode newProductCode = null;
            item.ProductCode.Should().Be(originalProductCode);
            item.ProductCode.Should().NotBe(newProductCode);

            var resultOrError = item.SetProductCode(newProductCode);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetItemNumber()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            string originalItemNumber = item.ItemNumber;
            string newItemNumber = Utilities.RandomCharacters(InventoryItem.MinimumLength + 1);
            item.ItemNumber.Should().Be(originalItemNumber);
            item.ItemNumber.Should().NotBe(newItemNumber);

            var resultOrError = item.SetItemNumber(newItemNumber);

            resultOrError.IsFailure.Should().BeFalse();
            item.ItemNumber.Should().Be(newItemNumber);
            item.ItemNumber.Should().NotBe(originalItemNumber);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_ItemNumber(int invalidLength)
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            string originalItemNumber = item.ItemNumber;
            string invalidItemNumber = Utilities.RandomCharacters(invalidLength);
            item.ItemNumber.Should().Be(originalItemNumber);
            item.ItemNumber.Should().NotBe(invalidItemNumber);

            var resultOrError = item.SetItemNumber(invalidItemNumber);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetDescription()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            string originalDescription = item.Description;
            string newDescription = Utilities.RandomCharacters(InventoryItem.MinimumLength + 1);
            item.Description.Should().Be(originalDescription);
            item.Description.Should().NotBe(newDescription);

            var resultOrError = item.SetDescription(newDescription);

            resultOrError.IsFailure.Should().BeFalse();
            item.Description.Should().Be(newDescription);
            item.Description.Should().NotBe(originalDescription);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Description(int invalidLength)
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            string originalDescription = item.Description;
            string invalidDescription = Utilities.RandomCharacters(invalidLength);
            item.Description.Should().Be(originalDescription);
            item.Description.Should().NotBe(invalidDescription);

            var resultOrError = item.SetDescription(invalidDescription);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetPart()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            InventoryItemPart originalPart = item.Part;
            InventoryItemPart newPart = InventoryItemTestHelper.CreateInventoryItemPart();
            item.ItemType.Should().Be(InventoryItemType.Part);
            item.Part.Should().Be(originalPart);

            var resultOrError = item.SetPart(newPart);

            resultOrError.IsFailure.Should().BeFalse();
            item.Part.Should().Be(newPart);
            item.Part.Should().NotBe(originalPart);
        }

        [Fact]
        public void Not_Set_Null_Part()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            InventoryItemPart originalPart = item.Part;
            InventoryItemPart newPart = null;
            item.ItemType.Should().Be(InventoryItemType.Part);
            item.Part.Should().Be(originalPart);

            var resultOrError = item.SetPart(newPart);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetLabor()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            item.ItemType.Should().Be(InventoryItemType.Part);
            item.Labor.Should().BeNull();
            var labor = InventoryItemTestHelper.CreateInventoryItemLabor();

            var resultOrError = item.SetLabor(labor);

            resultOrError.IsFailure.Should().BeFalse();
            item.Labor.Should().Be(labor);
        }

        [Fact]
        public void Not_Set_Null_Labor()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            item.ItemType.Should().Be(InventoryItemType.Part);
            item.Labor.Should().BeNull();
            InventoryItemLabor labor = null;

            var resultOrError = item.SetLabor(labor);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetTire()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            item.ItemType.Should().Be(InventoryItemType.Part);
            item.Tire.Should().BeNull();
            var tire = InventoryItemTestHelper.CreateInventoryItemTire();


            var resultOrError = item.SetTire(tire);

            resultOrError.IsFailure.Should().BeFalse();
            item.Tire.Should().Be(tire);
        }

        [Fact]
        public void Not_Set_Null_Tire()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            item.ItemType.Should().Be(InventoryItemType.Part);
            item.Tire.Should().BeNull();
            InventoryItemTire tire = null;


            var resultOrError = item.SetTire(tire);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetPackage()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            item.ItemType.Should().Be(InventoryItemType.Part);
            item.Package.Should().BeNull();
            InventoryItemPackage package = InventoryItemTestHelper.CreateInventoryItemPackage();

            var resultOrError = item.SetPackage(package);

            resultOrError.IsFailure.Should().BeFalse();
            item.Package.Should().Be(package);
        }

        [Fact]
        public void Not_Set_Null_Package()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            item.ItemType.Should().Be(InventoryItemType.Part);
            item.Package.Should().BeNull();
            InventoryItemPackage package = null;

            var resultOrError = item.SetPackage(package);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetInspection()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            InventoryItemPart originalPart = item.Part;
            InventoryItemInspection inspection = InventoryItemTestHelper.CreateInventoryItemInspection();
            item.Part.Should().Be(originalPart);
            item.Inspection.Should().BeNull();

            var resultOrError = item.SetInspection(inspection);

            resultOrError.IsFailure.Should().BeFalse();
            item.Inspection.Should().Be(inspection);
        }

        [Fact]
        public void Not_Set_Null_Inspection()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            InventoryItemPart originalPart = item.Part;
            InventoryItemInspection inspection = null;
            item.Part.Should().Be(originalPart);
            item.Inspection.Should().BeNull();

            var resultOrError = item.SetInspection(inspection);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetWarranty()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            InventoryItemPart originalPart = item.Part;
            InventoryItemWarranty warranty = InventoryItemTestHelper.CreateInventoryItemWarranty();
            item.Part.Should().Be(originalPart);
            item.Warranty.Should().BeNull();

            var resultOrError = item.SetWarranty(warranty);

            resultOrError.IsFailure.Should().BeFalse();
            item.Warranty.Should().Be(warranty);
        }

        [Fact]
        public void Not_Set_Null_Warranty()
        {
            InventoryItem item = InventoryItemTestHelper.CreateInventoryItem();
            InventoryItemPart originalPart = item.Part;
            InventoryItemWarranty warranty = null;
            item.Part.Should().Be(originalPart);
            item.Warranty.Should().BeNull();

            var resultOrError = item.SetWarranty(warranty);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { InventoryItem.MaximumLength + 1 };
                    yield return new object[] { InventoryItem.MinimumLength - 1 };
                }
            }
        }
    }
}
