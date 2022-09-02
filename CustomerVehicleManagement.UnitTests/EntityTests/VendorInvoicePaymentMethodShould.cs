using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class VendorInvoicePaymentMethodShould
    {
        [Fact]
        public void Create_VendorInvoicePaymentMethod()
        {
            // Arrange
            string name = Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength);
            bool isActive = true;
            bool isOnAccountPaymentType = true;
            var reconcilingVendor = CreateVendor();
            IList<string> paymentMethodNames = CreatePaymentMethodNames();

            // Act
            var vendorInvoicePaymentMethod = VendorInvoicePaymentMethod.Create(
                paymentMethodNames, name, isActive, isOnAccountPaymentType, reconcilingVendor).Value;

            // Assert
            vendorInvoicePaymentMethod.Name.Should().Be(name);
            vendorInvoicePaymentMethod.IsActive.Should().Be(isActive);
            vendorInvoicePaymentMethod.IsOnAccountPaymentType.Should().Be(isOnAccountPaymentType);
            vendorInvoicePaymentMethod.ReconcilingVendor.Should().Be(reconcilingVendor);
        }

        [Fact]
        public void Not_Create_VendorInvoicePaymentMethod_With_Duplicate_Name()
        {
            string name = "Duplicate_Name";
            bool isActive = true;
            bool isOnAccountPaymentType = true;
            var reconcilingVendor = CreateVendor();
            IList<string> paymentMethodNames = CreatePaymentMethodNames();
            paymentMethodNames.Add(name);

            var vendorInvoicePaymentMethodOrError = VendorInvoicePaymentMethod.Create(
                paymentMethodNames, name, isActive, isOnAccountPaymentType, reconcilingVendor);

            vendorInvoicePaymentMethodOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_VendorInvoicePaymentMethod_With_Null_Name()
        {
            string nullName = null;
            var reconcilingVendor = CreateVendor();
            IList<string> paymentMethodNames = CreatePaymentMethodNames();

            var vendorInvoicePaymentMethodOrError = VendorInvoicePaymentMethod.Create(
                paymentMethodNames, nullName, true, true, reconcilingVendor);

            vendorInvoicePaymentMethodOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_VendorInvoicePaymentMethod_With_Invalid_Name(int length)
        {
            string invalidName = Utilities.RandomCharacters(length);
            var reconcilingVendor = CreateVendor();
            IList<string> paymentMethodNames = CreatePaymentMethodNames();

            var vendorInvoicePaymentMethodOrError = VendorInvoicePaymentMethod.Create(
                paymentMethodNames, invalidName, true, true, reconcilingVendor);

            vendorInvoicePaymentMethodOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetName()
        {
            var vendorInvoicePaymentMethod = CreateVendorInvoicePaymentMethod();

            var newName = Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength);
            vendorInvoicePaymentMethod.SetName(newName);

            vendorInvoicePaymentMethod.Name.Should().Be(newName);
        }

        [Fact]
        public void Enable()
        {
            var vendorInvoicePaymentMethod = CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.IsActive.Should().BeTrue();

            vendorInvoicePaymentMethod.Disable();
            vendorInvoicePaymentMethod.IsActive.Should().BeFalse();
            vendorInvoicePaymentMethod.Enable();

            vendorInvoicePaymentMethod.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Disable()
        {
            var vendorInvoicePaymentMethod = CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.IsActive.Should().BeTrue();

            vendorInvoicePaymentMethod.Disable();

            vendorInvoicePaymentMethod.IsActive.Should().BeFalse();
        }

        [Fact]
        public void SetIsOnAccountPaymentType()
        {
            var vendorInvoicePaymentMethod = CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.IsOnAccountPaymentType.Should().BeTrue();

            vendorInvoicePaymentMethod.SetIsOnAccountPaymentType(false);

            vendorInvoicePaymentMethod.IsOnAccountPaymentType.Should().BeFalse();
        }

        [Fact]
        public void SetReconcilingVendor()
        {
            var vendorInvoicePaymentMethod = CreateVendorInvoicePaymentMethod();

            var reconcilingVendor = CreateVendor();
            vendorInvoicePaymentMethod.ReconcilingVendor.Should().NotBe(reconcilingVendor);
            vendorInvoicePaymentMethod.SetReconcilingVendor(reconcilingVendor);

            vendorInvoicePaymentMethod.ReconcilingVendor.Should().Be(reconcilingVendor);
        }

        [Fact]
        public void Not_Set_Name_With_Null_Name()
        {
            var vendorInvoicePaymentMethod = CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.Name.Should().NotBeNull();

            string newName = null;

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoicePaymentMethod.SetName(newName));
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Name_With_Invalid_Name(int length)
        {
            var vendorInvoicePaymentMethod = CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.Name.Should().NotBeNull();

            string invalidName = Utilities.RandomCharacters(length);

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoicePaymentMethod.SetName(invalidName));
        }

        [Fact]
        public void Not_Set_Null_ReconcilingVendor()
        {
            var vendorInvoicePaymentMethod = CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.ReconcilingVendor.Should().NotBeNull();

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoicePaymentMethod.SetReconcilingVendor(null));
        }

        private Vendor CreateVendor()
        {
            return Vendor.Create(
                Utilities.RandomCharacters(Vendor.MinimumLength),
                Utilities.RandomCharacters(Vendor.MinimumLength)).Value;
        }

        private static IList<string> CreatePaymentMethodNames()
        {
            IList<string> paymentMethodNames = new List<string>();
            paymentMethodNames.Add(Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength));
            paymentMethodNames.Add(Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 10));
            paymentMethodNames.Add(Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 20));
            return paymentMethodNames;
        }

        private VendorInvoicePaymentMethod CreateVendorInvoicePaymentMethod()
        {
            string name = Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 30);
            bool isActive = true;
            bool isOnAccountPaymentType = true;
            var reconcilingVendor = CreateVendor();
            IList<string> paymentMethodNames = CreatePaymentMethodNames();

            return VendorInvoicePaymentMethod.Create(
                paymentMethodNames, name, isActive, isOnAccountPaymentType, reconcilingVendor).Value;
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { VendorInvoicePaymentMethod.MinimumLength - 1 };
                    yield return new object[] { VendorInvoicePaymentMethod.MaximumLength + 1 };
                }
            }
        }
    }
}
