using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.TestUtilities;
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
            var personOrError = Person.Create(name, Gender.Female);

            // Assert
            personOrError.Value.Should().BeOfType<Person>();
            personOrError.IsFailure.Should().BeFalse();

        }

        [Fact]
        public void Return_Failure_On_Create_With_Null_Name()
        {
            var person = Person.Create(null, Gender.Female);

            person.IsFailure.Should().BeTrue();
            person.Error.Should().NotBeNull();
        }

        [Fact]
        public void Have_Empty_Phones_On_Create()
        {
            var person = Utilities.CreatePerson();

            person.Phones.Count.Should().Be(0);
        }

        [Fact]
        public void AddPhone()
        {
            var person = Utilities.CreatePerson();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            person.AddPhone(phone);

            person.Phones.Should().Contain(phone);
        }

        [Fact]
        public void RemovePhone()
        {
            var person = Utilities.CreatePerson();
            var number = "555.444.3333";
            var phoneType = PhoneType.Mobile;
            var phone0 = Phone.Create(number, phoneType, true).Value;
            person.AddPhone(phone0);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            var phone1 = Phone.Create(number, phoneType, false).Value;
            person.AddPhone(phone1);

            person.Phones.Count.Should().Be(2);
            person.RemovePhone(phone0);

            person.Phones.Count.Should().Be(1);
            person.Phones.Should().Contain(phone1);
        }

        [Fact]
        public void SetPhones()
        {
            var person = Utilities.CreatePerson();
            var phones = new List<Phone>();
            var number = "555.444.3333";
            var phoneType = PhoneType.Mobile;
            var phone0 = Phone.Create(number, phoneType, true).Value;
            phones.Add(phone0);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            var phone1 = Phone.Create(number, phoneType, false).Value;
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
            var person = Utilities.CreatePerson();
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            person.AddPhone(phone);
            number = "444.627.9206";
            phone = Phone.Create(number, phoneType, true).Value;

            Action action = () => person.AddPhone(phone);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void Throw_Exception_On_AddPhone_Duplicate()
        {
            var person = Utilities.CreatePerson();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            person.AddPhone(phone);
            phone = Phone.Create(number, phoneType, true).Value;

            Action action = () => person.AddPhone(phone);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void AddEmail()
        {
            var person = Utilities.CreatePerson();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            person.AddEmail(email);

            person.Emails.Should().Contain(email);
        }

        [Fact]
        public void RemoveEmail()
        {
            var person = Utilities.CreatePerson();
            var address = "jane@doe.com";
            var email0 = Email.Create(address, true).Value;
            person.AddEmail(email0);
            address = "june@doe.com";
            var email1 = Email.Create(address, false).Value;
            person.AddEmail(email1);

            person.Emails.Count.Should().Be(2);
            person.RemoveEmail(email0);

            person.Emails.Count.Should().Be(1);
            person.Emails.Should().Contain(email1);
        }

        [Fact]
        public void SetEmails()
        {
            var person = Utilities.CreatePerson();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email0 = Email.Create(address, true).Value;
            emails.Add(email0);
            address = "june@done.com";
            var email1 = Email.Create(address, false).Value;
            emails.Add(email1);

            person.SetEmails(emails);

            person.Emails.Count.Should().Be(2);
            person.Emails.Should().Contain(email1);
        }

        [Fact]
        public void Throw_Exception_On_SetEmails_Having_GreaterThan_One_PrimaryEmail()
        {
            var person = Utilities.CreatePerson();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            emails.Add(email);
            address = "june@done.com";
            email = Email.Create(address, true).Value;
            emails.Add(email);

            Action action = () => person.SetEmails(emails);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void Throw_Exception_On_SetEmails_With_Duplicates()
        {
            var person = Utilities.CreatePerson();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            emails.Add(email);
            emails.Add(email);

            Action action = () => person.SetEmails(emails);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void SetEmails_To_EmptyList()
        {
            var person = Utilities.CreatePersonWithEmails();
            List<Email> emails = new();

            person.Emails.Count.Should().NotBe(0);
            person.SetEmails(emails);

            person.Emails.Count.Should().Be(0);
        }

        [Fact]
        public void Throw_Exception_On_Add_GreaterThan_One_Primary_Email()
        {
            var person = Utilities.CreatePerson();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            person.AddEmail(email);
            address = "june@done.com";
            email = Email.Create(address, true).Value;

            Action action = () => person.AddEmail(email);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void Throw_Exception_On_AddEmail_Duplicate()
        {
            var person = Utilities.CreatePerson();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            person.AddEmail(email);
            email = Email.Create(address, true).Value;

            Action action = () => person.AddEmail(email);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void SetName()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;
            var person = Person.Create(name, Gender.Female).Value;
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
            var person = Person.Create(name, Gender.Female).Value;

            Action action = () => person.SetName(null);

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void SetGender()
        {
            var person = Utilities.CreatePerson();

            person.Gender.Should().Be(Gender.Female);
            person.SetGender(Gender.Male);

            person.Gender.Should().Be(Gender.Male);
        }

        [Fact]
        public void SetBirthday()
        {
            var person = Utilities.CreatePerson();

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
            var person = Utilities.CreatePerson();

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
            var person = Utilities.CreatePerson();
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
