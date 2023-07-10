using Menominee.Domain.Entities.Payables;
using FluentAssertions;
using Menominee.Common.Enums;
using System.Collections.Generic;
using TestingHelperLibrary.Payables;
using Xunit;
using static Menominee.Tests.Utilities;

namespace Menominee.Tests.ValueObjects
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
        public void Return_New_On_NewPaymentMethod()
        {
            var paymentMethod = CreateVendorInvoicePaymentMethod();
            var defaultPaymentMethod = DefaultPaymentMethod.Create(paymentMethod, true).Value;
            var anotherPaymentMethod = CreateVendorInvoicePaymentMethod();

            defaultPaymentMethod.PaymentMethod.Should().Be(paymentMethod);
            var resultOrError = defaultPaymentMethod.NewPaymentMethod(anotherPaymentMethod);

            resultOrError.IsSuccess.Should().BeTrue();
            resultOrError.Value.PaymentMethod.Should().Be(anotherPaymentMethod);
        }

        [Fact]
        public void Not_SetPaymentMethod_With_Null_PaymentMethod()
        {
            var paymentMethod = CreateVendorInvoicePaymentMethod();
            var defaultPaymentMethod = DefaultPaymentMethod.Create(paymentMethod, true).Value;
            VendorInvoicePaymentMethod nullPaymentMethod = null;

            var resultOrError = defaultPaymentMethod.NewPaymentMethod(nullPaymentMethod);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_New_On_NewAutoCompleteDocuments()
        {
            var defaultPaymentMethod = DefaultPaymentMethod.Create(CreateVendorInvoicePaymentMethod(), true).Value;

            defaultPaymentMethod.AutoCompleteDocuments.Should().Be(true);
            var resultOrError = defaultPaymentMethod.SetAutoCompleteDocuments(false);

            resultOrError.IsFailure.Should().BeFalse();
            resultOrError.Value.AutoCompleteDocuments.Should().Be(false);
        }


        [Fact]
        public void Equate_Two_Instances_Having_Same_Values()
        {
            VendorInvoicePaymentMethod paymentMethod = CreateVendorInvoicePaymentMethod();

            var defaultPaymentMethod1 = DefaultPaymentMethod.Create(paymentMethod, true).Value;
            var defaultPaymentMethod2 = DefaultPaymentMethod.Create(paymentMethod, true).Value;

            defaultPaymentMethod1.Should().BeEquivalentTo(defaultPaymentMethod2);
        }

        [Fact]
        public void Not_Equate_Two_Instances_Having_Differing_Values()
        {
            VendorInvoicePaymentMethod paymentMethod = CreateVendorInvoicePaymentMethod();

            var defaultPaymentMethod1 = DefaultPaymentMethod.Create(paymentMethod, true).Value;
            var defaultPaymentMethod2 = DefaultPaymentMethod.Create(paymentMethod, false).Value;

            defaultPaymentMethod1.Should().NotBeSameAs(defaultPaymentMethod2);
        }

        private static VendorInvoicePaymentMethod CreateVendorInvoicePaymentMethod()
        {
            var paymentMethodNames = VendorInvoiceTestHelper.CreatePaymentMethodNames(5);

            return VendorInvoicePaymentMethod.Create(
                (IReadOnlyList<string>)paymentMethodNames,
                RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 5),
                true,
                VendorInvoicePaymentMethodType.Normal,
                reconcilingVendor: null).Value;
        }
    }
}
