using Bogus;
using Menominee.Domain.Entities;
using Menominee.Domain.Enums;
using TestingHelperLibrary;

namespace Menominee.TestingHelperLibrary.Fakers;

public class VehicleFaker : Faker<Vehicle>
{
    public VehicleFaker(bool generateId = false)
    {
        RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);
        GenerateVehicle();
    }

    public VehicleFaker(long id = 0)
    {
        RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);
        GenerateVehicle();
    }

    private void GenerateVehicle()
    {
        CustomInstantiator(faker =>
        {
            var vin = faker.Random.Replace("?????????????????");
            var year = faker.Random.Number(DateTime.Now.AddYears(-30).Year, DateTime.Now.AddYears(+1).Year);
            var makes = faker.PickRandom(VehicleTestHelper.Makers.ToList());
            var make = makes.Value;
            var model = faker.PickRandom(VehicleTestHelper.Models[makes.Key]);
            var plate = faker.Random.Replace("???????");
            var plateStateProvince = faker.PickRandom<State>();
            var unitNumber = faker.Random.Replace("???????");
            var color = faker.Random.Replace("?????");
            var active = faker.Random.Bool();

            var result = Vehicle.Create(vin, year, make, model, plate, plateStateProvince, unitNumber, color, active);

            return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
        });
    }
}
