using Bogus;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderServiceTaxFaker : Faker<RepairOrderServiceTax>
    {
        public RepairOrderServiceTaxFaker(bool generateId = false)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var employee = Employee.Create(new PersonFaker(generateId).Generate(), new List<RoleAssignment>()).Value;
                var rate = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
                var amount = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
                var result = RepairOrderServiceTax.Create(
                    PartTax.Create(rate, amount).Value,
                    LaborTax.Create(rate, amount).Value);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });

        }
    }
}
