using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class VendorInvoiceLineItemShould
    {
        [Fact]
        public void CreateVendorInvoiceLineItem()
        {
            // Arrange
            var manufacturer = Manufacturer.Create("Manufacturer One", "Group", "M1").Value;
            var supplies = SaleCodeShopSupplies.Create(.25, 10, 5, 99999, true, true).Value;
            var saleCode = SaleCode.Create("SC1", "Sale Code One", .25, 100.00, supplies).Value;
            var item = VendorInvoiceItem.Create("BR549", manufacturer, "a description", saleCode).Value;

            // Act
            var vendorInvoiceLineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1, "001", null).Value;

            // Assert
            vendorInvoiceLineItem.Should().NotBeNull();
            vendorInvoiceLineItem.Item.Manufacturer.Should().Be(manufacturer);
            vendorInvoiceLineItem.Item.SaleCode.Should().Be(saleCode);
            vendorInvoiceLineItem.Item.SaleCode.ShopSupplies.Should().Be(saleCode.ShopSupplies);
        }
    }
}
