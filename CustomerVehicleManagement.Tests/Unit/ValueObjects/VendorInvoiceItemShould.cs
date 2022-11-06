using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.ValueObjects
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
        public void SetPartNumber()
        {
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            item.PartNumber.Should().Be("a part");
            var part = "different part";

            item.SetPartNumber(part);

            item.PartNumber.Should().Be(part);
        }

        [Fact]
        public void SetDescription()
        {
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            item.Description.Should().Be("a description");
            var description = "different description";

            item.SetDescription(description);

            item.Description.Should().Be(description);
        }

        [Fact]
        public void SetManufacturer()
        {
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            item.Manufacturer.Should().BeNull();
            var manufacturer = Manufacturer.Create("Manufacturer One", "Group", "M1").Value;
            item.SetManufacturer(manufacturer);

            item.Manufacturer.Should().Be(manufacturer);
        }

        [Fact]
        public void SetSaleCode()
        {
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            item.SaleCode.Should().BeNull();
            var supplies = SaleCodeShopSupplies.Create(.25, 10, 5, 99999, true, true).Value;
            var saleCode = SaleCode.Create("SC1", "Sale Code One", .25, 100.00, supplies).Value;

            item.SetSaleCode(saleCode);

            item.SaleCode.Should().Be(saleCode);
        }

        [Fact]
        public void ClearManufacturer()
        {
            var manufacturer = Manufacturer.Create("Manufacturer One", "Group", "M1").Value;
            var item = VendorInvoiceItem.Create("a part", "a description", manufacturer).Value;

            item.Manufacturer.Should().Be(manufacturer);
            item.ClearManufacturer();

            item.Manufacturer.Should().BeNull();
        }

        [Fact]
        public void ClearSaleCode()
        {
            var manufacturer = Manufacturer.Create("Manufacturer One", "Group", "M1").Value;
            var supplies = SaleCodeShopSupplies.Create(.25, 10, 5, 99999, true, true).Value;
            var saleCode = SaleCode.Create("SC1", "Sale Code One", .25, 100.00, supplies).Value;
            var item = VendorInvoiceItem.Create("a part", "a description", manufacturer, saleCode).Value;

            item.SaleCode.Should().Be(saleCode);
            item.ClearSaleCode();

            item.SaleCode.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid__Length_PartNumber(int length)
        {
            var invalidPartNumber = Utilities.RandomCharacters(length);
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            var resultOrError = item.SetPartNumber(invalidPartNumber);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Length_Description(int length)
        {
            var invalidDescription = Utilities.RandomCharacters(length);
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            var resultOrError = item.SetDescription(invalidDescription);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Null_Manufacturer()
        {
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            var resultOrError = item.SetManufacturer(null);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Null_SaleCode()
        {
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            var resultOrError = item.SetSaleCode(null);

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
