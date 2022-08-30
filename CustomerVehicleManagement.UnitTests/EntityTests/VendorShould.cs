using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentAssertions;
using System;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class VendorShould
    {
        [Fact]
        public void CreateVendor()
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

        [Fact]
        public void NotCreateInvalidVendor()
        {
            var name = "V";
            var vendorCode = "V1";

            var vendorOrError = Vendor.Create(name, vendorCode);

            vendorOrError.IsFailure.Should().Be(true);
        }

        [Fact]
        public void SetName()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var vendor = Vendor.Create(name, vendorCode).Value;

            var newName = "V2";
            vendor.SetName(newName);

            vendor.Name.Should().Be(newName);
        }

        [Fact]
        public void NotSetInvalidName()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var vendor = Vendor.Create(name, vendorCode).Value;

            var newName = "V";

            Assert.Throws<ArgumentOutOfRangeException>(() => vendor.SetName(newName));
        }

        [Fact]
        public void SetVendorCode()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var vendor = Vendor.Create(name, vendorCode).Value;

            var newVendorCode = "V2";
            vendor.SetVendorCode(newVendorCode);

            vendor.VendorCode.Should().Be(newVendorCode);
        }

        [Fact]
        public void NotSetInvalidVendorCode()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var vendor = Vendor.Create(name, vendorCode).Value;

            var newVendorCode = "V";

            Assert.Throws<ArgumentOutOfRangeException>(() => vendor.SetVendorCode(newVendorCode));
        }

        [Fact]
        public void Disable()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var vendor = Vendor.Create(name, vendorCode).Value;

            vendor.Disable();

            vendor.IsActive.Should().Be(false);
        }

        [Fact]
        public void Enable()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var vendor = Vendor.Create(name, vendorCode).Value;

            vendor.Disable();
            vendor.IsActive.Should().Be(false);

            vendor.Enable();
            vendor.IsActive.Should().Be(true);
        }

    }
}
