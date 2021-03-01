using CustomerVehicleManagement.Domain.Entities;
using NUnit.Framework;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Tests.EntityTests
{
    public class PersonShould
    {
        [Test]
        public void CreatePerson()
        {
            // Arrange
            var firstName = "Jane";
            var lastName = "Doe";

            // Act
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);

            // Assert
            Assert.That(person, Is.Not.Null);
        }

        [Test]
        public void NotCreatePersonWithNullName()
        {
            string firstName = null;
            string lastName = null;

            var exception = Assert.Throws<ArgumentException>(
                () => { new Person(new PersonName(lastName, firstName), Gender.Female); });

            Assert.That(exception.Message, Is.EqualTo(PersonName.PersonNameEmptyMessage));
        }

        [Test]
        public void NotCreatePersonWithEmptyName()
        {
            string firstName = "";
            string lastName = "";

            var exception = Assert.Throws<ArgumentException>(
                () => { new Person(new PersonName(lastName, firstName), Gender.Female); });

            Assert.That(exception.Message, Is.EqualTo(PersonName.PersonNameEmptyMessage));
        }

        [Test]
        public void CreatePersonWithEmptyLastName()
        {
            string firstName = "Jane";
            string lastName = "";

            var person = new Person(new PersonName(lastName, firstName), Gender.Female);

            Assert.That(person, Is.Not.Null);
        }

        [Test]
        public void CreatePersonWithEmptyFirstName()
        {
            string firstName = "";
            string lastName = "Doe";

            var person = new Person(new PersonName(lastName, firstName), Gender.Female);

            Assert.That(person, Is.Not.Null);
        }

        [Test]
        public void CreatePersonWithNullLastName()
        {
            string firstName = "Jane";
            string lastName = null;

            var person = new Person(new PersonName(lastName, firstName), Gender.Female);

            Assert.That(person, Is.Not.Null);
        }

        [Test]
        public void CreatePersonWithNullFirstName()
        {
            string firstName = null;
            string lastName = "Doe";

            var person = new Person(new PersonName(lastName, firstName), Gender.Female);

            Assert.That(person, Is.Not.Null);
        }

        [Test]
        public void CreatePersonWithBirthday()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = new PersonName(lastName, firstName);
            var birthday = DateTime.Today.AddYears(-40);

            var person = new Person(name, Gender.Female, birthday);

            Assert.That(person, Is.Not.Null);
            Assert.That(person.Birthday, Is.EqualTo(birthday));
        }

        [Test]
        public void CreatePersonWithDriversLicense()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var driversLicenseNumber = "123456789POIUYTREWQ";
            var driversLicenseState = "MI";
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);
            var driversLicenseValidRange = new DateTimeRange(issued, expiry);

            var driversLicense = new DriversLicense(driversLicenseNumber, driversLicenseState, driversLicenseValidRange);
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female, null, null, null, null, driversLicense);

            Assert.That(person, Is.Not.Null);
            Assert.That(person.DriversLicense, Is.Not.Null);
            Assert.That(person.DriversLicense.Number, Is.EqualTo(driversLicenseNumber));
            Assert.That(person.DriversLicense.State, Is.EqualTo(driversLicenseState));
            Assert.That(person.DriversLicense.ValidRange.Start, Is.EqualTo(issued));
            Assert.That(person.DriversLicense.ValidRange.End, Is.EqualTo(expiry));
        }

        [Test]
        public void CreatePersonWithAddress()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";

            var address = new Address(addressLine, city, state, postalCode);

            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female, null, address);

            Assert.That(person, Is.Not.Null);
            Assert.That(person.Address, Is.Not.Null);
            Assert.That(person.Address.AddressLine, Is.EqualTo(addressLine));
            Assert.That(person.Address.City, Is.EqualTo(city));
            Assert.That(person.Address.State, Is.EqualTo(state));
            Assert.That(person.Address.PostalCode, Is.EqualTo(postalCode));
        }

        [Test]
        public void CreatePersonWithPhones()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = new PersonName(lastName, firstName);
            var phones = new List<Phone>();
            var number = "555.444.3333";
            var phoneType = PhoneType.Mobile;
            var phone = new Phone(number, phoneType, true);
            phones.Add(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = new Phone(number, phoneType, false);
            phones.Add(phone);

            var person = new Person(name, Gender.Female, null, null, phones);

            Assert.That(person.Phones.Count == 2);
        }

        [Test]
        public void CreatePersonWithEmails()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = new PersonName(lastName, firstName);
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = new Email(address, true);
            emails.Add(email);
            address = "june@done.com";
            email = new Email(address, false);
            emails.Add(email);

            var person = new Person(name, Gender.Female, null, null, null, emails);

            Assert.That(person.Emails.Count == 2);

        }

        [Test]
        public void HaveEmptyPhonesOnCreate()
        {
            var person = CreateValidPerson();

            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);


            Assert.That(person.Phones, Is.Not.EqualTo(null));
            Assert.That(person.Phones.Count, Is.EqualTo(0));
            person.AddPhone(phone);

            Assert.That(person.Phones.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddPhone()
        {
            var person = CreateValidPerson();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);

            person.AddPhone(phone);

            Assert.That(person.Phones[0].Number == number);
        }

        [Test]
        public void RemovePhone()
        {
            var person = CreateValidPerson();
            var number = "555.444.3333";
            var phoneType = PhoneType.Mobile;
            var phone = new Phone(number, phoneType, true);
            person.AddPhone(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = new Phone(number, phoneType, false);
            person.AddPhone(phone);

            Assert.That(person.Phones.Count == 2);
            person.RemovePhone(phone);

            Assert.That(person.Phones.Count == 1);
            Assert.That(person.Phones[0].Number == "555.444.3333");
        }

        [Test]
        public void SetPhones()
        {
            var person = CreateValidPerson();
            var phones = new List<Phone>();
            var number = "555.444.3333";
            var phoneType = PhoneType.Mobile;
            var phone = new Phone(number, phoneType, true);

            phones.Add(phone);

            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = new Phone(number, phoneType, false);

            phones.Add(phone);

            person.SetPhones(phones);

            Assert.That(person.Phones.Count == 2);
            Assert.That(person.Phones[0].Number == "555.444.3333");

            var newPhones = new List<Phone>();
            number = "123.456.7890";
            phoneType = PhoneType.Home;
            phone = new Phone(number, phoneType, true);

            newPhones.Add(phone);

            number = "987.654.3210";
            phoneType = PhoneType.Home;
            phone = new Phone(number, phoneType, false);

            newPhones.Add(phone);

            person.SetPhones(newPhones);

            Assert.AreEqual(person.Phones.Count, 2);
            Assert.That(person.Phones[1].Number == "987.654.3210");
        }

        [Test]
        public void NotCreateMoreThanOnePrimaryPhone()
        {
            var person = CreateValidPerson();
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);
            person.AddPhone(phone);
            number = "444.627.9206";
            phone = new Phone(number, phoneType, true);

            var exception = Assert.Throws<ArgumentException>(
                () => { person.AddPhone(phone); });

            Assert.That(exception.Message, Is.EqualTo(Person.PrimaryPhoneExistsMessage));
        }

        [Test]
        public void NotAddDuplicatePhone()
        {
            var person = CreateValidPerson();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);

            person.AddPhone(phone);
            phone = new Phone(number, phoneType, true);

            var exception = Assert.Throws<ArgumentException>(
                () => { person.AddPhone(phone); });

            Assert.That(exception.Message, Is.EqualTo(Person.DuplicatePhoneExistsMessage));
        }

        [Test]
        public void AddEmail()
        {
            var person = CreateValidPerson();
            var address = "jane@doe.com";
            var email = new Email(address, true);

            person.AddEmail(email);

            Assert.That(person.Emails[0].Address == address);
        }

        [Test]
        public void RemoveEmail()
        {
            var person = CreateValidPerson();
            var address = "jane@doe.com";
            var email = new Email(address, true);

            person.AddEmail(email);

            address = "june@doe.com";
            email = new Email(address, false);
            person.AddEmail(email);

            Assert.That(person.Emails.Count == 2);

            person.RemoveEmail(email);

            Assert.That(person.Emails.Count == 1);
            Assert.That(person.Emails[0].Address == "jane@doe.com");
        }

        [Test]
        public void SetEmails()
        {
            var person = CreateValidPerson();
            var emails = new List<Email>();

            var address = "jane@doe.com";
            var email = new Email(address, true);

            emails.Add(email);

            address = "june@done.com";
            email = new Email(address, false);

            emails.Add(email);

            person.SetEmails(emails);

            Assert.That(person.Emails.Count == 2);
            Assert.That(person.Emails[0].Address == "jane@doe.com");

            var newEmails = new List<Email>();
            address = "jill@hill.com";
            email = new Email(address, true);

            newEmails.Add(email);

            address = "jack@hill.com";
            email = new Email(address, false);

            newEmails.Add(email);

            person.SetEmails(newEmails);

            Assert.AreEqual(person.Emails.Count, 2);
            Assert.That(person.Emails[1].Address == "jack@hill.com");
        }

        [Test]
        public void NotSetEmailsHavingMoreThanOnePrimaryEmail()
        {
            var person = CreateValidPerson();
            var emails = new List<Email>();

            var address = "jane@doe.com";
            var email = new Email(address, true);

            emails.Add(email);

            address = "june@done.com";
            email = new Email(address, true);

            emails.Add(email);

            var exception = Assert.Throws<ArgumentException>(
                () => { person.SetEmails(emails); });

            Assert.That(exception.Message, Is.EqualTo(Person.PrimaryEmailExistsMessage));


        }

        [Test]
        public void NotSetEmailsWithDuplicateEmails()
        {
            var person = CreateValidPerson();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = new Email(address, false);

            emails.Add(email);
            emails.Add(email);

            var exception = Assert.Throws<ArgumentException>(
                () => { person.SetEmails(emails); });

            Assert.That(exception.Message, Is.EqualTo(Person.DuplicateEmailExistsMessage));
        }

        [Test]
        public void NotSetEmailsToNull()
        {
            var person = CreateValidPerson();
            List<Email> emails = null;

            var exception = Assert.Throws<ArgumentException>(
                () => { person.SetEmails(emails); });

            Assert.That(exception.Message, Is.EqualTo(Person.EmptyEmailCollectionMessage));
        }

        [Test]
        public void NotCreateMoreThanOnePrimaryEmail()
        {
            var person = CreateValidPerson();
            var address = "jane@doe.com";
            var email = new Email(address, true);
            person.AddEmail(email);
            address = "june@done.com";
            email = new Email(address, true);

            var exception = Assert.Throws<ArgumentException>(
                () => { person.AddEmail(email); });

            Assert.That(exception.Message, Is.EqualTo(Person.PrimaryEmailExistsMessage));

        }

        [Test]
        public void NotAddDuplicateEmail()
        {
            var person = CreateValidPerson();
            var address = "jane@doe.com";
            var email = new Email(address, false);

            person.AddEmail(email);
            email = new Email(address, true);

            var exception = Assert.Throws<ArgumentException>(
                () => { person.AddEmail(email); });

            Assert.That(exception.Message, Is.EqualTo(Person.DuplicateEmailExistsMessage));
        }

        [Test]
        public void SetName()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);

            Assert.That(person.Name.FirstName == firstName);
            Assert.That(person.Name.LastName == lastName);

            firstName = "Jill";
            lastName = "Done";

            name = new PersonName(lastName, firstName);

            person.SetName(name);

            Assert.That(person.Name.FirstName == firstName);
            Assert.That(person.Name.LastName == lastName);
        }

        [Test]
        public void SetGender()
        {
            var person = CreateValidPerson();

            Assert.That(person.Gender == Gender.Female);

            person.SetGender(Gender.Male);

            Assert.That(person.Gender == Gender.Male);
        }

        [Test]
        public void SetBirthday()
        {
            var person = CreateValidPerson();

            person.SetBirthday(DateTime.Today.AddDays(10));

            Assert.That(person.Birthday == DateTime.Today.AddDays(10));
        }

        [Test]
        public void SetDriversLicense()
        {
            var driversLicenseNumber = "123456789POIUYTREWQ";
            var driversLicenseState = "MI";
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);
            var driversLicenseValidRange = new DateTimeRange(issued, expiry);
            var driversLicense = new DriversLicense(driversLicenseNumber, driversLicenseState, driversLicenseValidRange);
            var person = CreateValidPerson();

            person.SetDriversLicense(driversLicense);

            Assert.That(person.DriversLicense.Number, Is.EqualTo(driversLicenseNumber));
        }

        [Test]
        public void SetAddress()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";
            var person = CreateValidPerson();
            var address = new Address(addressLine, city, state, postalCode);

            person.SetAddress(address);

            Assert.That(person.Address.AddressLine, Is.EqualTo(addressLine));
            Assert.That(person.Address.City, Is.EqualTo(city));
            Assert.That(person.Address.State, Is.EqualTo(state));
            Assert.That(person.Address.PostalCode, Is.EqualTo(postalCode));
        }

        private Person CreateValidPerson()
        {
            var firstName = "Jane";
            var lastName = "Doe";

            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);

            return person;
        }

    }
}
