using Bogus;
using CustomerVehicleManagement.Domain.Entities.Payables;

namespace TestingHelperLibrary.Fakers
{
    public class VendorInvoicePaymentFaker : Faker<VendorInvoicePayment>
    {
        public VendorInvoicePaymentFaker(bool generateId = false, long id = 0)
        {
            if (generateId)
                RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            if (id > 0)
                RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);

            CustomInstantiator(faker =>
            {
                var paymentMethod = new VendorInvoicePaymentMethodFaker(generateId, id).Generate();
                var amount = Math.Round(faker.Random.Double(1, 10000), 2);

                var result = VendorInvoicePayment.Create(paymentMethod, amount);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
