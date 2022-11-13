using CustomerVehicleManagement.Domain.Entities.Payables;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using static CustomerVehicleManagement.Tests.Unit.Entities.VendorInvoiceTestHelper;
namespace CustomerVehicleManagement.Tests.Unit.ValueObjects
{
    public class DefaultPaymentMethodShould
    {
        [Fact]
        public void Create_DefaultPaymentMethod()
        {
            // Arrange
            VendorInvoicePaymentMethod paymentMethod = CreateVendorInvoicePaymentMethod();
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
            true.Should().BeFalse();
        }



        private VendorInvoicePaymentMethod CreateVendorInvoicePaymentMethod()
        {
            IList<string> paymentMethodNames = CreatePaymentMethodNames();

            return VendorInvoicePaymentMethod.Create(
                paymentMethodNames,
                Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength),
                true,
                true,
                reconcilingVendor: null).Value;
        }
    }
}
