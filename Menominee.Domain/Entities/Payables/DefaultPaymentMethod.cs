using CSharpFunctionalExtensions;
using Menominee.Domain.ValueObjects;
using System.Collections.Generic;

namespace Menominee.Domain.Entities.Payables
{
    public class DefaultPaymentMethod : AppValueObject
    {
        public static readonly string RequiredMessage = $"Please include all required items.";

        public VendorInvoicePaymentMethod PaymentMethod { get; private set; }
        public bool AutoCompleteDocuments { get; private set; }

        private DefaultPaymentMethod(VendorInvoicePaymentMethod paymentMethod, bool autoCompleteDocuments)
        {
            PaymentMethod = paymentMethod;
            AutoCompleteDocuments = autoCompleteDocuments;
        }

        public static Result<DefaultPaymentMethod> Create(VendorInvoicePaymentMethod paymentMethod, bool autoCompleteDocuments)
        {
            if (paymentMethod is null)
                return Result.Failure<DefaultPaymentMethod>(RequiredMessage);

            return Result.Success(new DefaultPaymentMethod(paymentMethod, autoCompleteDocuments));
        }

        public Result<DefaultPaymentMethod> NewPaymentMethod(VendorInvoicePaymentMethod paymentMethod)
        {
            if (paymentMethod is null)
                return Result.Failure<DefaultPaymentMethod>(RequiredMessage);

            return Result.Success(new DefaultPaymentMethod(paymentMethod, AutoCompleteDocuments));
        }

        public Result<DefaultPaymentMethod> SetAutoCompleteDocuments(bool autoCompleteDocuments)
        {
            return Result.Success(new DefaultPaymentMethod(PaymentMethod, autoCompleteDocuments));
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
