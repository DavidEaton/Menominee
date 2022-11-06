using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared;
using CustomerVehicleManagement.Tests.Unit.Helpers;
using FluentAssertions;
using System.Collections.Generic;
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
            List<string> manufacturerCodes = new() { "11" };

            // Act
            var resultOrError = ProductCode.Create(manufacturer, code, name, manufacturerCodes);

            // Assert
            resultOrError.Value.Should().BeOfType<ProductCode>();
            resultOrError.IsSuccess.Should().BeTrue();
            resultOrError.Value.Name.Should().Be(name);
            resultOrError.Value.Code.Should().Be(code);
            resultOrError.Value.Manufacturer.Should().Be(manufacturer);
        }

        [Fact]
        public void Create_ProductCode_With_Optional_SaleCode()
        {
            var name = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var code = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var manufacturer = InventoryItemHelper.CreateManufacturer();
            var saleCode = InventoryItemHelper.CreateSaleCode();
            List<string> manufacturerCodes = new()
            {
                "11"
            };

            var resultOrError = ProductCode.Create(manufacturer, code, name, manufacturerCodes, saleCode);

            resultOrError.Value.Should().BeOfType<ProductCode>();
            resultOrError.IsSuccess.Should().BeTrue();
            resultOrError.Value.Name.Should().Be(name);
            resultOrError.Value.Code.Should().Be(code);
            resultOrError.Value.Manufacturer.Should().Be(manufacturer);
            resultOrError.Value.SaleCode.Should().Be(saleCode);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_ProductCode_With_Invalid_Name(int length)
        {
            var invalidName = Utilities.RandomCharacters(length);
            var code = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var manufacturer = InventoryItemHelper.CreateManufacturer();
            var saleCode = InventoryItemHelper.CreateSaleCode();
            List<string> manufacturerCodes = new() { "11" };

            var resultOrError = ProductCode.Create(manufacturer, code, invalidName, manufacturerCodes, saleCode);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_ProductCode_With_Invalid_Code(int length)
        {
            var name = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var invalidCode = Utilities.RandomCharacters(length);
            var manufacturer = InventoryItemHelper.CreateManufacturer();
            var saleCode = InventoryItemHelper.CreateSaleCode();
            List<string> manufacturerCodes = new() { "11" };

            var resultOrError = ProductCode.Create(manufacturer, invalidCode, name, manufacturerCodes, saleCode);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_ProductCode_With_Null_Manufacturer()
        {
            var name = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var code = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var saleCode = InventoryItemHelper.CreateSaleCode();
            List<string> manufacturerCodes = new() { "11" };

            var resultOrError = ProductCode.Create(null, code, name, manufacturerCodes, saleCode);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Create_ProductCode_With_Nonunique_Manufacturer_Code()
        {
            var manufacturer = InventoryItemHelper.CreateManufacturer();
            var name = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var code = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var saleCode = InventoryItemHelper.CreateSaleCode();
            List<string> manufacturerCodes = new() { $"{manufacturer.Id}{code}" };

            var resultOrError = ProductCode.Create(manufacturer, code, name, manufacturerCodes, saleCode);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("unique");
        }

        [Fact]
        public void SetName()
        {
            var productCode = InventoryItemHelper.CreateProductCode();
            string originalName = Utilities.RandomCharacters(ProductCode.MinimumLength + 1);
            var newName = Utilities.RandomCharacters(originalName.Length);

            var resultOrError = productCode.SetName(newName);

            resultOrError.IsFailure.Should().BeFalse();
            resultOrError.Value.Should().Be(newName);
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
            List<string> manufacturerCodes = new() { "11" };

            var resultOrError = productCode.SetCode(newCode, manufacturerCodes);

            resultOrError.IsFailure.Should().BeFalse();
            resultOrError.Value.Should().Be(newCode);
            productCode.Code.Should().Be(newCode);
        }

        [Fact]
        public void Not_Set_Code_With_Nonunique_Manufacturer_Code()
        {
            var manufacturer = InventoryItemHelper.CreateManufacturer();
            var name = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var code = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var saleCode = InventoryItemHelper.CreateSaleCode();
            List<string> manufacturerCodes = new() { "11" };
            var productCode = ProductCode.Create(manufacturer, code, name, manufacturerCodes, saleCode).Value;
            manufacturerCodes.Add($"{manufacturer.Id}{code}");

            var resultOrError = productCode.SetCode(code, manufacturerCodes);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("unique");
        }

        [Fact]
        public void Not_Set_Invalid_Code()
        {
            var productCode = InventoryItemHelper.CreateProductCode();
            var invalidCode = Utilities.RandomCharacters(ProductCode.MaximumCodeLength + 1);
            List<string> manufacturerCodes = new() { "11" };

            var resultOrError = productCode.SetCode(invalidCode, manufacturerCodes);

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
            List<string> manufacturerCodes = new() { "11" };

            productCode.SetManufacturer(newManufacturer, manufacturerCodes);

            productCode.Manufacturer.Should().Be(newManufacturer);
            productCode.Manufacturer.Should().NotBe(originalManufacturer);
        }

        [Fact]
        public void Not_Set_Null_Manufacturer()
        {
            var productCode = InventoryItemHelper.CreateProductCode();
            List<string> manufacturerCodes = new() { "11" };
            var resultOrError = productCode.SetManufacturer(null, manufacturerCodes);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Set_Manufacturer_With_Nonunique_Manufacturer_Code()
        {
            var productCode = InventoryItemHelper.CreateProductCode();
            var newManufacturer = InventoryItemHelper.CreateManufacturer();

            List<string> manufacturerCodes = new() { $"{newManufacturer.Id}{productCode.Code}" };
            var resultOrError = productCode.SetManufacturer(newManufacturer, manufacturerCodes);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("unique");
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
