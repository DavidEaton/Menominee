using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.TestUtilities;
using CustomerVehicleManagement.Tests.Unit.Helpers;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class ProductCodeShould
    {
        [Fact]
        public void Create_ProductCode()
        {
            // Arrange
            var name = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var code = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var manufacturer = InventoryItemHelper.CreateManufacturer();

            // Act
            var productCodeOrError = ProductCode.Create(manufacturer, code, name);

            // Assert
            productCodeOrError.Value.Should().BeOfType<ProductCode>();
            productCodeOrError.IsSuccess.Should().BeTrue();
            productCodeOrError.Value.Name.Should().Be(name);
            productCodeOrError.Value.Code.Should().Be(code);
            productCodeOrError.Value.Manufacturer.Should().Be(manufacturer);
        }

        [Fact]
        public void Create_ProductCode_With_Optional_SaleCode()
        {
            var name = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var code = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var manufacturer = InventoryItemHelper.CreateManufacturer();
            var saleCode = InventoryItemHelper.CreateSaleCode();

            var productCodeOrError = ProductCode.Create(manufacturer, code, name, saleCode);

            productCodeOrError.Value.Should().BeOfType<ProductCode>();
            productCodeOrError.IsSuccess.Should().BeTrue();
            productCodeOrError.Value.Name.Should().Be(name);
            productCodeOrError.Value.Code.Should().Be(code);
            productCodeOrError.Value.Manufacturer.Should().Be(manufacturer);
            productCodeOrError.Value.SaleCode.Should().Be(saleCode);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_ProductCode_With_Invalid_Name(int length)
        {
            var invalidName = Utilities.RandomCharacters(length);
            var code = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var manufacturer = InventoryItemHelper.CreateManufacturer();
            var saleCode = InventoryItemHelper.CreateSaleCode();

            var productCode = ProductCode.Create(manufacturer, code, invalidName, saleCode);

            productCode.IsFailure.Should().BeTrue();
            productCode.Error.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_ProductCode_With_Invalid_Code(int length)
        {
            var name = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var invalidCode = Utilities.RandomCharacters(length);
            var manufacturer = InventoryItemHelper.CreateManufacturer();
            var saleCode = InventoryItemHelper.CreateSaleCode();

            var productCode = ProductCode.Create(manufacturer, invalidCode, name, saleCode);

            productCode.IsFailure.Should().BeTrue();
            productCode.Error.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Not_Create_ProductCode_With_Null_Manufacturer()
        {
            var name = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var code = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var saleCode = InventoryItemHelper.CreateSaleCode();

            var productCode = ProductCode.Create(null, code, name, saleCode);

            productCode.IsFailure.Should().BeTrue();
            productCode.Error.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void SetName()
        {
            var productCode = InventoryItemHelper.CreateProductCode();
            string originalName = Utilities.RandomCharacters(ProductCode.MinimumLength + 1);
            var newName = Utilities.RandomCharacters(originalName.Length);

            productCode.SetName(newName);

            productCode.Name.Should().Be(newName);
        }

        [Fact]
        public void Not_Set_Invalid_Name()
        {
            var productCode = InventoryItemHelper.CreateProductCode();
            var invalidName = Utilities.RandomCharacters(ProductCode.MaximumNameLength + 1);

            var resultOrError = productCode.SetName(invalidName);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetCode()
        {
            var productCode = InventoryItemHelper.CreateProductCode();
            var originalCode = productCode.Code;
            productCode.Code.Should().Be(originalCode);
            var newCode = Utilities.RandomCharacters(productCode.Code.Length);

            productCode.SetCode(newCode);

            productCode.Code.Should().Be(newCode);
        }

        [Fact]
        public void Not_Set_Invalid_Code()
        {
            var productCode = InventoryItemHelper.CreateProductCode();
            var invalidCode = Utilities.RandomCharacters(ProductCode.MaximumCodeLength + 1);

            var resultOrError = productCode.SetCode(invalidCode);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetManufacturer()
        {
            var productCode = InventoryItemHelper.CreateProductCode();
            var originalManufacturer = productCode.Manufacturer;
            productCode.Manufacturer.Should().Be(originalManufacturer);
            var newManufacturer = InventoryItemHelper.CreateManufacturer();

            productCode.SetManufacturer(newManufacturer);

            productCode.Manufacturer.Should().Be(newManufacturer);
            productCode.Manufacturer.Should().NotBe(originalManufacturer);
        }

        [Fact]
        public void Not_Set_Null_Manufacturer()
        {
            var productCode = InventoryItemHelper.CreateProductCode();
            var resultOrError = productCode.SetManufacturer(null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetSaleCode()
        {
            var productCode = InventoryItemHelper.CreateProductCode();
            var originalSaleCode = productCode.SaleCode;
            productCode.SaleCode.Should().Be(originalSaleCode);
            var newSaleCode = InventoryItemHelper.CreateSaleCode();
            productCode.SetSaleCode(newSaleCode);

            productCode.SaleCode.Should().Be(newSaleCode);
        }

        [Fact]
        public void Not_Set_Null_SaleCode()
        {
            var productCode = InventoryItemHelper.CreateProductCode();

            var resultOrError = productCode.SetSaleCode(null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }


        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { ProductCode.MinimumLength - 1 };
                    yield return new object[] { ProductCode.MaximumNameLength + 1 };
                }
            }
        }

    }
}
