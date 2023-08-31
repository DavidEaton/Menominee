using Bogus;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Person = Menominee.Domain.Entities.Person;

namespace TestingHelperLibrary.Fakers
{
    public class PersonFaker : Faker<Person>
    {
        public PersonFaker(bool generateId, bool includeAddress = false, bool includeDriversLicense = false, int emailsCount = 0, int phonesCount = 0, long id = 0)
        {
            if (generateId)
                RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            if (id > 0)
                RuleFor(entity => entity.Id, faker => id > 0 ? id : 0);

            CustomInstantiator(faker =>
            {
                var firstName = faker.Person.FirstName;
                var lastName = faker.Person.LastName;
                var notes = faker.Lorem.Sentence(20);
                var gender = faker.PickRandom<Gender>();
                var birthday = faker.Person.DateOfBirth;
                var name = PersonName.Create(lastName, firstName).Value;

                var driversLicense = includeDriversLicense ? new DriversLicenseFaker().Generate() : null;
                var address = includeAddress ? new AddressFaker().Generate() : null;

                var emails = emailsCount <= 0
                    ? null
                    : generateId
                        ? Utilities.GenerateRandomUniqueLongValues(emailsCount)
                            .Select(id => new EmailFaker(id).Generate())
                            .ToList()
                        : new EmailFaker(generateId: false).Generate(emailsCount);

                var phones = phonesCount <= 0
                    ? null
                    : generateId
                        ? Utilities.GenerateRandomUniqueLongValues(phonesCount)
                            .Select(id => new PhoneFaker(id).Generate())
                            .ToList()
                        : new PhoneFaker(generateId: false).Generate(phonesCount);

                var result = Person.Create(name, gender, notes, birthday, emails, phones, address, driversLicense);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
