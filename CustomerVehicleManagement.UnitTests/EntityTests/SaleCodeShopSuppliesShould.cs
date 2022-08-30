using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentAssertions;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class SaleCodeShopSuppliesShould
    {
        [Fact]
        public void CreateSaleCodeShopSupplies()
        {
            // Arrange
            var name = "Vendor One";
            var vendorCode = "V1";

            // Act
            var vendor = Vendor.Create(name, vendorCode).Value;

            // Assert
            vendor.Name.Should().Be(name);
            vendor.VendorCode.Should().Be(vendorCode);
        }
    }
}
