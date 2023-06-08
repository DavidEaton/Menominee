using Bogus;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Person = CustomerVehicleManagement.Domain.Entities.Person;

namespace TestingHelperLibrary.Fakers
{
    public class PersonFaker : Faker<Person>
    {
        public PersonFaker(bool generateId, bool includeAddress = false, bool includeDriversLicense = false, int emailsCount = 0, int phonesCount = 0)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var firstName = faker.Person.FirstName;
                var lastName = faker.Person.LastName;
                var notes = faker.Lorem.Sentence(20);
                var gender = faker.PickRandom<Gender>();
                var birthday = faker.Person.DateOfBirth;
                var name = PersonName.Create(lastName, firstName).Value;

                Address? address = null;
                DriversLicense? driversLicense = null;

                if (includeAddress)
                    address = new AddressFaker().Generate();

                if (includeDriversLicense)
                    driversLicense = new DriversLicenseFaker().Generate();

                var emails = new EmailFaker(generateId).Generate(emailsCount);
                var phones = new PhoneFaker(generateId).Generate(phonesCount);

                var result = Person.Create(name, gender, notes, birthday, emails, phones, address, driversLicense);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
