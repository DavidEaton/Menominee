using Bogus;
using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Domain.Entities;
using Menominee.TestingHelperLibrary.Fakers;
using Entity = Menominee.Common.Entity;
using Person = Menominee.Domain.Entities.Person;

namespace TestingHelperLibrary.Fakers
{
    public class CustomerFaker : Faker<Customer>
    {
        public CustomerFaker(
            bool generateId = false,
            bool includeAddress = false,
            bool includeContact = false,
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

                var customer = GenerateRandomEntity(
                    faker,
                    includeAddress,
                    includeContact,
                    includeDriversLicense,
                    emailsCount,
                    phonesCount);

                var result = CreateCustomer(customer, customerType, code);

                AddVehiclesToCustomer(result, vehiclesCount, generateId);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }

        private static Entity GenerateRandomEntity(
            Faker faker,
            bool includeAddress,
            bool includeContact,
            bool includeDriversLicense,
            int emailsCount,
            int phonesCount)
        {
            return faker.Random.Bool()
                ? new PersonFaker(true, includeAddress, includeDriversLicense, emailsCount, phonesCount).Generate()
                : new BusinessFaker(true, includeAddress, includeContact, emailsCount, phonesCount).Generate();
        }

        private static Result<Customer> CreateCustomer(Entity customer, CustomerType customerType, string code)
        {
            return customer switch
            {
                Person person => Customer.Create(person, customerType, code),
                Business business => Customer.Create(business, customerType, code),
                _ => throw new InvalidOperationException("Unsupported customer entity type.")
            };
        }

        private static void AddVehiclesToCustomer(Result<Customer> result, int vehiclesCount, bool generateId)
        {
            var vehicles = new VehicleFaker(generateId).Generate(vehiclesCount);
            vehicles.ForEach(vehicle => result.Value.AddVehicle(vehicle));
        }
    }
}
