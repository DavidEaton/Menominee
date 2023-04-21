using Bogus;
using CustomerVehicleManagement.Domain.Entities.Payables;

namespace TestingHelperLibrary.Fakers
{
    public class VendorInvoicePaymentFaker : Faker<VendorInvoicePayment>
    {
        public VendorInvoicePaymentFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var paymentMethod = new VendorInvoicePaymentMethodFaker(generateId).Generate();
                var amount = faker.Random.Double(1, 10000);

                var result = VendorInvoicePayment.Create(paymentMethod, amount);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
