using Migrations.Core.Entities;
using NUnit.Framework;

namespace Migrations.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateNewPersonCustomerWithAddress()
        {
            var address = new Address
            {
                AddressLine = "1234 Five Street",
                City = "Gaylord",
                CountryCode = "1",
                State = "MI",
                PostalCode = "49735"
            };

            var person = new Person
            {
                FirstName = "Jane",
                LastName = "Doe",
                Address = address
            };

            var customer = new Customer(person);
            Person jane = (Person)customer.Entity;

            Assert.That("Doe, Jane", Is.EqualTo(jane.NameLastFirst));
            Assert.That("Gaylord", Is.EqualTo(jane.Address.City));
        }

        [Test]
        public void CreateNewOrganizationCustomerWithAddress()
        {
            var address = new Address
            {
                AddressLine = "5432 One Street",
                City = "Petoskey",
                CountryCode = "1",
                State = "MI",
                PostalCode = "49770"
            };

            var organization = new Organization
            {
                Name = "Jane's Auto",
                Address = address
            };

            var customer = new Customer(organization);
            Organization janes = (Organization)customer.Entity;

            Assert.That("Jane's Auto", Is.EqualTo(janes.Name));
            Assert.That("49770", Is.EqualTo(janes.Address.PostalCode));
        }

    }
}