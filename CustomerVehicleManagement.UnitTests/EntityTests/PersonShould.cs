using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class PersonShould
    {
        [Fact]
        public void Create_Person()
        {
            // Arrange
            var firstName = "Jane";
            var lastName = "Doe";

            // Act
            var name = PersonName.Create(lastName, firstName).Value;
            var person = new Person(name, Gender.Female);

            // Assert
            person.Should().NotBeNull();
        }

        [Fact]
        public void Throw_Exception_On_CreateWithNullName()
        {
            Action action = () => new Person(null, Gender.Female);

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Have_Empty_Phones_On_Create()
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
        public void Throw_Exception_On_Add_Greater_Than_One_PrimaryPhone()
        {
            var person = Helpers.CreateValidPerson();
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);
            person.AddPhone(phone);
            number = "444.627.9206";
            phone = new Phone(number, phoneType, true);

            Action action = () => person.AddPhone(phone);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void Throw_Exception_On_AddPhone_Duplicate()
        {
            var person = Helpers.CreateValidPerson();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);
            person.AddPhone(phone);
            phone = new Phone(number, phoneType, true);

            Action action = () => person.AddPhone(phone);

            action.Should().Throw<Exception>();
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
        public void Throw_Exception_On_SetEmails_Having_GreaterThan_One_PrimaryEmail()
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

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void Throw_Exception_On_SetEmails_With_Duplicates()
        {
            var person = Helpers.CreateValidPerson();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = new Email(address, false);

            emails.Add(email);
            emails.Add(email);

            Action action = () => person.SetEmails(emails);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void Throw_Exception_On_SetEmails_To_Null()
        {
            var person = Helpers.CreateValidPerson();
            List<Email> emails = null;

            Action action = () => person.SetEmails(emails);

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Throw_Exception_On_Add_GreaterThan_One_Primary_Email()
        {
            var person = Helpers.CreateValidPerson();
            var address = "jane@doe.com";
            var email = new Email(address, true);
            person.AddEmail(email);
            address = "june@done.com";
            email = new Email(address, true);

            Action action = () => person.AddEmail(email);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void Throw_Exception_On_AddEmail_Duplicate()
        {
            var person = Helpers.CreateValidPerson();
            var address = "jane@doe.com";
            var email = new Email(address, false);
            person.AddEmail(email);
            email = new Email(address, true);

            Action action = () => person.AddEmail(email);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void SetName()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;
            var person = new Person(name, Gender.Female);
            firstName = "Jill";
            lastName = "Done";
            name = PersonName.Create(lastName, firstName).Value;

            person.SetName(name);

            person.Name.FirstName.Should().Be(firstName);
            person.Name.LastName.Should().Be(lastName);
        }

        [Fact]
        public void Throw_Exception_On_SetName_With_Null_Parameter()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;
            var person = new Person(name, Gender.Female);
            
            Action action = () => person.SetName(null);

            action.Should().Throw<ArgumentException>();
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

            person.Birthday.Should().BeCloseTo(DateTime.Today.AddDays(10), 1.Minutes());
        }

        [Fact]
        public void SetDriversLicense()
        {
            var driversLicenseNumber = "123456789POIUYTREWQ";
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);
            var driversLicenseValidRange = DateTimeRange.Create(issued, expiry).Value;
            var driversLicenseOrError = DriversLicense.Create(driversLicenseNumber, State.MI, driversLicenseValidRange);
            var person = Helpers.CreateValidPerson();

            person.SetDriversLicense(driversLicenseOrError.Value);

            person.DriversLicense.Number.Should().Be(driversLicenseNumber);
        }

        [Fact]
        public void SetAddress()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";
            var person = Helpers.CreateValidPerson();
            var address = Address.Create(addressLine, city, state, postalCode);

            person.SetAddress(address.Value);

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
