﻿using Menominee.Domain.Entities.Inventory;
using FluentAssertions;
using System.Collections.Generic;
using TestingHelperLibrary;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class ProductCodeShould
    {
        [Fact]
        public void Create_ProductCode()
        {
            // Arrange
            var name = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var code = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var manufacturer = InventoryItemTestHelper.CreateManufacturer();
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
            var manufacturer = InventoryItemTestHelper.CreateManufacturer();
            var saleCode = InventoryItemTestHelper.CreateSaleCode();
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
            var manufacturer = InventoryItemTestHelper.CreateManufacturer();
            var saleCode = InventoryItemTestHelper.CreateSaleCode();
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
            var manufacturer = InventoryItemTestHelper.CreateManufacturer();
            var saleCode = InventoryItemTestHelper.CreateSaleCode();
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
            var saleCode = InventoryItemTestHelper.CreateSaleCode();
            List<string> manufacturerCodes = new() { "11" };

            var resultOrError = ProductCode.Create(null, code, name, manufacturerCodes, saleCode);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void Not_Create_ProductCode_With_Nonunique_Manufacturer_Code()
        {
            var manufacturer = InventoryItemTestHelper.CreateManufacturer();
            var name = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var code = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var saleCode = InventoryItemTestHelper.CreateSaleCode();
            List<string> manufacturerCodes = new() { $"{manufacturer.Id}{code}" };

            var resultOrError = ProductCode.Create(manufacturer, code, name, manufacturerCodes, saleCode);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("unique");
        }

        [Fact]
        public void SetName()
        {
            var productCode = InventoryItemTestHelper.CreateProductCode();
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
            var productCode = InventoryItemTestHelper.CreateProductCode();
            var invalidName = Utilities.RandomCharacters(ProductCode.MaximumNameLength + 1);

            var resultOrError = productCode.SetName(invalidName);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetCode()
        {
            var productCode = InventoryItemTestHelper.CreateProductCode();
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
            var manufacturer = InventoryItemTestHelper.CreateManufacturer();
            var name = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var code = Utilities.RandomCharacters(ProductCode.MinimumLength);
            var saleCode = InventoryItemTestHelper.CreateSaleCode();
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
            var productCode = InventoryItemTestHelper.CreateProductCode();
            var invalidCode = Utilities.RandomCharacters(ProductCode.MaximumCodeLength + 1);
            List<string> manufacturerCodes = new() { "11" };

            var resultOrError = productCode.SetCode(invalidCode, manufacturerCodes);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("must");
        }

        [Fact]
        public void SetManufacturer()
        {
            var productCode = InventoryItemTestHelper.CreateProductCode();
            var originalManufacturer = productCode.Manufacturer;
            productCode.Manufacturer.Should().Be(originalManufacturer);
            var newManufacturer = InventoryItemTestHelper.CreateManufacturer();
            //List<string> manufacturerPrefix = new() { "11" };

            productCode.SetManufacturer(newManufacturer);

            productCode.Manufacturer.Should().Be(newManufacturer);
            //productCode.Manufacturer.Should().NotBe(originalManufacturer);
        }

        [Fact]
        public void Not_Set_Null_Manufacturer()
        {
            var productCode = InventoryItemTestHelper.CreateProductCode();
            //List<string> manufacturerCodes = new() { "11" };
            var resultOrError = productCode.SetManufacturer(null);

            resultOrError.IsFailure.Should().BeTrue();
            resultOrError.Error.Should().Contain("required");
        }

        [Fact]
        public void SetSaleCode()
        {
            var productCode = InventoryItemTestHelper.CreateProductCode();
            var originalSaleCode = productCode.SaleCode;
            productCode.SaleCode.Should().Be(originalSaleCode);
            var newSaleCode = InventoryItemTestHelper.CreateSaleCode();
            productCode.SetSaleCode(newSaleCode);

            productCode.SaleCode.Should().Be(newSaleCode);
        }

        [Fact]
        public void Not_Set_Null_SaleCode()
        {
            var productCode = InventoryItemTestHelper.CreateProductCode();

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
