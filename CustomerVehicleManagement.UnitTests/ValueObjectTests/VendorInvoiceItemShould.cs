using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.ValueObjectTests
{
    public class VendorInvoiceItemShould
    {
        [Fact]
        public void Create_VendorInvoiceItem()
        {
            // Arrange
            // Act
            var itemOrError = VendorInvoiceItem.Create("a part", "a description");

            // Assert
            itemOrError.IsFailure.Should().BeFalse();
            itemOrError.Should().NotBeNull();
            itemOrError.Value.Should().BeOfType<VendorInvoiceItem>();
        }

        [Fact]
        public void Create_VendorInvoiceItem_With_Optional_Manufacturer()
        {
            var manufacturer = Manufacturer.Create("Manufacturer One", "Group", "M1").Value;

            var itemOrError = VendorInvoiceItem.Create("a part", "a description", manufacturer: manufacturer);

            itemOrError.IsFailure.Should().BeFalse();
            itemOrError.Should().NotBeNull();
            itemOrError.Value.Should().BeOfType<VendorInvoiceItem>();
        }

        [Fact]
        public void Create_VendorInvoiceItem_With_Optional_SaleCode()
        {
            var supplies = SaleCodeShopSupplies.Create(.25, 10, 5, 99999, true, true).Value;
            var saleCode = SaleCode.Create("SC1", "Sale Code One", .25, 100.00, supplies).Value;

            var itemOrError = VendorInvoiceItem.Create("a part", "a description", saleCode: saleCode);

            itemOrError.IsFailure.Should().BeFalse();
            itemOrError.Should().NotBeNull();
            itemOrError.Value.Should().BeOfType<VendorInvoiceItem>();
        }

        [Fact]
        public void Not_Create_VendorInvoiceItem_With_Null_PartNumber()
        {
            var itemOrError = VendorInvoiceItem.Create(null, "a description");

            itemOrError.IsFailure.Should().BeTrue();
            itemOrError.Error.Should().NotBeNull();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_VendorInvoiceItem_With_Invalid_PartNumber(int length)
        {
            var partNumber = Utilities.RandomCharacters(length);

            var itemOrError = VendorInvoiceItem.Create(partNumber, "a description");

            itemOrError.IsFailure.Should().BeTrue();
            itemOrError.Error.Should().NotBeNull();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_VendorInvoiceItem_With_Invalid_Description(int length)
        {
            var description = Utilities.RandomCharacters(length);

            var itemOrError = VendorInvoiceItem.Create("a part", description);

            itemOrError.IsFailure.Should().BeTrue();
            itemOrError.Error.Should().NotBeNull();
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
        public void Not_Set_Invalid_PartNumber(int length)
        {
            var invalidPartNumber = Utilities.RandomCharacters(length);
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => item.SetPartNumber(invalidPartNumber));
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Description(int length)
        {
            var invalidDescription = Utilities.RandomCharacters(length);
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => item.SetDescription(invalidDescription));
        }

        [Fact]
        public void Not_Set_Invalid_Manufacturer()
        {
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => item.SetManufacturer(null));
        }

        [Fact]
        public void Not_Set_Invalid_SaleCode()
        {
            var item = VendorInvoiceItem.Create("a part", "a description").Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => item.SetSaleCode(null));
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
