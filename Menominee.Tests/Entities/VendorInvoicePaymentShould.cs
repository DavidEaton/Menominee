using Menominee.Domain.Entities.Payables;
using FluentAssertions;
using TestingHelperLibrary.Payables;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class VendorInvoicePaymentShould
    {
        [Fact]
        public void Create_VendorInvoicePayment()
        {
            // Arrange
            var paymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
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
            VendorInvoicePaymentMethod paymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            double invalidAmount = VendorInvoicePayment.InvalidValue;

            var paymentOrError = VendorInvoicePayment.Create(paymentMethod, invalidAmount);

            paymentOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetPaymentMethod()
        {
            var vendorInvoicePaymentMethod = VendorInvoiceTestHelper.CreateVendorInvoicePaymentMethod();
            var vendorInvoicePayment = VendorInvoiceTestHelper.CreateVendorInvoicePayment();

            vendorInvoicePayment.SetPaymentMethod(vendorInvoicePaymentMethod);

            vendorInvoicePayment.PaymentMethod.Should().Be(vendorInvoicePaymentMethod);
        }

        [Fact]
        public void SetAmount()
        {
            var vendorInvoicePayment = VendorInvoiceTestHelper.CreateVendorInvoicePayment();
            var amount = VendorInvoicePayment.InvalidValue + 1.0;

            vendorInvoicePayment.SetAmount(amount);

            vendorInvoicePayment.Amount.Should().Be(amount);
        }

        [Fact]
        public void Not_Set_Null_PaymentMethod()
        {
            VendorInvoicePaymentMethod vendorInvoicePaymentMethod = null;
            var vendorInvoicePayment = VendorInvoiceTestHelper.CreateVendorInvoicePayment();

            var resultOrError = vendorInvoicePayment.SetPaymentMethod(vendorInvoicePaymentMethod);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_Amount()
        {
            var invalidAmount = VendorInvoicePayment.InvalidValue;
            var vendorInvoicePayment = VendorInvoiceTestHelper.CreateVendorInvoicePayment();

            var resultOrError = vendorInvoicePayment.SetAmount(invalidAmount);

            resultOrError.IsFailure.Should().BeTrue();
        }
    }
}
