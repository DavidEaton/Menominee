using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class VendorInvoiceShould
    {
        [Fact]
        public void Create_VendorInvoice()
        {
            // Arrange
            var vendorOrError = Vendor.Create("Vendor One", "V1");

            // Act
            var vendorInvoiceOrError = VendorInvoice.Create(
                vendorOrError.Value,
                VendorInvoiceStatus.Open,
                1);

            // Assert
            vendorInvoiceOrError.IsFailure.Should().BeFalse();
            vendorInvoiceOrError.Should().NotBeNull();
            vendorInvoiceOrError.Value.Should().BeOfType<VendorInvoice>();
        }

        [Fact]
        public void Create_VendorInvoice_With_Optional_InvoiceNumber()
        {
            var vendorOneOrError = Vendor.Create("Vendor One", "V1");
            var invoiceNumber = "001";
            var vendorInvoice = VendorInvoice.Create(
                vendorOneOrError.Value,
                VendorInvoiceStatus.Open,
                1,
                invoiceNumber)
                .Value;

            vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Open);
        }

        [Fact]
        public void Create_VendorInvoice_With_Optional_DatePosted()
        {
            var datePosted = DateTime.Today;
            var vendor = Vendor.Create("Vendor One", "V1").Value;

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                1,
                datePosted: datePosted);

            vendorInvoiceOrError.IsFailure.Should().BeFalse();
            vendorInvoiceOrError.Should().NotBeNull();
            vendorInvoiceOrError.Value.Should().BeOfType<VendorInvoice>();
        }

        [Fact]
        public void Create_VendorInvoice_With_Optional_Date()
        {
            var date = DateTime.Today;
            var vendor = Vendor.Create("Vendor One", "V1").Value;

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                1,
                date: date);

            vendorInvoiceOrError.IsFailure.Should().BeFalse();
            vendorInvoiceOrError.Should().NotBeNull();
            vendorInvoiceOrError.Value.Should().BeOfType<VendorInvoice>();
        }

        [Fact]
        public void Create_VendorInvoice_With_Open_Status()
        {
            var vendorOneOrError = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOneOrError.Value,
                VendorInvoiceStatus.Open,
                1)
                .Value;

            vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Open);
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Null_Vendor()
        {
            var vendorInvoiceOrError = VendorInvoice.Create(
                null,
                VendorInvoiceStatus.Open,
                1);

            vendorInvoiceOrError.IsFailure.Should().BeTrue();
            vendorInvoiceOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Invalid_Status()
        {
            var invalidStatus = (VendorInvoiceStatus)(-1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                invalidStatus,
                1);

            vendorInvoiceOrError.IsFailure.Should().BeTrue();
            vendorInvoiceOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Invalid_Total()
        {
            var invalidTotal = -1;
            var vendor = Vendor.Create("Vendor One", "V1").Value;

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                invalidTotal);

            vendorInvoiceOrError.IsFailure.Should().BeTrue();
            vendorInvoiceOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Invalid_Date()
        {
            var invalidDate = DateTime.Today.AddDays(1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                1,
                date: invalidDate);

            vendorInvoiceOrError.IsFailure.Should().BeTrue();
            vendorInvoiceOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Invalid_DatePosted()
        {
            var date = DateTime.Today;
            var invalidDatePosted = DateTime.Today.AddDays(1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                1,
                date: date,
                datePosted: invalidDatePosted);

            vendorInvoiceOrError.IsFailure.Should().BeTrue();
            vendorInvoiceOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_VendorInvoice_With_Invalid_InvoiceNumber()
        {
            var invalidInvoiceNumber = Utilities.RandomCharacters(VendorInvoice.InvoiceNumberMaximumLength + 1);
            var vendor = Vendor.Create("Vendor One", "V1").Value;

            var vendorInvoiceOrError = VendorInvoice.Create(
                vendor,
                VendorInvoiceStatus.Open,
                1,
                invoiceNumber: invalidInvoiceNumber);

            vendorInvoiceOrError.IsFailure.Should().BeTrue();
            vendorInvoiceOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void SetVendor()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                1)
                .Value;

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
                1)
                .Value;

            vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Open);

            vendorInvoice.SetVendorInvoiceStatus(VendorInvoiceStatus.Reconciled);

            vendorInvoice.Status.Should().Be(VendorInvoiceStatus.Reconciled);
        }

        [Fact]
        public void SetInvoiceNumber()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");
            var invoiceNumber = "001";

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                1,
                invoiceNumber).Value;

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
                1)
                .Value;

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
                1)
                .Value;

            vendorInvoice.Date.Should().BeNull();
            DateTime? date = new(2000, 1, 1);
            vendorInvoice.SetDate(date);

            vendorInvoice.Date.Should().Be(date);
        }

        [Fact]
        public void ClearDate()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");
            DateTime? date = new(2000, 1, 1);

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                1,
                date: date)
                .Value;

            vendorInvoice.Date.Should().NotBeNull();
            vendorInvoice.ClearDate();

            vendorInvoice.Date.Should().BeNull();
        }

        [Fact]
        public void SetDatePosted()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                1)
                .Value;

            vendorInvoice.DatePosted.Should().BeNull();
            DateTime? date = new(2000, 1, 1);
            vendorInvoice.SetDatePosted(date);

            vendorInvoice.DatePosted.Should().Be(date);
        }

        [Fact]
        public void ClearDatePosted()
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");
            DateTime? datePosted = new(2000, 1, 1);

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                1,
                datePosted: datePosted).Value;

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
                1)
                .Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoice.SetVendor(null));
        }

        [Fact]
        public void Not_Set_Invalid_VendorInvoice_Status()
        {
            var invalidStatus = (VendorInvoiceStatus)(-1);
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
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
                1).Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoice.SetTotal(invalidTotal));
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Date(DateTime? date)
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                1).Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoice.SetDate(date));
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_DatePosted(DateTime? datePosted)
        {
            var vendorOne = Vendor.Create("Vendor One", "V1");

            var vendorInvoice = VendorInvoice.Create(
                vendorOne.Value,
                VendorInvoiceStatus.Open,
                1).Value;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoice.SetDatePosted(datePosted));
        }

        public class TestData
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
    }
}
