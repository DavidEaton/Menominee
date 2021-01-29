using CustomerVehicleManagement.Domain.Entities;
using NUnit.Framework;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;

namespace CustomerVehicleManagement.Tests.EntityTests
{
    public class PersonShould
    {
        [Test]
        public void CreateNewPerson()
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
        public void NotCreateNewPersonWithNullName()
        {
            string firstName = null;
            string lastName = null;

            var exception = Assert.Throws<ArgumentException>(
                () => { new Person(new PersonName(lastName, firstName), Gender.Female); });

            Assert.That(exception.Message, Is.EqualTo(PersonName.PersonNameEmptyMessage));
        }

        [Test]
        public void NotCreateNewPersonWithEmptyName()
        {
            string firstName = "";
            string lastName = "";

            var exception = Assert.Throws<ArgumentException>(
                () => { new Person(new PersonName(lastName, firstName), Gender.Female); });

            Assert.That(exception.Message, Is.EqualTo(PersonName.PersonNameEmptyMessage));
        }

        [Test]
        public void CreateNewPersonWithEmptyLastName()
        {
            string firstName = "Molly";
            string lastName = "";

            var person = new Person(new PersonName(lastName, firstName), Gender.Female);

            Assert.That(person, Is.Not.Null);
        }

        [Test]
        public void CreateNewPersonWithEmptyFirstName()
        {
            string firstName = "";
            string lastName = "Moops";

            var person = new Person(new PersonName(lastName, firstName), Gender.Female);

            Assert.That(person, Is.Not.Null);
        }

        [Test]
        public void CreateNewPersonWithNullLastName()
        {
            string firstName = "Molly";
            string lastName = null;

            var person = new Person(new PersonName(lastName, firstName), Gender.Female);

            Assert.That(person, Is.Not.Null);
        }

        [Test]
        public void CreateNewPersonWithNullFirstName()
        {
            string firstName = null;
            string lastName = "Moops";

            var person = new Person(new PersonName(lastName, firstName), Gender.Female);

            Assert.That(person, Is.Not.Null);
        }

        [Test]
        public void CreateNewPersonWithBirthday()
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
        public void CreateNewPersonWithDriversLicense()
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
            var person = new Person(name, Gender.Female, null, null, driversLicense);

            Assert.That(person, Is.Not.Null);
            Assert.That(person.DriversLicense, Is.Not.Null);
            Assert.That(person.DriversLicense.Number, Is.EqualTo(driversLicenseNumber));
            Assert.That(person.DriversLicense.State, Is.EqualTo(driversLicenseState));
            Assert.That(person.DriversLicense.ValidRange.Start, Is.EqualTo(issued));
            Assert.That(person.DriversLicense.ValidRange.End, Is.EqualTo(expiry));
        }

        [Test]
        public void CreateNewPersonWithAddress()
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

    }
}
