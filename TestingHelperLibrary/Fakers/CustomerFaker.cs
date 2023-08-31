using Bogus;
using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Domain.Entities;
using Entity = Menominee.Common.Entity;
using Person = Menominee.Domain.Entities.Person;

namespace TestingHelperLibrary.Fakers
{
    public class CustomerFaker : Faker<Customer>
    {
        public CustomerFaker(
            bool generateId = false,
            bool includeAddress = false,
            int emailsCount = 0,
            int phonesCount = 0,
            int vehiclesCount = 0,
            Person person = null,
            Business business = null)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var customerType = faker.PickRandom<CustomerType>();
                var code = faker.Random.Replace("??????????");
                var includeDriversLicense = faker.Random.Bool();
                Result<Customer> result = null;

                Entity customer = faker.Random.Bool()
                    ? new PersonFaker(true, includeAddress, includeDriversLicense, emailsCount, phonesCount).Generate()
                    : new BusinessFaker(true, includeAddress, false, emailsCount, phonesCount).Generate();

                if (customer.GetType() == typeof(Person))
                    result = Customer.Create((Person)customer, customerType, code);

                if (customer.GetType() == typeof(Business))
                    result = Customer.Create((Business)customer, customerType, code);

                var vehicles = new VehicleFaker(generateId).Generate(vehiclesCount);

                vehicles.ForEach(vehicles => result.Value.AddVehicle(vehicles));

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
