using Migrations.Core.Entities;
using Migrations.Core.Enums;
using Migrations.Core.ValueObjects;
using NUnit.Framework;
using System;

namespace Migrations.Tests.EntityTests
{
    public class CustomerShould
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateNewCustomerWithPersonEntity()
        {
            // Arrange
            string firstName = "Jane";
            string lastName = "Doe";

            // Act
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);

            // Assert
            var customer = new Customer(person);
            Assert.That(customer.Entity is Person);
        }

        [Test]
        public void CreateNewCustomerWithOrganizationEntity()
        {
            string name = "Jane's";
            var organization = new Organization(name);

            var customer = new Customer(organization);
            Assert.That(customer.Entity is Organization);
        }


        [Test]
        public void NotCreateOrganizationWithEmptyName()
        {
            string name = "";

            var exception = Assert.Throws<ArgumentException>(
                () => new Organization(name));

            Assert.That(exception.Message, Is.EqualTo(Organization.OrganizationNameEmptyMessage));
        }

        [Test]
        public void NotCreateOrganizationWithEmptyContact()
        {
            string janesAuto = "Jane's Auto";
            Person person = null;

            var exception = Assert.Throws<ArgumentException>(
                () =>
                {
                    new Organization(janesAuto, person);
                });

            Assert.That(exception.Message, Is.EqualTo(Organization.OrganizationContactNullMessage));
        }

        [Test]
        public void CreateNewPersonCustomerWithAddress()
        {
            string addressLine = "1234 Five Street";
            string city = "Gaylord";
            string countryCode = "1";
            string state = "MI";
            string postalCode = "49735";
            string firstName = "Jane";
            string lastName = "Doe";

            var name = new PersonName(lastName, firstName);
            var address = new Address(addressLine, city, state, postalCode, countryCode);

            var person = new Person(name, Gender.Female, null, null, address);

            var customer = new Customer(person);
            Person jane = (Person)customer.Entity;

            Assert.That($"{lastName}, {firstName}", Is.EqualTo(jane.Name.LastFirstMiddle));
            Assert.That(addressLine, Is.EqualTo(jane.Address.AddressLine));
            Assert.That(city, Is.EqualTo(jane.Address.City));
            Assert.That(countryCode, Is.EqualTo(jane.Address.CountryCode));
            Assert.That(state, Is.EqualTo(jane.Address.State));
            Assert.That(postalCode, Is.EqualTo(jane.Address.PostalCode));
        }

        [Test]
        public void CreateNewOrganizationCustomerWithAddress()
        {
            string name = "Jane's";
            var organization = new Organization(name);

            var customer = new Customer(organization);
            Assert.That(customer.Entity is Organization);

            Organization janes = (Organization)customer.Entity;

            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";
            var countryCode = "1";

            janes.Address = new Address(addressLine, city, state, postalCode, countryCode);

            Assert.That(addressLine, Is.EqualTo(janes.Address.AddressLine));
            Assert.That(city, Is.EqualTo(janes.Address.City));
            Assert.That(countryCode, Is.EqualTo(janes.Address.CountryCode));
            Assert.That(state, Is.EqualTo(janes.Address.State));
            Assert.That(postalCode, Is.EqualTo(janes.Address.PostalCode));
        }

        [Test]
        public void CreateNewOrganizationCustomerWithPersonContact()
        {
            string janesAuto = "Jane's Auto";
            string firstName = "Jane";
            string lastName = "Doe";

            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);

            var organization = new Organization(janesAuto)
            {
                Contact = person
            };

            var customer = new Customer(organization);
            Organization janes = (Organization)customer.Entity;

            Assert.That(janes.Contact is Person);
            Assert.That(janes.Contact.Name.LastFirstMiddle == $"{lastName}, {firstName}");
        }

    }
}