using Bogus;
using Menominee.Domain.Entities;

namespace TestingHelperLibrary.Fakers
{
    public class EmailFaker : Faker<Email>
    {
        private EmailFaker()
        {
            // Common rules can go here
        }

        public EmailFaker(bool generateId = false) : this()
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);
            GenerateEmail();
        }

        public EmailFaker(long id = 0) : this()
        {
            RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);
            GenerateEmail();
        }

        private void GenerateEmail()
        {
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
