using Bogus;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderStatusFaker : Faker<RepairOrderStatus>
    {
        public RepairOrderStatusFaker(bool generateId = false)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var status = faker.PickRandom<Status>();
                var sentence = faker.Lorem.Sentence();
                var description = sentence.Substring(0, Math.Min(50, sentence.Length));

                var result = RepairOrderStatus.Create(status, description);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}