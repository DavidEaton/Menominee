using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using static CustomerVehicleManagement.UnitTests.EntityTests.VendorInvoiceTestHelper;
namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class VendorShould
    {
        [Fact]
        public void Create_Vendor()
        {
            // Arrange
            var name = "Vendor One";
            var vendorCode = "V1";

            // Act
            var vendorOrError = Vendor.Create(name, vendorCode);

            // Assert
            vendorOrError.IsFailure.Should().BeFalse();
            vendorOrError.Value.Should().BeOfType<Vendor>();
            vendorOrError.Value.Name.Should().Be(name);
            vendorOrError.Value.VendorCode.Should().Be(vendorCode);
        }

        [Fact]
        public void Not_Create_Vendor_With_Null_Name()
        {
            var vendorCode = Utilities.RandomCharacters(Vendor.MinimumLength);
            string name = null;

            var vendorOrError = Vendor.Create(name, vendorCode);

            vendorOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_Vendor_With_Invalid_Name(int length)
        {
            var name = Utilities.RandomCharacters(length);
            var vendorCode = Utilities.RandomCharacters(Vendor.MinimumLength);

            var vendorOrError = Vendor.Create(name, vendorCode);

            vendorOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_Vendor_With_Null_Code()
        {
            var name = Utilities.RandomCharacters(Vendor.MinimumLength);
            string code = null;

            var vendorOrError = Vendor.Create(name, code);

            vendorOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_Vendor_With_Invalid_Code(int length)
        {
            var name = Utilities.RandomCharacters(Vendor.MinimumLength);
            var vendorCode = Utilities.RandomCharacters(length);

            var vendorOrError = Vendor.Create(name, vendorCode);

            vendorOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetName()
        {
            var vendor = CreateVendor();

            var name = Utilities.RandomCharacters(Vendor.MinimumLength + 11);
            vendor.SetName(name);

            vendor.Name.Should().Be(name);
        }

        [Fact]
        public void Not_Set_Name_With_Null_Name()
        {
            var vendorOrError = CreateVendor();

            //vendorOrError.Name.Should().BeTrue();

            //vendorOrError.Value.SetName(null);

            //vendorOrError.IsSuccess.Should().BeTrue();

            true.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Name_With_Invalid_Name(int length)
        {
            var name = Utilities.RandomCharacters(Vendor.MinimumLength);
            var vendorCode = Utilities.RandomCharacters(Vendor.MinimumLength);
            var vendorOrError = Vendor.Create(name, vendorCode);

            var invalidName = Utilities.RandomCharacters(length);
            vendorOrError.Value.SetName(invalidName);

            vendorOrError.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void SetVendorCode()
        {
            var vendor = CreateVendor();
            vendor.VendorCode.Length.Should().Be(Vendor.MinimumLength);
            var vendorCode = Utilities.RandomCharacters(Vendor.MinimumLength + 1);

            vendor.SetVendorCode(vendorCode);

            vendor.VendorCode.Should().Be(vendorCode);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Vendor_Code(int length)
        {
            var vendor = CreateVendor();

            var invalidVendorCode = Utilities.RandomCharacters(length);

            Assert.Throws<ArgumentOutOfRangeException>(() => vendor.SetVendorCode(invalidVendorCode));
        }

        [Fact]
        public void Not_Set_Null_Vendor_Code()
        {
            var vendor = CreateVendor();

            string nullVendorCode = null;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendor.SetVendorCode(nullVendorCode));
        }

        [Fact]
        public void Disable()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var vendor = Vendor.Create(name, vendorCode).Value;

            vendor.Disable();

            vendor.IsActive.Should().BeFalse();
        }

        [Fact]
        public void Enable()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var vendor = Vendor.Create(name, vendorCode).Value;

            vendor.Disable();
            vendor.IsActive.Should().BeFalse();

            vendor.Enable();
            vendor.IsActive.Should().BeTrue();
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { Vendor.MinimumLength - 1 };
                    yield return new object[] { Vendor.MaximumLength + 1 };
                }
            }
        }
    }
}
