using Bogus;
using Menominee.Domain.Entities;
using Menominee.Domain.ValueObjects;
using Person = Menominee.Domain.Entities.Person;

namespace TestingHelperLibrary.Fakers
{
    public class PersonFaker : Faker<Person>
    {
        public PersonFaker(bool generateId, bool includeAddress = false, bool includeDriversLicense = false, int emailsCount = 0, int phonesCount = 0, long id = 0)
        {
            ConfigureIdRules(generateId, id);
            CustomInstantiator(faker => GeneratePerson(faker, generateId, includeAddress, includeDriversLicense, emailsCount, phonesCount));
        }

        private void ConfigureIdRules(bool generateId, long id)
        {
            var rule = DetermineIdRule(generateId, id);
            RuleFor(entity => entity.Id, rule);
        }

        private static Func<Faker, long> DetermineIdRule(bool generateId, long id)
        {
            return faker => generateId
                ? faker.Random.Long(1, 10000)
                : id > 0
                    ? id
                    : 0;
        }

        private static Person GeneratePerson(Faker faker, bool generateId, bool includeAddress, bool includeDriversLicense, int emailsCount, int phonesCount)
        {
            var name = PersonName.Create(faker.Person.LastName, faker.Person.FirstName).Value;
            var notes = faker.Lorem.Sentence(20);
            var birthday = faker.Person.DateOfBirth;

            var address = includeAddress ? GenerateAddress() : null;
            var driversLicense = includeDriversLicense ? GenerateDriversLicense() : null;
            var emails = GenerateEmails(generateId, emailsCount);
            var phones = GeneratePhones(generateId, phonesCount);

            var result = Person.Create(name, notes, birthday, emails, phones, address, driversLicense);

            return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
        }

        private static Address GenerateAddress()
        {
            return new AddressFaker().Generate();
        }

        private static DriversLicense GenerateDriversLicense()
        {
            return new DriversLicenseFaker().Generate();
        }

        private static List<Email>? GenerateEmails(bool generateId, int emailsCount)
        {
            return emailsCount <= 0
                ? null
                : generateId
                    ? Utilities.GenerateRandomUniqueLongValues(emailsCount)
                        .Select(id => new EmailFaker(id).Generate())
                        .ToList()
                    : new EmailFaker(generateId: false).Generate(emailsCount);
        }

        private static List<Phone>? GeneratePhones(bool generateId, int phonesCount)
        {
            return phonesCount <= 0
                ? null
                : generateId
                    ? Utilities.GenerateRandomUniqueLongValues(phonesCount)
                        .Select(id => new PhoneFaker(id).Generate())
                        .ToList()
                    : new PhoneFaker(generateId: false).Generate(phonesCount);
        }
    }
}
