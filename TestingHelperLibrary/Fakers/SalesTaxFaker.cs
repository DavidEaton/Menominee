using Bogus;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class SalesTaxFaker : Faker<SalesTax>
    {
        public SalesTaxFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var description = faker.Random.String2(1, SalesTax.DescriptionMaximumLength);
                var taxType = faker.PickRandom<SalesTaxType>();
                var order = faker.Random.Int(0, 100);
                var taxIdNumber = faker.Random.String2(0, SalesTax.TaxIdNumberMaximumLength);
                var partTaxRate = faker.Random.Double(0, 100);
                var laborTaxRate = faker.Random.Double(0, 100);
                var exciseFees = new ExciseFeeFaker(generateId).Generate(3);
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
