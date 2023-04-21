using Bogus;
using CustomerVehicleManagement.Domain.Entities.Payables;

namespace TestingHelperLibrary.Fakers
{
    public class VendorInvoiceTaxFaker : Faker<VendorInvoiceTax>
    {
        public VendorInvoiceTaxFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var salesTax = new SalesTaxFaker(generateId).Generate();
                var amount = faker.Random.Double(1, 1000.0);

                var result = VendorInvoiceTax.Create(salesTax, amount);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
