using Bogus;
using CustomerVehicleManagement.Domain.Entities;
using System;

namespace CustomerVehicleManagement.Tests.Helpers.Fakers
{
    public class SaleCodeFaker : Faker<SaleCode>
    {
        public SaleCodeFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                string name = faker.Random.String2(SaleCode.MinimumLength, SaleCode.MaximumLength);
                string code = faker.Random.String2(SaleCode.MinimumLength, SaleCode.MaximumLength);
                double laborRate = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
                double desiredMargin = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
                SaleCodeShopSupplies shopSupplies = new SaleCodeShopSuppliesFaker(generateId).Generate();

                var result = SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
