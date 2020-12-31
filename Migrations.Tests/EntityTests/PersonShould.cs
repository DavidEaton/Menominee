using Migrations.Core.Entities;
using Migrations.Core.Enums;
using Migrations.Core.ValueObjects;
using NUnit.Framework;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace Migrations.Tests.EntityTests
{
    public class PersonShould
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateNewPerson()
        {
            // Arrange
            var firstName = "Jane";
            var lastName = "Doe";

            // Act
            var name = new PersonName(lastName, firstName);
            var person = new Person(name);

            // Assert
            Assert.That(person, Is.Not.Null);
        }

        [Test]
        public void NotCreateNewPersonWithEmptyName()
        {
            string firstName = null;
            string lastName = null;

            var exception = Assert.Throws<ArgumentException>(
                () => { new Person(new PersonName(lastName, firstName)); });
            Assert.That(exception.Message, Is.EqualTo(PersonName.PersonNameEmptyMessage));
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

            var driversLicense = new DriversLicence(driversLicenseNumber, driversLicenseState, driversLicenseValidRange);
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, driversLicense);

            Assert.That(person, Is.Not.Null);
            Assert.That(person.DriversLicence, Is.Not.Null);
            Assert.That(person.DriversLicence.Number, Is.EqualTo(driversLicenseNumber));
            Assert.That(person.DriversLicence.State, Is.EqualTo(driversLicenseState));
            Assert.That(person.DriversLicence.ValidRange.Start, Is.EqualTo(issued));
            Assert.That(person.DriversLicence.ValidRange.End, Is.EqualTo(expiry));
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
            var countryCode = "1";

            var address = new Address(addressLine, city, state, postalCode, countryCode);

            var name = new PersonName(lastName, firstName);
            var person = new Person(name, null, address);

            Assert.That(person, Is.Not.Null);
            Assert.That(person.Address, Is.Not.Null);
            Assert.That(person.Address.AddressLine, Is.EqualTo(addressLine));
            Assert.That(person.Address.City, Is.EqualTo(city));
            Assert.That(person.Address.State, Is.EqualTo(state));
            Assert.That(person.Address.PostalCode, Is.EqualTo(postalCode));
            Assert.That(person.Address.CountryCode, Is.EqualTo(countryCode));
        }

        [Test]
        public void CreateNewPersonWithPhones()
        {
            var number1 = "989.627.9206";
            var number2 = "555-555-5555";
            var phone1 = new Phone(number1, PhoneType.Home);
            var phone2 = new Phone(number2, PhoneType.Mobile);
            var firstName = "Jane";
            var lastName = "Doe";
            var phones = new List<Phone>
            {
                phone1,
                phone2
            };

            var name = new PersonName(lastName, firstName);
            var person = new Person(name, null, null, phones);

            Assert.That(person, Is.Not.Null);
            Assert.That(person.Phones, Is.Not.Null);
            Assert.That(person.Phones[0].Number, Is.EqualTo(number1));
            Assert.That(person.Phones[0].PhoneType, Is.EqualTo(PhoneType.Home));
            Assert.That(person.Phones[1].Number, Is.EqualTo(number2));
            Assert.That(person.Phones[1].PhoneType, Is.EqualTo(PhoneType.Mobile));
        }
    }
}
