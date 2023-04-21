﻿using CustomerVehicleManagement.Domain.Entities;

using FluentAssertions;
using System.Collections.Generic;
using TestingHelperLibrary;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class SaleCodeShould
    {
        [Fact]
        public void Create_SaleCode()
        {
            // Arrange
            string name = Utilities.RandomCharacters(SaleCode.MinimumLength);
            string code = Utilities.RandomCharacters(SaleCode.MinimumLength);
            double laborRate = SaleCode.MinimumValue;
            double desiredMargin = SaleCode.MinimumValue;
            SaleCodeShopSupplies shopSupplies = new();

            // Act
            var saleCodeOrError = SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies);

            // Assert
            saleCodeOrError.Value.Should().BeOfType<SaleCode>();
            saleCodeOrError.IsSuccess.Should().BeTrue();
            saleCodeOrError.Value.Name.Should().Be(name);
            saleCodeOrError.Value.Code.Should().Be(code);
            saleCodeOrError.Value.LaborRate.Should().Be(laborRate);
            saleCodeOrError.Value.DesiredMargin.Should().Be(desiredMargin);
            saleCodeOrError.Value.ShopSupplies.Should().Be(shopSupplies);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_SaleCode_With_Invalid_Name(int length)
        {
            string invalidName = Utilities.RandomCharacters(length);
            string code = Utilities.RandomCharacters(SaleCode.MinimumLength);
            double laborRate = SaleCode.MinimumValue;
            double desiredMargin = SaleCode.MinimumValue;
            SaleCodeShopSupplies shopSupplies = new();

            var saleCode = SaleCode.Create(invalidName, code, laborRate, desiredMargin, shopSupplies);

            saleCode.IsFailure.Should().BeTrue();
            saleCode.Error.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_SaleCode_With_Invalid_Code(int length)
        {
            string name = Utilities.RandomCharacters(SaleCode.MinimumLength);
            string invalidCode = Utilities.RandomCharacters(length);
            double laborRate = SaleCode.MinimumValue;
            double desiredMargin = SaleCode.MinimumValue;
            SaleCodeShopSupplies shopSupplies = new();

            var saleCode = SaleCode.Create(name, invalidCode, laborRate, desiredMargin, shopSupplies);

            saleCode.IsFailure.Should().BeTrue();
            saleCode.Error.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Not_Create_SaleCode_With_Invalid_LaborRate()
        {
            string name = Utilities.RandomCharacters(SaleCode.MinimumLength);
            string code = Utilities.RandomCharacters(SaleCode.MinimumLength);
            double invalidLaborRate = SaleCode.MinimumValue - .01;
            double desiredMargin = SaleCode.MinimumValue;
            SaleCodeShopSupplies shopSupplies = new();

            var saleCode = SaleCode.Create(name, code, invalidLaborRate, desiredMargin, shopSupplies);

            saleCode.IsFailure.Should().BeTrue();
            saleCode.Error.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Not_Create_SaleCode_With_Invalid_DesiredMargin()
        {
            string name = Utilities.RandomCharacters(SaleCode.MinimumLength);
            string code = Utilities.RandomCharacters(SaleCode.MinimumLength);
            double laborRate = SaleCode.MinimumValue;
            double invalidDesiredMargin = SaleCode.MinimumValue - .01;
            SaleCodeShopSupplies shopSupplies = new();

            var saleCode = SaleCode.Create(name, code, laborRate, invalidDesiredMargin, shopSupplies);

            saleCode.IsFailure.Should().BeTrue();
            saleCode.Error.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void SetName()
        {
            var saleCode = InventoryItemTestHelper.CreateSaleCode();

            var newName = Utilities.RandomCharacters(SaleCode.MinimumLength + 1);
            saleCode.SetName(newName);

            saleCode.Name.Should().Be(newName);
        }


        [Fact]
        public void SetCode()
        {
            var saleCode = InventoryItemTestHelper.CreateSaleCode();

            var newCode = Utilities.RandomCharacters(SaleCode.MinimumLength + 1);
            saleCode.SetCode(newCode);

            saleCode.Code.Should().Be(newCode);
        }

        [Fact]
        public void SetLaborRate()
        {
            var saleCode = InventoryItemTestHelper.CreateSaleCode();

            saleCode.SetLaborRate(SaleCode.MinimumValue + 1);

            saleCode.LaborRate.Should().Be(SaleCode.MinimumValue + 1);
        }

        [Fact]
        public void SetDesiredMargin()
        {
            var saleCode = InventoryItemTestHelper.CreateSaleCode();

            saleCode.SetDesiredMargin(SaleCode.MinimumValue + 1);

            saleCode.DesiredMargin.Should().Be(SaleCode.MinimumValue + 1);
        }

        [Fact]
        public void SetShopSupplies()
        {
            var saleCode = InventoryItemTestHelper.CreateSaleCode();

            SaleCodeShopSupplies originalShopSupplies = saleCode.ShopSupplies;
            SaleCodeShopSupplies newShopSupplies = new();
            saleCode.SetShopSupplies(newShopSupplies);

            saleCode.ShopSupplies.Should().NotBe(originalShopSupplies);
            saleCode.ShopSupplies.Should().Be(newShopSupplies);
        }

        [Fact]
        public void Not_Set_Null_Name()
        {
            var saleCode = InventoryItemTestHelper.CreateSaleCode();

            string nullName = null;
            var resultOrError = saleCode.SetName(nullName);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Name(int length)
        {
            var saleCode = InventoryItemTestHelper.CreateSaleCode();

            var invalidNName = Utilities.RandomCharacters(length);

            var resultOrError = saleCode.SetName(invalidNName);

            resultOrError.IsFailure.Should().BeTrue();
        }


        [Fact]
        public void Not_Set_Null_Code()
        {
            var saleCode = InventoryItemTestHelper.CreateSaleCode();

            var resultOrError = saleCode.SetCode(null);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Code(int length)
        {
            var saleCode = InventoryItemTestHelper.CreateSaleCode();
            var invalidCode = Utilities.RandomCharacters(length);

            var resultOrError = saleCode.SetCode(invalidCode);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_LaborRate()
        {
            var saleCode = InventoryItemTestHelper.CreateSaleCode();
            var invalidLaborRate = SaleCode.MinimumValue - .01;

            var resultOrError = saleCode.SetLaborRate(invalidLaborRate);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_DesiredMargin()
        {
            var saleCode = InventoryItemTestHelper.CreateSaleCode();
            var invalidDesiredMargin = SaleCode.MinimumValue - .01;

            var resultOrError = saleCode.SetDesiredMargin(invalidDesiredMargin);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Null_ShopSupplies()
        {
            var saleCode = InventoryItemTestHelper.CreateSaleCode();

            var resultOrError = saleCode.SetShopSupplies(null);

            resultOrError.IsFailure.Should().BeTrue();
        }


        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { SaleCode.MinimumLength - 1 };
                    yield return new object[] { SaleCode.MaximumLength + 1 };
                }
            }
        }
    }
}