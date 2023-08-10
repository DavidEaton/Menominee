using Bogus;
using Menominee.Common.ValueObjects;
using Business = Menominee.Domain.Entities.Business;
using Person = Menominee.Domain.Entities.Person;

namespace TestingHelperLibrary.Fakers
{
    public class BusinessFaker : Faker<Business>
    {
        public BusinessFaker(bool generateId, bool includeAddress = false, bool includeContact = false, int emailsCount = 0, int phonesCount = 0)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var name = BusinessName.Create(faker.Company.CompanyName()).Value;
                var notes = faker.Lorem.Sentence(20);
                var birthday = faker.Person.DateOfBirth;

                Address? address = null;
                Person? contact = null;

                if (includeAddress)
                {
                    address = new AddressFaker().Generate();
                }

                if (includeContact)
                {
                    contact = new PersonFaker(true, includeAddress, false, emailsCount, phonesCount).Generate();
                }

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

                var result = Business.Create(name, notes, contact, address, emails, phones);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
