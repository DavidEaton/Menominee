using CustomerVehicleManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.IntegrationTests.Helpers
{
    public static class Utilities
    {
        public static void InitializeDbForTests(AppDbContext db)
        {
            db.Persons.AddRange(GetSeedingPersons());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(AppDbContext db)
        {
            db.Persons.RemoveRange(db.Persons);
            InitializeDbForTests(db);
        }

        public static List<Person> GetSeedingPersons()
        {
            return new List<Person>()
            {
                new Person(new PersonName("Smith", "Jane"), Gender.Female),
                new Person(new PersonName("Jones", "Latisha"), Gender.Female),
                new Person(new PersonName("Lee", "Wong"), Gender.Male),
                new Person(new PersonName("Kelly", "Junice"), Gender.Female),
            };
        }

        public static DbContextOptions<AppDbContext> CreateDbContextOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"testdb{Guid.NewGuid()}")
                .Options;
        }
        internal static Organization CreateValidOrganization()
        {
            var name = "jane's";
            Organization organization = null;
            var organizationNameOrError = OrganizationName.Create(name);

            if (organizationNameOrError.IsSuccess)
                organization = new Organization(organizationNameOrError.Value);

            return organization;
        }

        public static Person CreateValidPerson()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);

            return person;
        }

        public static Person CreateValidPersonWithPhones()
        {
            var person = CreateValidPerson();
            person.SetPhones(CreateValidPhones());
            return person;
        }

        public static Person CreateValidPersonWithEmails()
        {
            var person = CreateValidPerson();
            person.SetEmails(CreateValidEmails());
            return person;
        }

        public static void CreateAndSavePersonGraph(DbContextOptions<AppDbContext> options, out Person person, out int id)
        {
            // Create a new Person with Emails and Phones, and save
            using (var context = new AppDbContext(options))
            {
                person = CreateValidPersonWithEmails();
                person.SetPhones(CreateValidPhones());
                context.Persons.Add(person);
                context.SaveChanges();
                id = person.Id;
            }
        }
        public static IList<Phone> CreateValidPhones()
        {
            IList<Phone> phones = new List<Phone>();

            var number1 = "(555) 987-6543";
            var phoneType1 = PhoneType.Mobile;
            var isPrimary1 = true;
            var phone1 = new Phone(number1, phoneType1, isPrimary1);

            var number2 = "(555) 123-4567";
            var phoneType2 = PhoneType.Mobile;
            var isPrimary2 = false;
            var phone2 = new Phone(number2, phoneType2, isPrimary2);

            phones.Add(phone1);
            phones.Add(phone2);

            return phones;
        }

        public static IList<Email> CreateValidEmails()
        {
            IList<Email> Emails = new List<Email>();

            var address1 = "a@b.c";
            var isPrimary1 = true;
            var Email1 = new Email(address1, isPrimary1);

            var address2 = "d@e.f";
            var isPrimary2 = false;
            var Email2 = new Email(address2, isPrimary2);

            Emails.Add(Email1);
            Emails.Add(Email2);

            return Emails;
        }

        public static Address CreateValidAddress()
        {
            string addressLine = "1234 Fifth Ave.";
            string city = "Traverse City";
            string state = "MI";
            string postalCode = "49686";
            return new Address(addressLine, city, state, postalCode);
        }
    }
}
