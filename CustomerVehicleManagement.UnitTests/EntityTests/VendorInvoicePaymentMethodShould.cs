﻿using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using static CustomerVehicleManagement.UnitTests.EntityTests.VendorInvoiceTestHelper;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class VendorInvoicePaymentMethodShould
    {
        [Fact]
        public void Create_VendorInvoicePaymentMethod()
        {
            // Arrange
            string name = Utilities.RandomCharacters(VendorInvoicePaymentMethod.MaximumLength - 1); ;
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
            var paymentMethodNames = CreatePaymentMethodNames();
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

            var newName = Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 30);
            var paymentMethodNames = CreatePaymentMethodNames();
            vendorInvoicePaymentMethod.SetName(newName, paymentMethodNames);

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
        public void Not_Set_Duplicate_Name()
        {
            string name = "Name";
            bool isActive = true;
            bool isOnAccountPaymentType = true;
            var reconcilingVendor = CreateVendor();
            var paymentMethodNames = CreatePaymentMethodNames();

            var vendorInvoicePaymentMethod= VendorInvoicePaymentMethod.Create(
                paymentMethodNames, name, isActive, isOnAccountPaymentType, reconcilingVendor).Value;
            paymentMethodNames.Add(name);

            Assert.Throws<ArgumentException>(() => vendorInvoicePaymentMethod.SetName(name, paymentMethodNames));
        }

        [Fact]
        public void Not_Set_Null_Name()
        {
            var vendorInvoicePaymentMethod = CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.Name.Should().NotBeNull();

            string newName = null;
            var paymentMethodNames = CreatePaymentMethodNames();

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoicePaymentMethod.SetName(newName, paymentMethodNames));
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Name(int length)
        {
            var vendorInvoicePaymentMethod = CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.Name.Should().NotBeNull();

            string invalidName = Utilities.RandomCharacters(length);
            var paymentMethodNames = CreatePaymentMethodNames();

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoicePaymentMethod.SetName(invalidName, paymentMethodNames));
        }

        [Fact]
        public void Not_Set_Null_ReconcilingVendor()
        {
            var vendorInvoicePaymentMethod = CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.ReconcilingVendor.Should().NotBeNull();

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoicePaymentMethod.SetReconcilingVendor(null));
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