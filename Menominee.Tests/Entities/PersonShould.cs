using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using Menominee.Domain.BaseClasses;
using Menominee.Domain.Entities;
using Menominee.Domain.Enums;
using Menominee.Domain.ValueObjects;
using System;
using TestingHelperLibrary;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class PersonShould
    {
        [Fact]
        public void Create_Person()
        {
            // Arrange
            var firstName = "Jane";
            var lastName = "Doe";
            var notes = Utilities.LoremIpsum(100);

            // Act
            var name = PersonName.Create(lastName, firstName).Value;
            var resultOrError = Person.Create(name, notes);

            // Assert
            resultOrError.Value.Should().BeOfType<Person>();
            resultOrError.IsFailure.Should().BeFalse();

        }

        [Fact]
        public void Create_Person_With_Birthday()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;
            var notes = Utilities.LoremIpsum(100);
            var birthday = DateTime.Today.AddYears(-50);

            var resultOrError = Person.Create(name, notes, birthday);

            resultOrError.Value.Should().BeOfType<Person>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Create_Person_With_Address()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;
            var notes = Utilities.LoremIpsum(100);
            var address = ContactableTestHelper.CreateAddress();

            var resultOrError = Person.Create(name, notes, address: address);

            resultOrError.Value.Should().BeOfType<Person>();
            resultOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Return_Failure_On_Create_With_Null_Name()
        {
            var notes = Utilities.LoremIpsum(100);
            var person = Person.Create(null, notes);

            person.IsFailure.Should().BeTrue();
            person.Error.Should().Be(Contactable.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_With_Invalid_Birthday()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var notes = Utilities.LoremIpsum(100);
            var name = PersonName.Create(lastName, firstName).Value;
            DateTime? invalidBirthday = DateTime.MinValue;

            var personOrError = Person.Create(name, notes, invalidBirthday);

            personOrError.IsFailure.Should().BeTrue();
            personOrError.Error.Should().NotBeNull();

            invalidBirthday = DateTime.MaxValue;
            personOrError = Person.Create(name, notes, invalidBirthday);

            personOrError.IsFailure.Should().BeTrue();
            personOrError.Error.Should().NotBeNull();

        }

        [Fact]
        public void Return_Failure_On_Create_With_Invalid_Name()
        {
            PersonName name = null;
            var notes = Utilities.LoremIpsum(100);

            var personOrError = Person.Create(name, notes);

            personOrError.IsFailure.Should().BeTrue();
            personOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Have_Empty_Phones_On_Create()
        {
            var person = ContactableTestHelper.CreatePerson();

            person.Phones.Count.Should().Be(0);
        }

        [Fact]
        public void AddPhone()
        {
            var person = ContactableTestHelper.CreatePerson();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            person.AddPhone(phone);

            person.Phones.Should().Contain(phone);
        }

        [Fact]
        public void RemovePhone()
        {
            var person = ContactableTestHelper.CreatePerson();
            var number = "555.444.3333";
            var phoneType = PhoneType.Mobile;
            var phoneOne = Phone.Create(number, phoneType, true).Value;
            person.AddPhone(phoneOne);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            var phoneTwo = Phone.Create(number, phoneType, false).Value;
            person.AddPhone(phoneTwo);

            person.Phones.Count.Should().Be(2);
            person.RemovePhone(phoneOne);

            person.Phones.Count.Should().Be(1);
            person.Phones.Should().Contain(phoneTwo);
        }

        [Fact]
        public void Return_Failure_On_Add_More_Than_One_Primary_Phone()
        {
            var person = ContactableTestHelper.CreatePerson();
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            person.AddPhone(phone);
            number = "444.627.9206";
            phone = Phone.Create(number, PhoneType.Mobile, true).Value;

            var result = person.AddPhone(phone);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_Failure_On_Add_Duplicate_Phone()
        {
            var person = ContactableTestHelper.CreatePerson();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            person.AddPhone(phone);
            phone = Phone.Create(number, phoneType, true).Value;

            var result = person.AddPhone(phone);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void AddEmail()
        {
            var person = ContactableTestHelper.CreatePerson();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            person.AddEmail(email);

            person.Emails.Should().Contain(email);
        }

        [Fact]
        public void RemoveEmail()
        {
            var person = ContactableTestHelper.CreatePerson();
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
        public void Return_Failure_On_Add_GreaterThan_One_Primary_Email()
        {
            var person = ContactableTestHelper.CreatePerson();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            person.AddEmail(email);
            address = "june@done.com";
            email = Email.Create(address, true).Value;

            var result = person.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_Failure_On_Add_Duplicate_Email()
        {
            var person = ContactableTestHelper.CreatePerson();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            person.AddEmail(email);
            email = Email.Create(address, true).Value;

            var result = person.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        public void Return_Failure_On_Add_Invalid_Email(Email email)
        {
            var person = ContactableTestHelper.CreatePerson();

            var result = person.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        public void Return_Failure_On_Add_Invalid_Phone(Phone phone)
        {
            var person = ContactableTestHelper.CreatePerson();

            var result = person.AddPhone(phone);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetName()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;
            var notes = Utilities.LoremIpsum(100);
            var person = Person.Create(name, notes).Value;
            firstName = "Jill";
            lastName = "Done";
            name = PersonName.Create(lastName, firstName).Value;

            person.SetName(name);

            person.Name.FirstName.Should().Be(firstName);
            person.Name.LastName.Should().Be(lastName);
        }

        [Fact]
        public void Fail_SetName_With_Null_Parameter()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;
            var notes = Utilities.LoremIpsum(100);
            var person = Person.Create(name, notes).Value;

            var result = person.SetName(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.RequiredMessage);
        }

        [Fact]
        public void SetBirthday()
        {
            var person = ContactableTestHelper.CreatePerson();
            var birthday = DateTime.Today.AddYears(-50);

            var result = person.SetBirthday(birthday);

            result.IsSuccess.Should().BeTrue();
            person.Birthday.Should().BeCloseTo(birthday, 1.Minutes());
        }

        [Fact]
        public void Return_Failure_On_Set_Null_Birthday()
        {
            var person = ContactableTestHelper.CreatePerson();
            DateTime? birthday = null;

            var result = person.SetBirthday(birthday);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.InvalidValueMessage);
        }

        [Fact]
        public void SetBirthday_Failure_Too_Young()
        {
            var person = ContactableTestHelper.CreatePerson();
            var birthday = DateTime.Today.AddYears(1);

            var result = person.SetBirthday(birthday);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.InvalidValueMessage);
        }

        [Fact]
        public void SetBirthday_Failure_Too_Old()
        {
            var person = ContactableTestHelper.CreatePerson();
            var birthday = DateTime.Today.AddYears(-200);

            var result = person.SetBirthday(birthday);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.InvalidValueMessage);
        }

        [Fact]
        public void SetDriversLicense()
        {
            var driversLicenseNumber = "123456789POIUYTREWQ";
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);
            var driversLicenseValidRange = DateTimeRange.Create(issued, expiry).Value;
            var driversLicenseOrError = DriversLicense.Create(driversLicenseNumber, State.MI, driversLicenseValidRange);
            var person = ContactableTestHelper.CreatePerson();

            person.SetDriversLicense(driversLicenseOrError.Value);

            person.DriversLicense.Number.Should().Be(driversLicenseNumber);
        }

        [Fact]
        public void SetDriversLicense_Failure()
        {
            var person = ContactableTestHelper.CreatePerson();

            var result = person.SetDriversLicense(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.InvalidValueMessage);
        }

        [Fact]
        public void SetAddress()
        {
            var addressLine1 = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";
            var person = ContactableTestHelper.CreatePerson();
            var address = Address.Create(addressLine1, city, state, postalCode);

            person.SetAddress(address.Value);

            using (new AssertionScope())
            {
                person.Address.AddressLine1.Should().Be(addressLine1);
                person.Address.City.Should().Be(city);
                person.Address.State.Should().Be(state);
                person.Address.PostalCode.Should().Be(postalCode);
            }
        }
    }
}
