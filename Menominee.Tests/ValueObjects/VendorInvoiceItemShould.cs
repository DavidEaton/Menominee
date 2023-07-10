using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.Payables;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Menominee.Tests.ValueObjects
{
    public class VendorInvoiceItemShould
    {
        [Fact]
        public void Create_VendorInvoiceItem()
        {
            // Arrange
            // Act
            var resultOrError = VendorInvoiceItem.Create("a part", "a description");

            // Assert
            resultOrError.IsFailure.Should().BeFalse();
            resultOrError.Value.Should().BeOfType<VendorInvoiceItem>();
        }

        [Fact]
        public void Create_VendorInvoiceItem_With_Optional_Manufacturer()
        {
            var manufacturer = Manufacturer.Create("Manufacturer One", "Group", "M1").Value;

            var resultOrError = VendorInvoiceItem.Create("a part", "a description", manufacturer: manufacturer);

            resultOrError.IsFailure.Should().BeFalse();
            resultOrError.Value.Should().BeOfType<VendorInvoiceItem>();
        }

        [Fact]
        public void Create_VendorInvoiceItem_With_Optional_SaleCode()
        {
            var supplies = SaleCodeShopSupplies.Create(.25, 10, 5, 99999, true, true).Value;
            var saleCode = SaleCode.Create("SC1", "Sale Code One", .25, 100.00, supplies).Value;

            var resultOrError = VendorInvoiceItem.Create("a part", "a description", saleCode: saleCode);

            resultOrError.IsFailure.Should().BeFalse();
            resultOrError.Value.Should().BeOfType<VendorInvoiceItem>();
        }

        [Fact]
        public void Not_Create_VendorInvoiceItem_With_Null_PartNumber()
        {
            var resultOrError = VendorInvoiceItem.Create(null, "a description");

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_VendorInvoiceItem_With_Invalid_PartNumber_Length(int length)
        {
            var partNumber = Utilities.RandomCharacters(length);

            var resultOrError = VendorInvoiceItem.Create(partNumber, "a description");

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_VendorInvoiceItem_With_Invalid_Description_Length(int length)
        {
            var description = Utilities.RandomCharacters(length);

            var resultOrError = VendorInvoiceItem.Create("a part", description);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_New_On_NewPartNumber()
        {
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            item.PartNumber.Should().Be("a part");
            var part = "different part";

            var result = item.NewPartNumber(part);

            result.IsSuccess.Should().BeTrue();
            result.Value.PartNumber.Should().Be(part);
        }

        [Fact]
        public void Return_New_On_NewDescription()
        {
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            item.Description.Should().Be("a description");
            var description = "different description";

            var result = item.NewDescription(description);

            result.Value.Description.Should().Be(description);
        }

        [Fact]
        public void Return_New_On_NewManufacturer()
        {
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            item.Manufacturer.Should().BeNull();
            var manufacturer = Manufacturer.Create("Manufacturer One", "Group", "M1").Value;
            var result = item.NewManufacturer(manufacturer);

            result.Value.Manufacturer.Should().Be(manufacturer);
        }

        [Fact]
        public void Return_New_On_NewSaleCode()
        {
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            item.SaleCode.Should().BeNull();
            var supplies = SaleCodeShopSupplies.Create(.25, 10, 5, 99999, true, true).Value;
            var saleCode = SaleCode.Create("SC1", "Sale Code One", .25, 100.00, supplies).Value;

            var result = item.NewSaleCode(saleCode);

            result.Value.SaleCode.Should().Be(saleCode);
        }

        [Fact]
        public void ClearManufacturer()
        {
            var manufacturer = Manufacturer.Create("Manufacturer One", "Group", "M1").Value;
            var item = VendorInvoiceItem.Create("a part", "a description", manufacturer).Value;

            item.Manufacturer.Should().Be(manufacturer);
            var result = item.ClearManufacturer();

            result.IsSuccess.Should().BeTrue();
            result.Value.Manufacturer.Should().BeNull();
        }

        [Fact]
        public void ClearSaleCode()
        {
            var manufacturer = Manufacturer.Create("Manufacturer One", "Group", "M1").Value;
            var supplies = SaleCodeShopSupplies.Create(.25, 10, 5, 99999, true, true).Value;
            var saleCode = SaleCode.Create("SC1", "Sale Code One", .25, 100.00, supplies).Value;
            var item = VendorInvoiceItem.Create("a part", "a description", manufacturer, saleCode).Value;

            item.SaleCode.Should().Be(saleCode);
            var result = item.ClearSaleCode();

            result.IsSuccess.Should().BeTrue();
            result.Value.SaleCode.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid__Length_PartNumber(int length)
        {
            var invalidPartNumber = Utilities.RandomCharacters(length);
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            var resultOrError = item.NewPartNumber(invalidPartNumber);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Length_Description(int length)
        {
            var invalidDescription = Utilities.RandomCharacters(length);
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            var resultOrError = item.NewDescription(invalidDescription);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Null_Manufacturer()
        {
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            var resultOrError = item.NewManufacturer(null);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Null_SaleCode()
        {
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            var resultOrError = item.NewSaleCode(null);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Equate_Two_Instances_Having_Same_Values()
        {
            var itemOne = VendorInvoiceItem.Create("a part", "a description").Value;
            var itemTwo = VendorInvoiceItem.Create("a part", "a description").Value;

            itemOne.Should().Be(itemTwo);
        }

        [Fact]
        public void Not_Equate_Two_Instances_Having_Differing_Values()
        {
            var itemOne = VendorInvoiceItem.Create("a part", "a description").Value;
            var itemTwo = VendorInvoiceItem.Create("a part", "a Differing description").Value;

            itemOne.Should().NotBe(itemTwo);
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { VendorInvoiceItem.MaximumLength + 1 };
                    yield return new object[] { VendorInvoiceItem.MinimumLength - 1 };
                }
            }
        }
    }
}
