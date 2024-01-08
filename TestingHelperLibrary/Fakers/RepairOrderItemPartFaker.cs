using Bogus;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Domain.Enums;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderItemPartFaker : Faker<RepairOrderItemPart>
    {
        public RepairOrderItemPartFaker(bool generateId = false)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var list = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
                var cost = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
                var core = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
                var retail = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
                var techAmount = TechAmount.Create(
                    faker.PickRandom<ItemLaborType>(),
                    (double)Math.Round(faker.Random.Decimal(1, 1000), 2), SkillLevel.A)
                    .Value;

                var result = RepairOrderItemPart.Create(list, cost, core, retail, techAmount, false);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
