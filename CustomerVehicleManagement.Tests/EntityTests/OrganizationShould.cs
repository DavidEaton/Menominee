using CustomerVehicleManagement.Domain.Entities;
using NUnit.Framework;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;

namespace CustomerVehicleManagement.Tests.EntityTests
{
    public class OrganizationShould
    {
        [Test]
        public void CreateNewOrganization()
        {
            // Arrange
            string name = "Jane's";

            // Act
            var organization = new Organization(name);

            // Assert
            Assert.That(organization, Is.Not.Null);
        }

        [Test]
        public void NotCreateOrganizationWithNullName()
        {
            string name = null;

            var exception = Assert.Throws<ArgumentException>(
                () => new Organization(name));

            Assert.That(exception.Message, Is.EqualTo(Organization.OrganizationNameEmptyMessage));
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
        public void CreateNewOrganizationWithAddress()
        {
            string name = "Jane's";
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";
            var countryCode = "1";

            var address = new Address(addressLine, city, state, postalCode, countryCode);

            var organization = new Organization(name, address);

            Assert.That(organization, Is.Not.Null);
            Assert.That(organization.Address, Is.Not.Null);
            Assert.That(organization.Address.AddressLine, Is.EqualTo(addressLine));
            Assert.That(organization.Address.City, Is.EqualTo(city));
            Assert.That(organization.Address.State, Is.EqualTo(state));
            Assert.That(organization.Address.PostalCode, Is.EqualTo(postalCode));
            Assert.That(organization.Address.CountryCode, Is.EqualTo(countryCode));
        }

        [Test]
        public void CreateNewOrganizationWithContact()
        {
            string organizationName = "Jane's";
            string firstName = "Jane";
            string lastName = "Doe";

            // Act
            var personName = new PersonName(lastName, firstName);
            var contact = new Person(personName, Gender.Female);


            var organization = new Organization(organizationName, null, contact);

            Assert.That(organization, Is.Not.Null);
            Assert.That(organization.Contact, Is.Not.Null);
            Assert.That(organization.Name, Is.EqualTo(organizationName));
            Assert.That(organization.Contact.Name.FirstName, Is.EqualTo(firstName));
        }
    }
}
