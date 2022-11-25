using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using static CustomerVehicleManagement.Tests.Utilities;

namespace CustomerVehicleManagement.Tests.Unit.ValueObjects
{
    public class DefaultPaymentMethodShould
    {
        [Fact]
        public void Create_DefaultPaymentMethod()
        {
            // Arrange
            var paymentMethod = CreateVendorInvoicePaymentMethod();

            // Act
            var resultOrError = DefaultPaymentMethod.Create(paymentMethod, true);

            // Assert
            resultOrError.Value.Should().BeOfType<DefaultPaymentMethod>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Not_Create_DefaultPaymentMethod_With_Null_PaymentMethod()
        {
            VendorInvoicePaymentMethod paymentMethod = null;

            var resultOrError = DefaultPaymentMethod.Create(paymentMethod, true);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetPaymentMethod()
        {
            var paymentMethod = CreateVendorInvoicePaymentMethod();
            var defaultPaymentMethod = DefaultPaymentMethod.Create(paymentMethod, true).Value;
            var anotherPaymentMethod = CreateVendorInvoicePaymentMethod();

            defaultPaymentMethod.PaymentMethod.Should().Be(paymentMethod);
            var resultOrError = defaultPaymentMethod.SetPaymentMethod(anotherPaymentMethod);

            resultOrError.IsFailure.Should().BeFalse();
            defaultPaymentMethod.PaymentMethod.Should().Be(anotherPaymentMethod);
        }

        [Fact]
        public void Not_SetPaymentMethod_With_Null_PaymentMethod()
        {
            var paymentMethod = CreateVendorInvoicePaymentMethod();
            var defaultPaymentMethod = DefaultPaymentMethod.Create(paymentMethod, true).Value;
            VendorInvoicePaymentMethod nullPaymentMethod = null;

            var resultOrError = defaultPaymentMethod.SetPaymentMethod(nullPaymentMethod);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetAutoCompleteDocuments()
        {
            var defaultPaymentMethod = DefaultPaymentMethod.Create(CreateVendorInvoicePaymentMethod(), true).Value;

            defaultPaymentMethod.AutoCompleteDocuments.Should().Be(true);
            var resultOrError = defaultPaymentMethod.SetAutoCompleteDocuments(false);

            resultOrError.IsFailure.Should().BeFalse();
            defaultPaymentMethod.AutoCompleteDocuments.Should().Be(false);
        }

        private static VendorInvoicePaymentMethod CreateVendorInvoicePaymentMethod()
        {
            IList<string> paymentMethodNames = CreatePaymentMethodNames();

            return VendorInvoicePaymentMethod.Create(
                paymentMethodNames,
                RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 5),
                true,
                true,
                reconcilingVendor: null).Value;
        }
    }
}
