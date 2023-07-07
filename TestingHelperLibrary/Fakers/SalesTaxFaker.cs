using Bogus;
using Bogus.Extensions.UnitedStates;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class SalesTaxFaker : Faker<SalesTax>
    {
        public SalesTaxFaker(bool generateId = false, long id = 0)
        {
            if (generateId)
                RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            if (id > 0)
                RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);

            CustomInstantiator(faker =>
            {
                var description = faker.Commerce.ProductAdjective();
                var taxType = faker.PickRandom<SalesTaxType>();
                var order = faker.Random.Int(0, 100);
                var taxIdNumber = faker.Company.Ein();
                var partTaxRate = Math.Round(faker.Random.Double(0, 100), 2);
                var laborTaxRate = Math.Round(faker.Random.Double(0, 100), 2);
                var exciseFees = new ExciseFeeFaker(generateId, id).Generate(3);
                var isAppliedByDefault = faker.Random.Bool() ? (bool?)faker.Random.Bool() : null;
                var isTaxable = faker.Random.Bool() ? (bool?)faker.Random.Bool() : null;

                var result = SalesTax.Create(
                    description,
                    taxType,
                    order,
                    taxIdNumber,
                    partTaxRate,
                    laborTaxRate,
                    exciseFees,
                    isAppliedByDefault,
                    isTaxable
                );

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }

    }
}
