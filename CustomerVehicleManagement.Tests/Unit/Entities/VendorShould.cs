using CustomerVehicleManagement.Api.Payables.Vendors;
using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using static CustomerVehicleManagement.Tests.Unit.Entities.VendorInvoiceTestHelper;
namespace CustomerVehicleManagement.Tests.Unit.Entities
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
        public void Create_Vendor_With_Optional_DefaultPaymentMethod()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            var paymentMethod = CreateVendorInvoicePaymentMethod();
            var defaultPaymentMethod = DefaultPaymentMethod.Create(paymentMethod, true).Value;

            var vendorOrError = Vendor.Create(name, vendorCode, defaultPaymentMethod);

            vendorOrError.IsFailure.Should().BeFalse();
            vendorOrError.Value.Should().BeOfType<Vendor>();
            vendorOrError.Value.DefaultPaymentMethod.Should().BeOfType<DefaultPaymentMethod>();
            vendorOrError.Value.DefaultPaymentMethod.Should().Be(defaultPaymentMethod);
            vendorOrError.Value.VendorCode.Should().Be(vendorCode);
        }

        [Fact]
        public void Create_Vendor_With_Null_DefaultPaymentMethod()
        {
            var name = "Vendor One";
            var vendorCode = "V1";
            DefaultPaymentMethod defaultPaymentMethod = null;

            var vendorOrError = Vendor.Create(name, vendorCode, defaultPaymentMethod);

            vendorOrError.IsFailure.Should().BeFalse();
            vendorOrError.Value.Should().BeOfType<Vendor>();
            vendorOrError.Value.DefaultPaymentMethod.Should().Be(null);
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

            var resultOrError = vendorOrError.SetName(null);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Name_With_Invalid_Name(int length)
        {
            var name = Utilities.RandomCharacters(Vendor.MinimumLength);
            var vendorCode = Utilities.RandomCharacters(Vendor.MinimumLength);
            var vendorOrError = Vendor.Create(name, vendorCode);

            var invalidName = Utilities.RandomCharacters(length);
            var resultOrError = vendorOrError.Value.SetName(invalidName);

            resultOrError.IsFailure.Should().BeTrue();
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

        [Fact]
        public void SetDefaultPaymentMethod()
        {
            var vendor = CreateVendor();
            var paymentMethod = CreateVendorInvoicePaymentMethod();
            var defaultPaymentMethod = DefaultPaymentMethod.Create(paymentMethod, true).Value;

            vendor.SetDefaultPaymentMethod(defaultPaymentMethod);

            vendor.DefaultPaymentMethod.Should().Be(defaultPaymentMethod);
        }

        [Fact]
        public void Not_SetDefaultPaymentMethod_With_Null_DefaultPaymentMethod()
        {
            var vendor = CreateVendor();
            var paymentMethod = CreateVendorInvoicePaymentMethod();
            DefaultPaymentMethod defaultPaymentMethod = null;

            var resultOrError = vendor.SetDefaultPaymentMethod(defaultPaymentMethod);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void ClearDefaultPaymentMethod()
        {
            var vendor = CreateVendor();
            var paymentMethod = CreateVendorInvoicePaymentMethod();
            var defaultPaymentMethod = DefaultPaymentMethod.Create(paymentMethod, true).Value;
            vendor.SetDefaultPaymentMethod(defaultPaymentMethod);
            vendor.DefaultPaymentMethod.Should().Be(defaultPaymentMethod);

            var resultOrError = vendor.ClearDefaultPaymentMethod();

            resultOrError.IsFailure.Should().BeFalse();
            vendor.DefaultPaymentMethod.Should().Be(null);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Vendor_Code(int length)
        {
            var vendor = CreateVendor();

            var invalidVendorCode = Utilities.RandomCharacters(length);
            var resultOrError = vendor.SetVendorCode(invalidVendorCode);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Null_Vendor_Code()
        {
            var vendor = CreateVendor();

            var resultOrError = vendor.SetVendorCode(null);

            resultOrError.IsFailure.Should().BeTrue();
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
