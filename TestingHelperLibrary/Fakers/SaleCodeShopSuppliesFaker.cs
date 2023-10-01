using Bogus;
using Menominee.Domain.Entities;

namespace TestingHelperLibrary.Fakers
{
    public class SaleCodeShopSuppliesFaker : Faker<SaleCodeShopSupplies>
    {
        public SaleCodeShopSuppliesFaker(bool generateId = false, long id = 0)
        {
            if (generateId)
                RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            if (id > 0)
                RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);

            CustomInstantiator(faker =>
            {
                var percentage = (double)faker.Random.Double(0.01, 1);
                var minimumJobAmount = (double)Math.Round(faker.Random.Double(SaleCodeShopSupplies.MinimumValue, 1000), 2);
                var minimumCharge = (double)Math.Round(faker.Random.Double(SaleCodeShopSupplies.MinimumValue, 1000), 2);
                var maximumCharge = (double)Math.Round(faker.Random.Double(SaleCodeShopSupplies.MinimumValue, 1000), 2);
                var includeParts = faker.Random.Bool();
                var includeLabor = faker.Random.Bool();

                var result = SaleCodeShopSupplies.Create(percentage, minimumJobAmount, minimumCharge, maximumCharge, includeParts, includeLabor);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
