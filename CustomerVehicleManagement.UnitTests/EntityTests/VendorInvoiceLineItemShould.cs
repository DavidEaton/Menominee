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
            Manufacturer manufacturer = Manufacturer.Create("Manufacturer One", "Group", "M1").Value;
            SaleCode saleCode = new() { Code = "SC1", Name = "Sale Code One", DesiredMargin = .25, LaborRate = 100.00 };
            var item = VendorInvoiceItem.Create("BR549", manufacturer, "a description", saleCode).Value;
            var vendorInvoiceLineItem = VendorInvoiceLineItem.Create(VendorInvoiceItemType.Purchase, item, 1, 1, 1, "001", null).Value;


            // Act

            // Assert
            vendorInvoiceLineItem.Should().NotBeNull();
        }
    }
}
