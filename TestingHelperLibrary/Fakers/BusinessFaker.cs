using Bogus;
using Menominee.Domain.Entities;
using Menominee.Domain.ValueObjects;
using Business = Menominee.Domain.Entities.Business;
using Person = Menominee.Domain.Entities.Person;

namespace TestingHelperLibrary.Fakers
{
    public class BusinessFaker : Faker<Business>
    {
        public BusinessFaker(bool generateId, bool includeAddress = false, bool includeContact = false, int emailsCount = 0, int phonesCount = 0)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker => GenerateBusiness(faker, generateId, includeAddress, includeContact, emailsCount, phonesCount));
        }

        private Business GenerateBusiness(Faker faker, bool generateId, bool includeAddress, bool includeContact, int emailsCount, int phonesCount)
        {
            var name = BusinessName.Create(faker.Company.CompanyName()).Value;
            var notes = faker.Lorem.Sentence(20);

            var address = includeAddress ? GenerateAddress() : null;
            var contact = includeContact ? GenerateContact(generateId, includeAddress, emailsCount, phonesCount) : null;
            var emails = GenerateEmails(generateId, emailsCount);
            var phones = GeneratePhones(generateId, phonesCount);

            var result = Business.Create(name, notes, contact, address, emails, phones);

            return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
        }

        private Address GenerateAddress()
        {
            return new AddressFaker().Generate();
        }

        private Person GenerateContact(bool generateId, bool includeAddress, int emailsCount, int phonesCount)
        {
            return new PersonFaker(generateId, includeAddress, false, emailsCount, phonesCount).Generate();
        }

        private List<Email>? GenerateEmails(bool generateId, int emailsCount)
        {
            return emailsCount <= 0
                ? null
                : generateId
                    ? Utilities.GenerateRandomUniqueLongValues(emailsCount)
                        .Select(id => new EmailFaker(id).Generate())
                        .ToList()
                    : new EmailFaker(generateId: false).Generate(emailsCount);
        }

        private List<Phone>? GeneratePhones(bool generateId, int phonesCount)
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
