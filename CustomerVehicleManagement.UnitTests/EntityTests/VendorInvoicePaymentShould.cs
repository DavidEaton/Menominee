using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentAssertions;
using System;
using Xunit;
using static CustomerVehicleManagement.UnitTests.EntityTests.VendorInvoiceTestHelper;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class VendorInvoicePaymentShould
    {
        [Fact]
        public void Create_VendorInvoicePayment()
        {
            // Arrange
            var paymentMethod = CreateVendorInvoicePaymentMethod();
            double amount = VendorInvoicePayment.InvalidValue + 1.0;

            // Act
            var paymentOrError = VendorInvoicePayment.Create(paymentMethod, amount);

            // Assert
            paymentOrError.Value.Should().BeOfType<VendorInvoicePayment>();
            paymentOrError.IsFailure.Should().BeFalse();
            paymentOrError.Value.PaymentMethod.Should().Be(paymentMethod);
            paymentOrError.Value.Amount.Should().Be(amount);
        }

        [Fact]
        public void Not_Create_VendorInvoicePayment_With_Null_PaymentMethod()
        {
            VendorInvoicePaymentMethod paymentMethod = null;
            double amount = VendorInvoicePayment.InvalidValue + 1.0;

            var paymentOrError = VendorInvoicePayment.Create(paymentMethod, amount);

            paymentOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_VendorInvoicePayment_With_Invalid_Amount()
        {
            VendorInvoicePaymentMethod paymentMethod = CreateVendorInvoicePaymentMethod();
            double invalidAmount = VendorInvoicePayment.InvalidValue;

            var paymentOrError = VendorInvoicePayment.Create(paymentMethod, invalidAmount);

            paymentOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetPaymentMethod()
        {
            var vendorInvoicePaymentMethod = CreateVendorInvoicePaymentMethod();
            var vendorInvoicePayment = CreateVendorInvoicePayment();

            vendorInvoicePayment.SetPaymentMethod(vendorInvoicePaymentMethod);

            vendorInvoicePayment.PaymentMethod.Should().Be(vendorInvoicePaymentMethod);
        }

        [Fact]
        public void SetAmount()
        {
            var vendorInvoicePayment = CreateVendorInvoicePayment();
            var amount = VendorInvoicePayment.InvalidValue + 1.0;

            vendorInvoicePayment.SetAmount(amount);

            vendorInvoicePayment.Amount.Should().Be(amount);
        }

        [Fact]
        public void Not_Set_Null_PaymentMethod()
        {
            VendorInvoicePaymentMethod vendorInvoicePaymentMethod = null;
            var vendorInvoicePayment = CreateVendorInvoicePayment();

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoicePayment.SetPaymentMethod(vendorInvoicePaymentMethod));
        }

        [Fact]
        public void Not_Set_Invalid_Amount()
        {
            var invalidAmount = VendorInvoicePayment.InvalidValue;
            var vendorInvoicePayment = CreateVendorInvoicePayment();

            Assert.Throws<ArgumentOutOfRangeException>(() => vendorInvoicePayment.SetAmount(invalidAmount));
        }
    }
}
