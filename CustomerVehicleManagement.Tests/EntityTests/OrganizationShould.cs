using CustomerVehicleManagement.Domain.Entities;
using NUnit.Framework;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

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

            var address = new Address(addressLine, city, state, postalCode);

            var organization = new Organization(name, address);

            Assert.That(organization, Is.Not.Null);
            Assert.That(organization.Address, Is.Not.Null);
            Assert.That(organization.Address.AddressLine, Is.EqualTo(addressLine));
            Assert.That(organization.Address.City, Is.EqualTo(city));
            Assert.That(organization.Address.State, Is.EqualTo(state));
            Assert.That(organization.Address.PostalCode, Is.EqualTo(postalCode));
        }

        [Test]
        public void CreateNewOrganizationWithContact()
        {
            string organizationName = "Jane's";
            string firstName = "Jane";
            string lastName = "Doe";

            var personName = new PersonName(lastName, firstName);
            var contact = new Person(personName, Gender.Female);


            var organization = new Organization(organizationName, null, contact);

            Assert.That(organization, Is.Not.Null);
            Assert.That(organization.Contact, Is.Not.Null);
            Assert.That(organization.Name, Is.EqualTo(organizationName));
            Assert.That(organization.Contact.Name.FirstName, Is.EqualTo(firstName));
        }

        [Test]
        public void AddPhonesWhenOrganizationHasNoPhonesOnAddPhone()
        {
            string organizationName = "Jane's";
            var organization = new Organization(organizationName);

            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);


            Assert.That(organization.Phones == null);
            organization.AddPhone(phone);

            Assert.That(organization.Phones.Count == 1);
        }

        [Test]
        public void AddPhone()
        {
            string organizationName = "Jane's";
            var organization = new Organization(organizationName);

            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);

            organization.AddPhone(phone);

            Assert.That(organization.Phones[0].Number == number);
        }

        [Test]
        public void RemovePhone()
        {
            string organizationName = "Jane's";
            var organization = new Organization(organizationName);

            var number = "989.627.9206";
            var phoneType = PhoneType.Mobile;
            var phone = new Phone(number, phoneType, true);

            organization.AddPhone(phone);

            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = new Phone(number, phoneType, false);

            organization.AddPhone(phone);

            Assert.That(organization.Phones.Count == 2);

            organization.RemovePhone(phone);

            Assert.That(organization.Phones.Count == 1);
            Assert.That(organization.Phones[0].Number == "989.627.9206");
        }

        [Test]
        public void SetPhones()
        {
            string organizationName = "Jane's";
            var organization = new Organization(organizationName);

            var phones = new List<Phone>();

            var number = "989.627.9206";
            var phoneType = PhoneType.Mobile;
            var phone = new Phone(number, phoneType, true);

            phones.Add(phone);

            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = new Phone(number, phoneType, false);

            phones.Add(phone);

            organization.SetPhones(phones);

            Assert.That(organization.Phones.Count == 2);
            Assert.That(organization.Phones[0].Number == "989.627.9206");
        }

        [Test]
        public void NotCreateMoreThanOnePrimaryPhone()
        {
            string organizationName = "Jane's";
            var organization = new Organization(organizationName);
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);
            organization.AddPhone(phone);
            number = "444.627.9206";
            phone = new Phone(number, phoneType, true);

            var exception = Assert.Throws<ArgumentException>(
                () => { organization.AddPhone(phone); });

            Assert.That(exception.Message, Is.EqualTo(Organization.PrimaryPhoneExistsMessage));

        }

        [Test]
        public void NotAddDuplicatePhone()
        {
            string organizationName = "Jane's";
            var organization = new Organization(organizationName);
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);
            organization.AddPhone(phone);
            phone = new Phone(number, phoneType, true);

            var exception = Assert.Throws<ArgumentException>(
                () => { organization.AddPhone(phone); });

            Assert.That(exception.Message, Is.EqualTo(Person.DuplicatePhoneExistsMessage));

        }

        [Test]
        public void SetName()
        {
            string organizationName = "Jane's";
            var organization = new Organization(organizationName);

            Assert.That(organization.Name == organizationName);

            organizationName = "June's";

            organization.SetName(organizationName);

            Assert.That(organization.Name == organizationName);
        }

        [Test]
        public void SetContact()
        {
            string organizationName = "Jane's";
            string firstName = "Jane";
            string lastName = "Doe";

            var personName = new PersonName(lastName, firstName);
            var contact = new Person(personName, Gender.Female);
            var organization = new Organization(organizationName);

            Assert.That(organization, Is.Not.Null);
            Assert.That(organization.Contact, Is.Null);

            organization.SetContact(contact);

            Assert.That(organization.Contact.Name.FirstName, Is.EqualTo(firstName));
        }

        [Test]
        public void SetAddress()
        {
            string organizationName = "Jane's";
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";

            var address = new Address(addressLine, city, state, postalCode);
            var organization = new Organization(organizationName, address);

            Assert.That(organization.Address.AddressLine, Is.EqualTo(addressLine));
            Assert.That(organization.Address.City, Is.EqualTo(city));
            Assert.That(organization.Address.State, Is.EqualTo(state));
            Assert.That(organization.Address.PostalCode, Is.EqualTo(postalCode));

            addressLine = "5432 One Street";
            city = "Petoskey";
            state = "ME";
            postalCode = "49770";

            address = new Address(addressLine, city, state, postalCode);
            organization.SetAddress(address);

            Assert.That(organization.Address.AddressLine, Is.EqualTo(addressLine));
            Assert.That(organization.Address.City, Is.EqualTo(city));
            Assert.That(organization.Address.State, Is.EqualTo(state));
            Assert.That(organization.Address.PostalCode, Is.EqualTo(postalCode));
        }

        [Test]
        public void SetNotes()
        {
            string organizationName = "Jane's";
            string notes = "Behold, notes!";
            var organization = new Organization(organizationName);

            Assert.That(string.IsNullOrWhiteSpace(organization.Notes));

            organization.SetNotes(notes);

            Assert.That(organization.Notes == notes);
        }

    }
}
