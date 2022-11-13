using CSharpFunctionalExtensions;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class DefaultPaymentMethod : AppValueObject
    {
        public static readonly string RequiredMessage = $"Please include all required items.";

        public VendorInvoicePaymentMethod PaymentMethod { get; private set; }
        public bool AutoCompleteDocuments { get; private set; }

        private DefaultPaymentMethod(VendorInvoicePaymentMethod paymentMethod, bool autoCompleteDocuments)
        {
            if (paymentMethod is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            PaymentMethod = paymentMethod;
            AutoCompleteDocuments = autoCompleteDocuments;
        }

        public static Result<DefaultPaymentMethod> Create(VendorInvoicePaymentMethod paymentMethod, bool autoCompleteDocuments)
        {
            if (paymentMethod is null)
                return Result.Failure<DefaultPaymentMethod>(RequiredMessage);

            return Result.Success(new DefaultPaymentMethod(paymentMethod, autoCompleteDocuments));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PaymentMethod;
            yield return AutoCompleteDocuments;
        }

        #region ORM

        // EF requires a parameterless constructor
        protected DefaultPaymentMethod() { }

        #endregion

    }

}
