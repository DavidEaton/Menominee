using Bogus;
using CustomerVehicleManagement.Domain.Entities;

namespace TestingHelperLibrary.Fakers
{
    public class SaleCodeFaker : Faker<SaleCode>
    {
        public SaleCodeFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var name = faker.Random.String2(SaleCode.MinimumLength, SaleCode.MaximumLength);
                var code = faker.Random.String2(SaleCode.MinimumLength, SaleCode.MaximumLength);
                var laborRate = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
                var desiredMargin = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
                var shopSupplies = new SaleCodeShopSuppliesFaker(generateId).Generate();

                var result = SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
