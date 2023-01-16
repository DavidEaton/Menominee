using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentAssertions;
using Xunit;
using static CustomerVehicleManagement.Tests.Utilities;

namespace CustomerVehicleManagement.Tests.Unit.Entities
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

            var resultOrError = vendorInvoicePayment.SetPaymentMethod(vendorInvoicePaymentMethod);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Invalid_Amount()
        {
            var invalidAmount = VendorInvoicePayment.InvalidValue;
            var vendorInvoicePayment = CreateVendorInvoicePayment();

            var resultOrError = vendorInvoicePayment.SetAmount(invalidAmount);

            resultOrError.IsFailure.Should().BeTrue();
        }
    }
}
