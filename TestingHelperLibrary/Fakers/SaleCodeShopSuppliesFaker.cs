using Bogus;
using CustomerVehicleManagement.Domain.Entities;

namespace TestingHelperLibrary.Fakers
{
    public class SaleCodeShopSuppliesFaker : Faker<SaleCodeShopSupplies>
    {
        public SaleCodeShopSuppliesFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var percentage = (double)faker.Random.Decimal(0.01M, 1M);
                var minimumJobAmount = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
                var minimumCharge = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
                var maximumCharge = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
                var includeParts = faker.Random.Bool();
                var includeLabor = faker.Random.Bool();

                var result = SaleCodeShopSupplies.Create(percentage, minimumJobAmount, minimumCharge, maximumCharge, includeParts, includeLabor);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
