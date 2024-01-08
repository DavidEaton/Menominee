using Bogus;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Domain.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderItemLaborFaker : Faker<RepairOrderItemLabor>
    {
        public RepairOrderItemLaborFaker(bool generateId = false)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var techAmount = TechAmount.Create(
                    faker.PickRandom<ItemLaborType>(),
                    (double)Math.Round(faker.Random.Decimal(1, 1000), 2), SkillLevel.A)
                    .Value;
                var laborAmount = LaborAmount.Create(
                    faker.PickRandom<ItemLaborType>(),
                    (double)Math.Round(faker.Random.Decimal(1, 1000), 2))
                    .Value;

                var result = RepairOrderItemLabor.Create(laborAmount, techAmount);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
