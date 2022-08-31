using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class VendorInvoiceShouldTestData
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[] { null };
                yield return new object[] { DateTime.Today.AddDays(1) };
            }
        }
    }

    public class VendorInvoiceShould
    {
        [Fact]
        public void Create_Vendor_Invoice()
        {
            // Arrange
            var vendorOrError = Vendor.Create("Vendor One", "V1");

            // Act
            var vendorInvoice = VendorInvoice.Create(
                vendorOrError.Value,
                VendorInvoiceStatus.Open,
                null,
                1);

            // Assert
            vendorInvoice.IsFailure.Should().Be(false);
            vendorInvoice.Should().NotBeNull();
            vendorInvoice.Value.Should().BeOfType<VendorInvoice>();
        }

        [Fact]
        public void Create_Vendor_Invoice_With_Optional_InvoiceNumber()
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
        public void Create_Vendor_Invoice_With_Optional_DatePosted()
        {
            var datePosted = DateTime.Today;
            var vendor = Vendor.Create("Vendor One", "V1").Value;

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                "001",
                1,
                null,
                datePosted);

            vendorInvoice.IsFailure.Should().Be(false);
            vendorInvoice.Should().NotBeNull();
            vendorInvoice.Value.Should().BeOfType<VendorInvoice>();
        }

        [Fact]
        public void Create_Vendor_Invoice_With_Optional_Date()
        {
            var date = DateTime.Today;
            var vendor = Vendor.Create("Vendor One", "V1").Value;

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                "001",
                1,
                date,
                null);

            vendorInvoice.IsFailure.Should().Be(false);
            vendorInvoice.Should().NotBeNull();
            vendorInvoice.Value.Should().BeOfType<VendorInvoice>();
        }

        [Fact]
        public void Create_Vendor_Invoice_With_Open_Status()
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
        public void Not_Create_Vendor_Invoice_With_Null_Vendor()
        {
            var vendorInvoice = VendorInvoice.Create(
                null,
                VendorInvoiceStatus.Open,
                "001",
                1);

            vendorInvoice.IsFailure.Should().Be(true);
            vendorInvoice.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_Vendor_Invoice_With_Invalid_Status()
        {
            var invalidStatus = (VendorInvoiceStatus)(-1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                invalidStatus,
                "001",
                1);

            vendorInvoice.IsFailure.Should().Be(true);
            vendorInvoice.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_Vendor_Invoice_With_Invalid_Total()
        {
            var invalidTotal = -1;
            var vendor = Vendor.Create("Vendor One", "V1").Value;

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                "001",
                invalidTotal);

            vendorInvoice.IsFailure.Should().Be(true);
            vendorInvoice.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_Vendor_Invoice_With_Invalid_Date()
        {
            var invalidDate = DateTime.Today.AddDays(1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                "001",
                1,
                invalidDate);

            vendorInvoice.IsFailure.Should().Be(true);
            vendorInvoice.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_Vendor_Invoice_With_Invalid_DatePosted()
        {
            var date = DateTime.Today;
            var invalidDatePosted = DateTime.Today.AddDays(1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                "001",
                1,
                date,
                invalidDatePosted);

            vendorInvoice.IsFailure.Should().Be(true);
            vendorInvoice.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_Vendor_Invoice_With_Invalid_InvoiceNumber()
        {
            var invalidInvoiceNumber = Utilities.RandomCharacters(VendorInvoice.InvoiceNumberMaximumLength + 1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;

            var vendorInvoice = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                invalidInvoiceNumber,
                1);

            vendorInvoice.IsFailure.Should().Be(true);
            vendorInvoice.Error.Should().NotBeNull();
        }

        [Fact]
        public void Set_Vendor()
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
        public void Set_Vendor_Invoice_Status()
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
        public void Set_Invoice_Number()
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
        public void Set_Total()
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
        public void Set_Date()
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
        public void Clear_Date()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");
            DateTime? date = new(2000, 1, 1);

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                "001",
                1,
                date).Value;

            vendorInvoice.Date.Should().NotBeNull();
            vendorInvoice.ClearDate();

            vendorInvoice.Date.Should().BeNull();
        }

        [Fact]
        public void Set_Date_Posted()
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

        [Fact]
        public void Clear_DatePosted()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");
            DateTime? datePosted = new(2000, 1, 1);

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                "001",
                1,
                null,
                datePosted).Value;

            vendorInvoice.DatePosted.Should().NotBeNull();
            vendorInvoice.ClearDatePosted();

            vendorInvoice.Date.Should().BeNull();
        }

        [Fact]
        public void Not_Set_Invalid_Vendor()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                "001",
                1).Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoice.SetVendor(null));
        }

        [Fact]
        public void Not_Set_Invalid_Vendor_Invoice_Status()
        {
            var invalidStatus = (VendorInvoiceStatus)(-1);
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                "001",
                1).Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoice.SetVendorInvoiceStatus(invalidStatus));
        }

        [Fact]
        public void Not_Set_Invalid_Invoice_Number()
        {
            var invalidInvoiceNumber = Utilities.RandomCharacters(VendorInvoice.InvoiceNumberMaximumLength + 1);
            var vendor = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendor.Value,
                VendorInvoiceStatus.Open,
                "001",
                1).Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoice.SetInvoiceNumber(invalidInvoiceNumber));
        }

        [Fact]
        public void Not_Set_Invalid_Total()
        {
            var invalidTotal = VendorInvoice.MinimumValue - 1;
            var vendor = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendor.Value,
                VendorInvoiceStatus.Open,
                "001",
                1).Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoice.SetTotal(invalidTotal));
        }

        [Theory]
        [MemberData(nameof(VendorInvoiceShouldTestData.Data), MemberType = typeof(VendorInvoiceShouldTestData))]
        public void Not_Set_Invalid_Date(DateTime? date)
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                "001",
                1).Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoice.SetDate(date));
        }

        [Theory]
        [MemberData(nameof(VendorInvoiceShouldTestData.Data), MemberType = typeof(VendorInvoiceShouldTestData))]
        public void Not_Set_Invalid_DatePosted(DateTime? date)
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                "001",
                1).Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoice.SetDatePosted(date));
        }


    }
}
