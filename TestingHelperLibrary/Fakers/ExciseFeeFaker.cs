using Bogus;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using Menominee.Common.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class ExciseFeeFaker : Faker<ExciseFee>
    {
        public ExciseFeeFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var description = faker.Random.String2(1, SalesTax.DescriptionMaximumLength);
                var feeType = faker.PickRandom<ExciseFeeType>();
                var amount = faker.Random.Double(0, 100);
                var result = ExciseFee.Create(
                    description,
                    feeType,
                    amount);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
