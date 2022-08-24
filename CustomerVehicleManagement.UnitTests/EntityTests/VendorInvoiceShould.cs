using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class VendorInvoiceShould
    {
        [Fact]
        public void CreateVendorInvoice()
        {
            // Arrange
            var vendorOrError = Vendor.Create("Vendor One", "V1");

            // Act
            var vendorInvoice = VendorInvoice.Create(
                vendorOrError.Value,
                VendorInvoiceStatus.Open,
                "001",
                1).Value;

            // Assert
            vendorInvoice.Should().NotBeNull();
        }
    }
}
