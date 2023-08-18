using Menominee.Domain.Entities;

using FluentAssertions;
using System.Collections.Generic;
using TestingHelperLibrary;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class SaleCodeShould
    {
        [Fact]
        public void Create_SaleCode()
        {
            // Arrange
            var name = Utilities.RandomCharacters(SaleCode.MinimumLength);
            var code = Utilities.RandomCharacters(SaleCode.MinimumLength);
            var laborRate = SaleCode.MinimumValue;
            var desiredMargin = SaleCode.MinimumValue;
            var shopSupplies = new SaleCodeShopSuppliesFaker(true).Generate();

            // Act
            var result = SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies);

            // Assert
            result.Value.Should().BeOfType<SaleCode>();
            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be(name);
            result.Value.Code.Should().Be(code);
            result.Value.LaborRate.Should().Be(laborRate);
            result.Value.DesiredMargin.Should().Be(desiredMargin);
            result.Value.ShopSupplies.Should().Be(shopSupplies);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_SaleCode_With_Invalid_Name(int length)
        {
            var invalidName = Utilities.RandomCharacters(length);
            var code = Utilities.RandomCharacters(SaleCode.MinimumLength);
            var laborRate = SaleCode.MinimumValue;
            var desiredMargin = SaleCode.MinimumValue;
            var shopSupplies = new SaleCodeShopSuppliesFaker(true).Generate();

            var saleCode = SaleCode.Create(invalidName, code, laborRate, desiredMargin, shopSupplies);

            saleCode.IsFailure.Should().BeTrue();
            saleCode.Error.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_SaleCode_With_Invalid_Code(int length)
        {
            var name = Utilities.RandomCharacters(SaleCode.MinimumLength);
            var invalidCode = Utilities.RandomCharacters(length);
            var laborRate = SaleCode.MinimumValue;
            var desiredMargin = SaleCode.MinimumValue;
            var shopSupplies = new SaleCodeShopSuppliesFaker(true).Generate();

            var saleCode = SaleCode.Create(name, invalidCode, laborRate, desiredMargin, shopSupplies);

            saleCode.IsFailure.Should().BeTrue();
            saleCode.Error.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Not_Create_SaleCode_With_Invalid_LaborRate()
        {
            var name = Utilities.RandomCharacters(SaleCode.MinimumLength);
            var code = Utilities.RandomCharacters(SaleCode.MinimumLength);
            var invalidLaborRate = SaleCode.MinimumValue - .01;
            var desiredMargin = SaleCode.MinimumValue;
            var shopSupplies = new SaleCodeShopSuppliesFaker(true).Generate();

            var saleCode = SaleCode.Create(name, code, invalidLaborRate, desiredMargin, shopSupplies);

            saleCode.IsFailure.Should().BeTrue();
            saleCode.Error.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Not_Create_SaleCode_With_Invalid_DesiredMargin()
        {
            var name = Utilities.RandomCharacters(SaleCode.MinimumLength);
            var code = Utilities.RandomCharacters(SaleCode.MinimumLength);
            var laborRate = SaleCode.MinimumValue;
            var invalidDesiredMargin = SaleCode.MinimumValue - .01;
            var shopSupplies = new SaleCodeShopSuppliesFaker(true).Generate();

            var saleCode = SaleCode.Create(name, code, laborRate, invalidDesiredMargin, shopSupplies);

            saleCode.IsFailure.Should().BeTrue();
            saleCode.Error.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Not_Create_SaleCode_With_Invalid_ShopSupplies()
        {
            var name = Utilities.RandomCharacters(SaleCode.MinimumLength);
            var code = Utilities.RandomCharacters(SaleCode.MinimumLength);
            var laborRate = SaleCode.MinimumValue;
            var desiredMargin = SaleCode.MinimumValue;
            SaleCodeShopSupplies invalidShopSupplies = null;

            var saleCode = SaleCode.Create(name, code, laborRate, desiredMargin, invalidShopSupplies);

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
            var newShopSupplies = new SaleCodeShopSuppliesFaker(true).Generate();

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
                    yield return new object[] { SaleCode.NameMaximumLength + 1 };
                }
            }
        }
    }
}
