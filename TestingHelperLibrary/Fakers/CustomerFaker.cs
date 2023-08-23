using Bogus;
using CSharpFunctionalExtensions;
using Menominee.Domain.Entities;
using Menominee.Common.Enums;
using Entity = Menominee.Common.Entity;
using Person = Menominee.Domain.Entities.Person;

namespace TestingHelperLibrary.Fakers
{
    public class CustomerFaker : Faker<Customer>
    {
        public CustomerFaker(bool generateId = false, bool includeAddress = false, int emailsCount = 0, int phonesCount = 0)
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

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
