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
            string name = RandomCharacters(VendorInvoicePaymentMethod.MaximumLength - 1);
            bool isActive = true;
            //bool isOnAccountPaymentType = true;
            VendorInvoicePaymentMethodType paymentType = VendorInvoicePaymentMethodType.Normal;
            var reconcilingVendor = VendorTestHelper.CreateVendor();
            IList<string> paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);

            // Act
            var vendorInvoicePaymentMethodOrError = VendorInvoicePaymentMethod.Create(
                paymentMethodNames, name, isActive, /*isOnAccountPaymentType,*/ paymentType, reconcilingVendor);

            // Assert
            vendorInvoicePaymentMethodOrError.Value.Should().BeOfType<VendorInvoicePaymentMethod>();
            vendorInvoicePaymentMethodOrError.IsFailure.Should().BeFalse();
            vendorInvoicePaymentMethodOrError.Value.Name.Should().Be(name);
            vendorInvoicePaymentMethodOrError.Value.IsActive.Should().Be(isActive);
            //vendorInvoicePaymentMethodOrError.Value.IsOnAccountPaymentType.Should().Be(isOnAccountPaymentType);
            vendorInvoicePaymentMethodOrError.Value.PaymentType.Should().Be(paymentType);
            vendorInvoicePaymentMethodOrError.Value.ReconcilingVendor.Should().Be(reconcilingVendor);
        }

        [Fact]
        public void Not_Create_VendorInvoicePaymentMethod_With_Duplicate_Name()
        {
            string name = "Duplicate_Name";
            bool isActive = true;
            //bool isOnAccountPaymentType = true;
            VendorInvoicePaymentMethodType paymentType = VendorInvoicePaymentMethodType.Normal;
            var reconcilingVendor = VendorTestHelper.CreateVendor();
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);
            paymentMethodNames.Add(name);

            var vendorInvoicePaymentMethodOrError = VendorInvoicePaymentMethod.Create(
                paymentMethodNames, name, isActive, /*isOnAccountPaymentType,*/ paymentType, reconcilingVendor);

            vendorInvoicePaymentMethodOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_VendorInvoicePaymentMethod_With_Null_Name()
        {
            string nullName = null;
            var reconcilingVendor = VendorTestHelper.CreateVendor();
            IList<string> paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);

            var vendorInvoicePaymentMethodOrError = VendorInvoicePaymentMethod.Create(
                paymentMethodNames, nullName, true, VendorInvoicePaymentMethodType.Normal, reconcilingVendor);

            vendorInvoicePaymentMethodOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_VendorInvoicePaymentMethod_With_Invalid_VendorInvoicePaymentMethodType()
        {
            string name = RandomCharacters(VendorInvoicePaymentMethod.MaximumLength - 1);
            var reconcilingVendor = VendorTestHelper.CreateVendor();
            IList<string> paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);

            var vendorInvoicePaymentMethodOrError = VendorInvoicePaymentMethod.Create(
                paymentMethodNames, name, true, /*true,*/ (VendorInvoicePaymentMethodType)(-1), reconcilingVendor);

            vendorInvoicePaymentMethodOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_VendorInvoicePaymentMethod_With_Invalid_Name(int length)
        {
            string invalidName = RandomCharacters(length);
            var reconcilingVendor = VendorTestHelper.CreateVendor();
            IList<string> paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);

            var vendorInvoicePaymentMethodOrError = VendorInvoicePaymentMethod.Create(
                paymentMethodNames, invalidName, true, /*true,*/ VendorInvoicePaymentMethodType.Normal, reconcilingVendor);

            vendorInvoicePaymentMethodOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetName()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();

            var newName = RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 30);
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);
            vendorInvoicePaymentMethod.SetName(newName, paymentMethodNames);

            vendorInvoicePaymentMethod.Name.Should().Be(newName);
        }

        [Fact]
        public void Enable()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.IsActive.Should().BeTrue();

            vendorInvoicePaymentMethod.SetInactive();
            vendorInvoicePaymentMethod.IsActive.Should().BeFalse();
            vendorInvoicePaymentMethod.SetActive();

            vendorInvoicePaymentMethod.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Disable()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.IsActive.Should().BeTrue();

            vendorInvoicePaymentMethod.SetInactive();

            vendorInvoicePaymentMethod.IsActive.Should().BeFalse();
        }

        [Fact]
        public void SetPaymentType()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.PaymentType.Should().Be(VendorInvoicePaymentMethodType.Normal);
            var updatedPaymentType = VendorInvoicePaymentMethodType.Charge;

            vendorInvoicePaymentMethod.SetPaymentType(updatedPaymentType);

            vendorInvoicePaymentMethod.PaymentType.Should().Be(updatedPaymentType);
        }

        [Fact]
        public void SetReconcilingVendor()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();

            var reconcilingVendor = VendorTestHelper.CreateVendor();
            vendorInvoicePaymentMethod.ReconcilingVendor.Should().NotBe(reconcilingVendor);
            vendorInvoicePaymentMethod.SetReconcilingVendor(reconcilingVendor);

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

            vendorInvoicePaymentMethod.RemoveReconcilingVendor();

            vendorInvoicePaymentMethod.ReconcilingVendor.Should().BeNull();
        }

        [Fact]
        public void Not_Set_Duplicate_Name()
        {
            string name = "Name";
            bool isActive = true;
            //bool isOnAccountPaymentType = true;
            VendorInvoicePaymentMethodType paymentType = VendorInvoicePaymentMethodType.Normal;
            var reconcilingVendor = VendorTestHelper.CreateVendor();
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);
            var vendorInvoicePaymentMethod = VendorInvoicePaymentMethod.Create(
                paymentMethodNames, name, isActive, /*isOnAccountPaymentType,*/ paymentType, reconcilingVendor).Value;
            paymentMethodNames.Add(name);

            var resultOrError = vendorInvoicePaymentMethod.SetName(name, paymentMethodNames);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Null_Name()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.Name.Should().NotBeNull();
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);

            var resultOrError = vendorInvoicePaymentMethod.SetName(null, paymentMethodNames);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Name(int length)
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.Name.Should().NotBeNull();
            string invalidName = RandomCharacters(length);
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);

            var resultOrError = vendorInvoicePaymentMethod.SetName(invalidName, paymentMethodNames);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_PaymentType()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            vendorInvoicePaymentMethod.PaymentType.Should().Be(VendorInvoicePaymentMethodType.Normal);
            var invalidPaymentType = (VendorInvoicePaymentMethodType)(-1);

            var resultOrError = vendorInvoicePaymentMethod.SetPaymentType(invalidPaymentType);

            resultOrError.IsFailure.Should().BeTrue();
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
