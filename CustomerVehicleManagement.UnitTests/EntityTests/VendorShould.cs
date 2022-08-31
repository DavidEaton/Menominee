using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
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

    public class VendorShould
    {
        [Fact]
        public void Create_Vendor()
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
        public void Set_Name()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var vendor = Vendor.Create(name, vendorCode).Value;

            var newName = "V2";
            vendor.SetName(newName);

            vendor.Name.Should().Be(newName);
        }

        [Fact]
        public void Not_Set_Name_With_Null_Name()
        {
            var name = Utilities.RandomCharacters(Vendor.MinimumLength);
            var vendorCode = Utilities.RandomCharacters(Vendor.MinimumLength);

            var vendor = Vendor.Create(name, vendorCode).Value;

            string newName = null;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendor.SetName(newName));
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Name_With_Invalid_Name(int length)
        {
            var name = Utilities.RandomCharacters(Vendor.MinimumLength);
            var vendorCode = Utilities.RandomCharacters(Vendor.MinimumLength);

            var vendor = Vendor.Create(name, vendorCode).Value;

            var newName = Utilities.RandomCharacters(length);

            Assert.Throws<ArgumentOutOfRangeException>(() => vendor.SetName(newName));
        }

        [Fact]
        public void Set_Vendor_Code()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var vendor = Vendor.Create(name, vendorCode).Value;

            var newVendorCode = "V2";
            vendor.SetVendorCode(newVendorCode);

            vendor.VendorCode.Should().Be(newVendorCode);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Vendor_Code(int length)
        {
            var name = Utilities.RandomCharacters(Vendor.MinimumLength);
            var vendorCode = Utilities.RandomCharacters(Vendor.MinimumLength);
            var vendor = Vendor.Create(name, vendorCode).Value;

            var newVendorCode = Utilities.RandomCharacters(length);

            Assert.Throws<ArgumentOutOfRangeException>(() => vendor.SetVendorCode(newVendorCode));
        }

        [Fact]
        public void Not_Set_Null_Vendor_Code()
        {
            var name = Utilities.RandomCharacters(Vendor.MinimumLength);
            var vendorCode = Utilities.RandomCharacters(Vendor.MinimumLength);
            var vendor = Vendor.Create(name, vendorCode).Value;

            string newVendorCode = null;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendor.SetVendorCode(newVendorCode));
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

    }
}
