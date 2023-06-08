using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentAssertions;
using Menominee.Common.Enums;
using System.Collections.Generic;
using TestingHelperLibrary.Payables;
using Xunit;
using static CustomerVehicleManagement.Tests.Utilities;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class VendorInvoicePaymentMethodShould
    {
        [Fact]
        public void Not_Set_Null_ReconcilingVendor()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            var reconcilingVendor = VendorTestHelper.CreateVendor();
            vendorInvoicePaymentMethod.ReconcilingVendor.Should().NotBe(reconcilingVendor);
            vendorInvoicePaymentMethod.SetReconcilingVendor(reconcilingVendor);
            vendorInvoicePaymentMethod.ReconcilingVendor.Should().Be(reconcilingVendor);

            var resultOrError = vendorInvoicePaymentMethod.SetReconcilingVendor(null);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Create_VendorInvoicePaymentMethod()
        {
            // Arrange
            var name = RandomCharacters(VendorInvoicePaymentMethod.MaximumLength - 1);
            var isActive = true;
            var paymentType = VendorInvoicePaymentMethodType.Normal;
            var reconcilingVendor = VendorTestHelper.CreateVendor();
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);

            // Act
            var result = VendorInvoicePaymentMethod.Create(
                (IReadOnlyList<string>)paymentMethodNames, name, isActive, paymentType, reconcilingVendor);

            // Assert
            result.Value.Should().BeOfType<VendorInvoicePaymentMethod>();
            result.IsFailure.Should().BeFalse();
            result.Value.Name.Should().Be(name);
            result.Value.IsActive.Should().Be(isActive);
            result.Value.PaymentType.Should().Be(paymentType);
            result.Value.ReconcilingVendor.Should().Be(reconcilingVendor);
        }

        [Fact]
        public void Not_Create_VendorInvoicePaymentMethod_With_Duplicate_Name()
        {
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);
            var duplicateName = paymentMethodNames[0];

            var result = VendorInvoicePaymentMethod.Create(
                (IReadOnlyList<string>)paymentMethodNames,
                duplicateName,
                true,
                VendorInvoicePaymentMethodType.Normal, VendorTestHelper.CreateVendor());

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_VendorInvoicePaymentMethod_With_Null_Name()
        {
            string nullName = null;
            var reconcilingVendor = VendorTestHelper.CreateVendor();
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);

            var result = VendorInvoicePaymentMethod.Create(
                (IReadOnlyList<string>)paymentMethodNames, nullName, true, /*true,*/ VendorInvoicePaymentMethodType.Normal, reconcilingVendor);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_VendorInvoicePaymentMethod_With_Invalid_VendorInvoicePaymentMethodType()
        {
            string name = RandomCharacters(VendorInvoicePaymentMethod.MaximumLength - 1);
            var reconcilingVendor = VendorTestHelper.CreateVendor();
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);

            var result = VendorInvoicePaymentMethod.Create(
                (IReadOnlyList<string>)paymentMethodNames, name, true, /*true,*/ (VendorInvoicePaymentMethodType)(-1), reconcilingVendor);

            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_VendorInvoicePaymentMethod_With_Invalid_Name(int length)
        {
            string invalidName = RandomCharacters(length);
            var reconcilingVendor = VendorTestHelper.CreateVendor();
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);

            var result = VendorInvoicePaymentMethod.Create(
                (IReadOnlyList<string>)paymentMethodNames, invalidName, true, /*true,*/ VendorInvoicePaymentMethodType.Normal, reconcilingVendor);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain(VendorInvoicePaymentMethod.InvalidLengthMessage);
        }

        [Fact]
        public void SetName()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            var newName = RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 30);
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);

            var result = vendorInvoicePaymentMethod.SetName(newName, (IReadOnlyList<string>)paymentMethodNames);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(newName);
        }

        [Fact]
        public void Activate()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.IsActive.Should().BeTrue();

            vendorInvoicePaymentMethod.Deactivate();
            vendorInvoicePaymentMethod.IsActive.Should().BeFalse();
            vendorInvoicePaymentMethod.Activate();

            vendorInvoicePaymentMethod.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Deactivate()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.IsActive.Should().BeTrue();

            vendorInvoicePaymentMethod.Deactivate();

            vendorInvoicePaymentMethod.IsActive.Should().BeFalse();
        }

        [Fact]
        public void SetPaymentType()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.PaymentType.Should().Be(VendorInvoicePaymentMethodType.Normal);
            var updatedPaymentType = VendorInvoicePaymentMethodType.Charge;

            var result = vendorInvoicePaymentMethod.SetPaymentType(updatedPaymentType);

            result.IsSuccess.Should().BeTrue();
            vendorInvoicePaymentMethod.PaymentType.Should().Be(updatedPaymentType);
        }

        [Fact]
        public void SetReconcilingVendor()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            var reconcilingVendor = VendorTestHelper.CreateVendor();
            vendorInvoicePaymentMethod.ReconcilingVendor.Should().NotBe(reconcilingVendor);

            var result = vendorInvoicePaymentMethod.SetReconcilingVendor(reconcilingVendor);

            result.IsSuccess.Should().BeTrue();
            vendorInvoicePaymentMethod.ReconcilingVendor.Should().Be(reconcilingVendor);
        }

        [Fact]
        public void RemoveReconcilingVendor()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            var reconcilingVendor = VendorTestHelper.CreateVendor();
            vendorInvoicePaymentMethod.ReconcilingVendor.Should().NotBe(reconcilingVendor);
            vendorInvoicePaymentMethod.SetReconcilingVendor(reconcilingVendor);
            vendorInvoicePaymentMethod.ReconcilingVendor.Should().Be(reconcilingVendor);

            var result = vendorInvoicePaymentMethod.RemoveReconcilingVendor();

            result.IsSuccess.Should().BeTrue();
            vendorInvoicePaymentMethod.ReconcilingVendor.Should().BeNull();
        }

        [Fact]
        public void Not_Set_Duplicate_Name()
        {
            var name = "Name";
            var paymentType = VendorInvoicePaymentMethodType.Normal;
            var reconcilingVendor = VendorTestHelper.CreateVendor();
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);
            var duplicateName = paymentMethodNames[0];
            var vendorInvoicePaymentMethod = VendorInvoicePaymentMethod.Create(
                (IReadOnlyList<string>)paymentMethodNames,
                name,
                true,
                paymentType,
                reconcilingVendor)
                .Value;

            var result = vendorInvoicePaymentMethod.SetName(duplicateName, (IReadOnlyList<string>)paymentMethodNames);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain(VendorInvoicePaymentMethod.NonuniqueMessage);
        }

        [Fact]
        public void Not_Set_Null_Name()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.Name.Should().NotBeNull();
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);

            var result = vendorInvoicePaymentMethod.SetName(null, (IReadOnlyList<string>)paymentMethodNames);

            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Name(int length)
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.Name.Should().NotBeNull();
            string invalidName = RandomCharacters(length);
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);

            var result = vendorInvoicePaymentMethod.SetName(invalidName, (IReadOnlyList<string>)paymentMethodNames);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_PaymentType()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.PaymentType.Should().Be(VendorInvoicePaymentMethodType.Normal);
            var invalidPaymentType = (VendorInvoicePaymentMethodType)(-1);

            var result = vendorInvoicePaymentMethod.SetPaymentType(invalidPaymentType);

            result.IsFailure.Should().BeTrue();
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
