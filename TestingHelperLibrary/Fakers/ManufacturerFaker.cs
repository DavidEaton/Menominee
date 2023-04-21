using Bogus;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using System;

namespace TestingHelperLibrary.Fakers
{
    public class ManufacturerFaker : Faker<Manufacturer>
    {
        public ManufacturerFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                string name = faker.Company.CompanyName();
                string prefix = faker.Random.AlphaNumeric(3).ToUpper();
                string code = name[..3].ToUpper();

                var result = Manufacturer.Create(name, prefix, code);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
