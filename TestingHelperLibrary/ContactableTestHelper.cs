using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using TestingHelperLibrary.Fakers;
using Address = Menominee.Common.ValueObjects.Address;
using Email = CustomerVehicleManagement.Domain.Entities.Email;
using Person = CustomerVehicleManagement.Domain.Entities.Person;
using Phone = CustomerVehicleManagement.Domain.Entities.Phone;

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

        public static Organization CreateOrganization()
        {
            return Organization.Create(OrganizationName.Create(Utilities.RandomCharacters(10)).Value, "note").Value;
        }

        public static List<Organization> CreateOrganizations(int count)
        {
            var names = CreateOrganizationNames(count);
            return CreateOrganizations(names);
        }

        public static List<Organization> CreateOrganizations(List<OrganizationName> names)
        {
            var orgs = new List<Organization>();

            foreach (var name in names)
                orgs.Add(CreateOrganization(name));

            return orgs;
        }

        public static Organization CreateOrganization(OrganizationName name)
        {
            return Organization.Create(name, Utilities.LoremIpsum(100)).Value;
        }

        private static List<OrganizationName> CreateOrganizationNames(int count)
        {
            var names = new List<OrganizationName>();

            for (int i = 0; i < count; i++)
                names.Add(OrganizationName.Create(Utilities.LoremIpsum(10)).Value);

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
