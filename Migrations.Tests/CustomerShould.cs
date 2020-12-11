using Migrations.Core.Entities;
using NUnit.Framework;
using System;

namespace Migrations.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateNewPerson()
        {
            string firstName = "Jane";
            string lastName = "Doe";

            var person = new Person(lastName, firstName);

            Assert.That($"{lastName}, {firstName}", Is.EqualTo(person.NameLastFirst));
            Assert.That($"{firstName} {lastName}", Is.EqualTo(person.NameFirstLast));
        }

        public void CreateNewPersonWithMiddleName()
        {
            string firstName = "Jane";
            string lastName = "Doe";
            string middleName = "Zebra";

            var person = new Person(lastName, firstName, middleName);

            Assert.That($"{lastName}, {firstName} {middleName}", Is.EqualTo(person.NameLastFirst));
            Assert.That($"{firstName} {middleName} {lastName}", Is.EqualTo(person.NameFirstLast));
        }

        [Test]
        public void CreateNewCustomerWithPersonEntity()
        {
            string firstName = "Jane";
            string lastName = "Doe";

            var person = new Person(lastName, firstName);

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
        public void CreateNewPersonCustomerWithAddress()
        {
            string addressLine = "1234 Five Street";
            string city = "Gaylord";
            string countryCode = "1";
            string state = "MI";
            string postalCode = "49735";
            string firstName = "Jane";
            string lastName = "Doe";

            var address = new Address
            {
                AddressLine = addressLine,
                City = city,
                CountryCode = countryCode,
                State = state,
                PostalCode = postalCode
            };

            var person = new Person(lastName, firstName)
            {
                Address = address
            };


            var customer = new Customer(person);
            Person jane = (Person)customer.Entity;

            Assert.That($"{lastName}, {firstName}", Is.EqualTo(jane.NameLastFirst));
            Assert.That(addressLine, Is.EqualTo(jane.Address.AddressLine));
            Assert.That(city, Is.EqualTo(jane.Address.City));
            Assert.That(countryCode, Is.EqualTo(jane.Address.CountryCode));
            Assert.That(state, Is.EqualTo(jane.Address.State));
            Assert.That(postalCode, Is.EqualTo(jane.Address.PostalCode));
            Assert.That(postalCode, Is.EqualTo(jane.Address.PostalCode));
        }

        [Test]
        public void CreateNewOrganizationCustomerWithAddress()
        {
            string janesAuto = "Jane's Auto";

            string addressLine = "5432 One Street";
            string city = "Petoskey";
            string countryCode = "1";
            string state = "MI";
            string postalCode = "49770";

            var address = new Address
            {
                AddressLine = addressLine,
                City = city,
                CountryCode = countryCode,
                State = state,
                PostalCode = postalCode
            };

            var organization = new Organization(janesAuto)
            {
                Address = address
            };

            var customer = new Customer(organization);
            Organization janes = (Organization)customer.Entity;

            Assert.That(janesAuto, Is.EqualTo(janes.Name));
            Assert.That(addressLine, Is.EqualTo(janes.Address.AddressLine));
            Assert.That(city, Is.EqualTo(janes.Address.City));
            Assert.That(countryCode, Is.EqualTo(janes.Address.CountryCode));
            Assert.That(state, Is.EqualTo(janes.Address.State));
            Assert.That(postalCode, Is.EqualTo(janes.Address.PostalCode));
            Assert.That(address, Is.EqualTo(janes.Address));
        }
        [Test]
        public void CreateNewOrganizationCustomerWithPersonContact()
        {
            string janesAuto = "Jane's Auto";
            string firstName = "Jane";
            string lastName = "Doe";

            var person = new Person(lastName, firstName);

            var organization = new Organization(janesAuto)
            {
                Contact = person
            };

            var customer = new Customer(organization);
            Organization janes = (Organization)customer.Entity;

            Assert.That(janes.Contact is Person);
            Assert.That(janes.Contact.NameLastFirst == $"{lastName}, {firstName}");
        }

        [Test]
        public void NotCreatePersonWithEmptyName()
        {
            string firstName = "";
            string lastName = "";

            var exception = Assert.Throws<ArgumentException>(
                () => { new Person(lastName, firstName); });

            Assert.That(exception.Message, Is.EqualTo(Person.PersonNameEmptyMessage));
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
        public void NotCreateCustomerWithEmptyPersonName()
        {
            string firstName = "";
            string lastName = "";

            var exception = Assert.Throws<ArgumentException>(
                () => { new Person(lastName, firstName); });

            Assert.That(exception.Message, Is.EqualTo(Person.PersonNameEmptyMessage));
        }

        [Test]
        public void NotCreateCustomerWithEmptyOrganizationName()
        {
            string name = "";

            var exception = Assert.Throws<ArgumentException>(
                () => { new Organization(name); });

            Assert.That(exception.Message, Is.EqualTo(Organization.OrganizationNameEmptyMessage));
        }
    }
}