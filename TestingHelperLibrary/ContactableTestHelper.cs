using Menominee.Domain.Entities;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using TestingHelperLibrary.Fakers;
using Address = Menominee.Common.ValueObjects.Address;
using Email = Menominee.Domain.Entities.Email;
using Person = Menominee.Domain.Entities.Person;
using Phone = Menominee.Domain.Entities.Phone;

namespace TestingHelperLibrary
{
    public class ContactableTestHelper
    {
        public static Person CreatePerson()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var nameOrError = PersonName.Create(lastName, firstName);

            return Person.Create(nameOrError.Value, Gender.Female, "some notes").Value;
        }

        public static Person CreatePersonWithEmails(int emailCount)
        {
            var person = CreatePerson();
            var emails = CreateEmails(emailCount);

            foreach (var email in emails)
                person.AddEmail(Email.Create(email.Address, email.IsPrimary).Value);

            return person;
        }

        public static Person CreatePersonWithPhones(int phoneCount)
        {
            var person = CreatePerson();
            var phones = CreatePhones(phoneCount);

            foreach (var phone in phones)
                person.AddPhone(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            return person;
        }

        public static Person CreatePersonWithPhonesAndEmails()
        {
            var person = CreatePerson();
            var phones = CreatePhones(5);
            var emails = CreateEmails(5);

            foreach (var phone in phones)
                person.AddPhone(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            foreach (var email in emails)
                person.AddEmail(Email.Create(email.Address, email.IsPrimary).Value);

            return person;
        }

        public static Business CreateBusiness()
        {
            return Business.Create(BusinessName.Create(Utilities.RandomCharacters(10)).Value, "note").Value;
        }

        public static List<Business> CreateBusinesses(int count)
        {
            var names = CreateBusinessNames(count);
            return CreateBusinesses(names);
        }

        public static List<Business> CreateBusinesses(List<BusinessName> names)
        {
            var orgs = new List<Business>();

            foreach (var name in names)
                orgs.Add(CreateBusiness(name));

            return orgs;
        }

        public static Business CreateBusiness(BusinessName name)
        {
            return Business.Create(name, Utilities.LoremIpsum(100)).Value;
        }

        private static List<BusinessName> CreateBusinessNames(int count)
        {
            var names = new List<BusinessName>();

            for (int i = 0; i < count; i++)
                names.Add(BusinessName.Create(Utilities.LoremIpsum(10)).Value);

            return names;
        }

        public static IReadOnlyList<Phone> CreatePhones(int count)
        {
            var phones = new List<Phone>();

            for (int i = 0; i < count; i++)
                phones.Add(Phone.Create(
                    number: Utilities.RandomNumberSequence(10),
                    phoneType: PhoneType.Home,
                    isPrimary: false)
                    .Value);

            return phones;
        }

        public static IReadOnlyList<Email> CreateEmails(int count)
        {
            var emails = new List<Email>();

            for (int i = 0; i < count; i++)
            {
                emails.Add(Email.Create(
                    $"{Utilities.RandomNumberSequence(10)}@{Utilities.RandomNumberSequence(10)}.com",
                    isPrimary: false)
                    .Value);
            }

            return emails;
        }

        public static Address CreateAddress()
        {
            return new AddressFaker().Generate();
        }
    }
}
