using Bogus;
using CustomerVehicleManagement.Domain.Entities;

namespace TestingHelperLibrary.Fakers
{
    public class VehicleFaker : Faker<Vehicle>
    {
        public VehicleFaker(bool generateId = false)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var vin = faker.Random.Replace("?????????????????");
                var year = faker.Random.Number(DateTime.Now.AddYears(-30).Year, DateTime.Now.AddYears(+1).Year);
                var makes = faker.PickRandom(VehicleTestHelper.Makers.ToList());
                var make = makes.Value;
                var model = faker.PickRandom(VehicleTestHelper.Models[makes.Key]);

                var result = Vehicle.Create(vin, year, make, model);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
