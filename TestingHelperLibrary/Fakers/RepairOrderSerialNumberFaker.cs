using Bogus;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;

namespace TestingHelperLibrary.Fakers
{
    public class RepairOrderSerialNumberFaker : Faker<RepairOrderSerialNumber>
    {
        public RepairOrderSerialNumberFaker(bool generateId = false)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var serialNumber = faker.Random.AlphaNumeric(faker.Random.Int(
                    RepairOrderSerialNumber.MinimumLength, RepairOrderSerialNumber.MaximumLength));
                var result = RepairOrderSerialNumber.Create(serialNumber);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
