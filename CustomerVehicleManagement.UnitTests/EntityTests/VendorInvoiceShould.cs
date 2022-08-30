using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
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

        [Fact]
        public void CreateVendorInvoiceWithStautsEqualToOpen()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                "001",
                1).Value;

            vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Open);
        }

        [Fact]
        public void SetVendor()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                "001",
                1).Value;

            var vendorTwo = Vendor.Create("Vendor Two", "V@").Value;
            vendorInvoice.SetVendor(vendorTwo);

            vendorInvoice.Vendor.Should().Be(vendorTwo);
        }

        [Fact]
        public void SetVendorInvoiceStatus()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                "001",
                1).Value;

            vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Open);

            vendorInvoice.SetVendorInvoiceStatus(VendorInvoiceStatus.Reconciled);

            vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Reconciled);
        }

        [Fact]
        public void SetInvoiceNumber()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                "001",
                1).Value;

            vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Open);
            var newInvoiceNumber = "002";
            vendorInvoice.SetInvoiceNumber(newInvoiceNumber);

            vendorInvoice.InvoiceNumber.Should().Be(newInvoiceNumber);
        }

        [Fact]
        public void SetTotal()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                "001",
                1).Value;

            vendorInvoice.Total.Should().Be(1);
            var newTotal = 2;
            vendorInvoice.SetTotal(newTotal);

            vendorInvoice.Total.Should().Be(newTotal);
        }

        [Fact]
        public void SetDate()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                "001",
                1).Value;

            vendorInvoice.Date.Should().BeNull();
            DateTime? date = new(2000, 1, 1);
            vendorInvoice.SetDate(date);

            vendorInvoice.Date.Should().Be(date);
        }

        [Fact]
        public void SetDatePosted()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                "001",
                1).Value;

            vendorInvoice.DatePosted.Should().BeNull();
            DateTime? date = new(2000, 1, 1);
            vendorInvoice.SetDatePosted(date);

            vendorInvoice.DatePosted.Should().Be(date);
        }

    }
}
