using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.Entities
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
            var person = Utilities.CreateTestPerson();

            person.Phones.Count.Should().Be(0);
        }

        [Fact]
        public void AddPhone()
        {
            var person = Utilities.CreateTestPerson();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            person.AddPhone(phone);

            person.Phones.Should().Contain(phone);
        }

        [Fact]
        public void RemovePhone()
        {
            var person = Utilities.CreateTestPerson();
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
        public void Not_Add_More_Than_One_Primary_Phone()
        {
            var person = Utilities.CreateTestPerson();
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
        public void Not_Add_Duplicate_Phone()
        {
            var person = Utilities.CreateTestPerson();
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
            var person = Utilities.CreateTestPerson();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            person.AddEmail(email);

            person.Emails.Should().Contain(email);
        }

        [Fact]
        public void RemoveEmail()
        {
            var person = Utilities.CreateTestPerson();
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
        public void Not_Add_GreaterThan_One_Primary_Email()
        {
            var person = Utilities.CreateTestPerson();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            person.AddEmail(email);
            address = "june@done.com";
            email = Email.Create(address, true).Value;

            var result = person.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Add_Duplicate_Email()
        {
            var person = Utilities.CreateTestPerson();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            person.AddEmail(email);
            email = Email.Create(address, true).Value;

            var result = person.AddEmail(email);

            result.IsFailure.Should().BeTrue();
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
            var person = Utilities.CreateTestPerson();

            person.Gender.Should().Be(Gender.Female);
            person.SetGender(Gender.Male);

            person.Gender.Should().Be(Gender.Male);
        }

        [Fact]
        public void SetBirthday()
        {
            var person = Utilities.CreateTestPerson();

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
            var person = Utilities.CreateTestPerson();

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
            var person = Utilities.CreateTestPerson();
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
