using Bogus;
using Menominee.Domain.Entities.Taxes;
using Menominee.Domain.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class ExciseFeeFaker : Faker<ExciseFee>
    {
        public ExciseFeeFaker(bool generateId = false, long id = 0)
        {
            if (generateId)
                RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            if (id > 0)
                RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);

            CustomInstantiator(faker =>
            {
                var description = faker.Lorem.Sentence();
                var feeType = faker.PickRandom<ExciseFeeType>();
                var amount = Math.Round(faker.Random.Double(0, 100), 2);
                var result = ExciseFee.Create(
                    description,
                    feeType,
                    amount);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
