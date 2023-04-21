using Bogus;
using CustomerVehicleManagement.Domain.Entities.Payables;
using System;

namespace TestingHelperLibrary.Fakers
{
    public class DefaultPaymentMethodFaker : Faker<DefaultPaymentMethod>
    {
        public DefaultPaymentMethodFaker(bool generateId)
        {
            CustomInstantiator(faker =>
            {
                VendorInvoicePaymentMethod paymentMethod = new VendorInvoicePaymentMethodFaker(generateId).Generate();
                bool autoCompleteDocuments = faker.Random.Bool();

                var result = DefaultPaymentMethod.Create(paymentMethod, autoCompleteDocuments);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });

        }
    }
}
