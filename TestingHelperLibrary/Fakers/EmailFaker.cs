using Bogus;
using CustomerVehicleManagement.Domain.Entities;

namespace TestingHelperLibrary.Fakers
{
    public class EmailFaker : Faker<Email>
    {
        public EmailFaker(bool generateId)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var emailAddress = faker.Internet.Email();
                var isPrimary = false;
                var result = Email.Create(emailAddress, isPrimary);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
