using CustomerVehicleManagement.Domain.BaseClasses;
using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class PersonShould
    {
        [Fact]
        public void CreatePerson()
        {
            // Arrange
            var firstName = "Jane";
            var lastName = "Doe";

            // Act
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);

            // Assert
            person.Should().NotBeNull();
        }

        [Fact]
        public void NotCreatePersonWithNullName()
        {
            string firstName = null;
            string lastName = null;

            Action action = () => new Person(new PersonName(lastName, firstName), Gender.Female);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(PersonName.PersonNameEmptyMessage);
        }

        [Fact]
        public void NotCreatePersonWithEmptyName()
        {
            var firstName = "";
            var lastName = "";

            Action action = () => new Person(new PersonName(lastName, firstName), Gender.Female);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(PersonName.PersonNameEmptyMessage);
        }

        [Fact]
        public void CreatePersonWithEmptyLastName()
        {
            var firstName = "Jane";
            var lastName = "";

            var person = new Person(new PersonName(lastName, firstName), Gender.Female);

            person.Should().NotBeNull();
        }

        [Fact]
        public void CreatePersonWithEmptyFirstName()
        {
            var firstName = "";
            var lastName = "Doe";

            var person = new Person(new PersonName(lastName, firstName), Gender.Female);

            person.Should().NotBeNull();
        }

        [Fact]
        public void CreatePersonWithNullLastName()
        {
            var firstName = "Jane";
            string lastName = null;

            var person = new Person(new PersonName(lastName, firstName), Gender.Female);

            person.Should().NotBeNull();
        }

        [Fact]
        public void CreatePersonWithNullFirstName()
        {
            string firstName = null;
            var lastName = "Doe";

            var person = new Person(new PersonName(lastName, firstName), Gender.Female);

            person.Should().NotBeNull();
        }

        [Fact]
        public void HaveEmptyPhonesOnCreate()
        {
            var person = Helpers.CreateValidPerson();

            person.Phones.Count.Should().Be(0);
        }

        [Fact]
        public void AddPhone()
        {
            var person = Helpers.CreateValidPerson();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);

            person.AddPhone(phone);

            person.Phones.Should().Contain(phone);
        }

        [Fact]
        public void RemovePhone()
        {
            var person = Helpers.CreateValidPerson();
            var number = "555.444.3333";
            var phoneType = PhoneType.Mobile;
            var phone0 = new Phone(number, phoneType, true);
            person.AddPhone(phone0);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            var phone1 = new Phone(number, phoneType, false);
            person.AddPhone(phone1);

            person.Phones.Count.Should().Be(2);
            person.RemovePhone(phone0);

            person.Phones.Count.Should().Be(1);
            person.Phones.Should().Contain(phone1);
        }

        [Fact]
        public void SetPhones()
        {
            var person = Helpers.CreateValidPerson();
            var phones = new List<Phone>();
            var number = "555.444.3333";
            var phoneType = PhoneType.Mobile;
            var phone0 = new Phone(number, phoneType, true);
            phones.Add(phone0);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            var phone1 = new Phone(number, phoneType, false);
            phones.Add(phone1);
            person.SetPhones(phones);

            using (new AssertionScope())
            {
                person.Phones.Count.Should().Be(2);
                person.Phones.Should().Contain(phone0);
                person.Phones.Should().Contain(phone1);
            }
        }

        [Fact]
        public void NotCreateMoreThanOnePrimaryPhone()
        {
            var person = Helpers.CreateValidPerson();
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);
            person.AddPhone(phone);
            number = "444.627.9206";
            phone = new Phone(number, phoneType, true);

            Action action = () => person.AddPhone(phone);

            action.Should().Throw<InvalidOperationException>()
                           .WithMessage(Contactable.PrimaryPhoneExistsMessage);
        }

        [Fact]
        public void NotAddDuplicatePhone()
        {
            var person = Helpers.CreateValidPerson();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);
            person.AddPhone(phone);
            phone = new Phone(number, phoneType, true);

            Action action = () => person.AddPhone(phone);

            action.Should().Throw<InvalidOperationException>()
                           .WithMessage(Contactable.DuplicatePhoneExistsMessage);
        }

        [Fact]
        public void AddEmail()
        {
            var person = Helpers.CreateValidPerson();
            var address = "jane@doe.com";
            var email = new Email(address, true);

            person.AddEmail(email);

            person.Emails.Should().Contain(email);
        }

        [Fact]
        public void RemoveEmail()
        {
            var person = Helpers.CreateValidPerson();
            var address = "jane@doe.com";
            var email0 = new Email(address, true);
            person.AddEmail(email0);
            address = "june@doe.com";
            var email1 = new Email(address, false);
            person.AddEmail(email1);

            person.Emails.Count.Should().Be(2);
            person.RemoveEmail(email0);

            person.Emails.Count.Should().Be(1);
            person.Emails.Should().Contain(email1);
        }

        [Fact]
        public void SetEmails()
        {
            var person = Helpers.CreateValidPerson();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email0 = new Email(address, true);
            emails.Add(email0);
            address = "june@done.com";
            var email1 = new Email(address, false);
            emails.Add(email1);

            person.SetEmails(emails);

            person.Emails.Count.Should().Be(2);
            person.Emails.Should().Contain(email1);
        }

        [Fact]
        public void NotSetEmailsHavingMoreThanOnePrimaryEmail()
        {
            var person = Helpers.CreateValidPerson();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = new Email(address, true);
            emails.Add(email);
            address = "june@done.com";
            email = new Email(address, true);
            emails.Add(email);

            Action action = () => person.SetEmails(emails);

            action.Should().Throw<InvalidOperationException>()
                           .WithMessage(Contactable.PrimaryEmailExistsMessage);
        }

        [Fact]
        public void NotSetEmailsWithDuplicateEmails()
        {
            var person = Helpers.CreateValidPerson();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = new Email(address, false);

            emails.Add(email);
            emails.Add(email);

            Action action = () => person.SetEmails(emails);

            action.Should().Throw<InvalidOperationException>()
                           .WithMessage(Contactable.DuplicateEmailExistsMessage);
        }

        [Fact]
        public void NotSetEmailsToNull()
        {
            var person = Helpers.CreateValidPerson();
            List<Email> emails = null;

            Action action = () => person.SetEmails(emails);

            action.Should().Throw<ArgumentException>()
                           .WithMessage($"{Contactable.EmptyEmailCollectionMessage} (Parameter 'emails')");
        }

        [Fact]
        public void NotCreateMoreThanOnePrimaryEmail()
        {
            var person = Helpers.CreateValidPerson();
            var address = "jane@doe.com";
            var email = new Email(address, true);
            person.AddEmail(email);
            address = "june@done.com";
            email = new Email(address, true);

            Action action = () => person.AddEmail(email);

            action.Should().Throw<InvalidOperationException>()
                           .WithMessage(Contactable.PrimaryEmailExistsMessage);
        }

        [Fact]
        public void NotAddDuplicateEmail()
        {
            var person = Helpers.CreateValidPerson();
            var address = "jane@doe.com";
            var email = new Email(address, false);
            person.AddEmail(email);
            email = new Email(address, true);

            Action action = () => person.AddEmail(email);

            action.Should().Throw<InvalidOperationException>()
                           .WithMessage(Contactable.DuplicateEmailExistsMessage);
        }

        [Fact]
        public void SetName()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);
            firstName = "Jill";
            lastName = "Done";
            name = new PersonName(lastName, firstName);

            person.SetName(name);

            person.Name.FirstName.Should().Be(firstName);
            person.Name.LastName.Should().Be(lastName);
        }

        [Fact]
        public void SetGender()
        {
            var person = Helpers.CreateValidPerson();

            person.Gender.Should().Be(Gender.Female);
            person.SetGender(Gender.Male);

            person.Gender.Should().Be(Gender.Male);
        }

        [Fact]
        public void SetBirthday()
        {
            var person = Helpers.CreateValidPerson();

            person.SetBirthday(DateTime.Today.AddDays(10));

            person.Birthday.Should().BeCloseTo(DateTime.Today.AddDays(10));
        }

        [Fact]
        public void SetDriversLicense()
        {
            var driversLicenseNumber = "123456789POIUYTREWQ";
            var driversLicenseState = "MI";
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);
            var driversLicenseValidRange = new DateTimeRange(issued, expiry);
            var driversLicense = new DriversLicense(driversLicenseNumber, driversLicenseState, driversLicenseValidRange);
            var person = Helpers.CreateValidPerson();

            person.SetDriversLicense(driversLicense);

            person.DriversLicense.Number.Should().Be(driversLicenseNumber);
        }

        [Fact]
        public void SetAddress()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";
            var person = Helpers.CreateValidPerson();
            var address = new Address(addressLine, city, state, postalCode);

            person.SetAddress(address);

            using (new AssertionScope())
            {
                person.Address.AddressLine.Should().Be(addressLine);
                person.Address.City.Should().Be(city);
                person.Address.State.Should().Be(state);
                person.Address.PostalCode.Should().Be(postalCode);
            }
        }
    }
}
