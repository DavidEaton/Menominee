using CustomerVehicleManagement.Domain.BaseClasses;
using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class OrganizationShould
    {
        [Fact]
        public void CreateOrganization()
        {
            // Arrange
            var name = "Jane's";

            // Act
            var organization = new Organization(name);

            // Assert
            organization.Should().NotBeNull();
        }

        [Fact]
        public void NotCreateOrganizationWithNullName()
        {
            string name = null;

            Action action = () => new Organization(name);

            action.Should().Throw<ArgumentException>()
                           .WithMessage($"{Organization.OrganizationNameEmptyMessage} (Parameter 'name')")
                           .And
                           .ParamName.Should().Be("name");
        }

        [Fact]
        public void NotCreateOrganizationWithEmptyName()
        {
            var name = "";

            Action action = () => new Organization(name);

            action.Should().Throw<ArgumentException>()
                           .WithMessage($"{Organization.OrganizationNameEmptyMessage} (Parameter 'name')")
                           .And
                           .ParamName.Should().Be("name");
        }

        [Fact]
        public void CreateOrganizationWithAddress()
        {
            var name = "Jane's";
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";
            var address = new Address(addressLine, city, state, postalCode);

            var organization = new Organization(name, address);

            organization.Address.AddressLine.Should().Be(addressLine);
            organization.Address.City.Should().Be(city);
            organization.Address.State.Should().Be(state);
            organization.Address.PostalCode.Should().Be(postalCode);
        }

        [Fact]
        public void CreateOrganizationWithContact()
        {
            var organizationName = "Jane's";
            var firstName = "Jane";
            var lastName = "Doe";
            var personName = new PersonName(lastName, firstName);
            var contact = new Person(personName, Gender.Female);

            var organization = new Organization(organizationName, null, contact);

            organization.Name.Should().Be(organizationName);
            organization.Contact.Name.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void CreateOrganizationWithPhones()
        {
            var organizationName = "Jane's";
            var phones = new List<Phone>();
            var number = "555.444.3333";
            var phoneType = PhoneType.Mobile;
            var phone = new Phone(number, phoneType, true);
            phones.Add(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = new Phone(number, phoneType, false);
            phones.Add(phone);

            var organization = new Organization(organizationName, null, null, phones);

            organization.Phones.Count.Should().Be(2);
        }

        [Fact]
        public void CreateOrganizationWithEmails()
        {
            var organizationName = "Jane's";
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = new Email(address, true);
            emails.Add(email);
            address = "june@done.com";
            email = new Email(address, false);
            emails.Add(email);

            var organization = new Organization(organizationName, null, null, null, emails);

            organization.Emails.Count.Should().Be(2);
        }

        [Fact]
        public void HaveEmptyPhonesOnCreate()
        {
            var organizationName = "Jane's";
            var organization = new Organization(organizationName);
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);

            organization.Phones.Count.Should().Be(0);

            organization.AddPhone(phone);
            organization.Phones.Count.Should().Be(1);
        }

        [Fact]
        public void AddPhone()
        {
            var organizationName = "Jane's";
            var organization = new Organization(organizationName);
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);

            organization.AddPhone(phone);

            organization.Phones.Should().Contain(phone);
        }

        [Fact]
        public void RemovePhone()
        {
            var organizationName = "Jane's";
            var organization = new Organization(organizationName);
            var number = "989.627.9206";
            var phoneType = PhoneType.Mobile;
            var phone = new Phone(number, phoneType, true);
            organization.AddPhone(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = new Phone(number, phoneType, false);
            organization.AddPhone(phone);

            organization.Phones.Count.Should().Be(2);
            organization.RemovePhone(phone);

            organization.Phones.Count.Should().Be(1);

        }

        [Fact]
        public void SetPhones()
        {
            var organizationName = "Jane's";
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

            organization.Phones.Count.Should().Be(2);
        }

        [Fact]
        public void NotCreateMoreThanOnePrimaryPhone()
        {
            var organizationName = "Jane's";
            var organization = new Organization(organizationName);
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);
            organization.AddPhone(phone);
            number = "444.627.9206";
            phone = new Phone(number, phoneType, true);

            Action action = () => organization.AddPhone(phone);

            action.Should().Throw<InvalidOperationException>()
                           .WithMessage(Contactable.PrimaryPhoneExistsMessage);
        }

        [Fact]
        public void NotAddDuplicatePhone()
        {
            var organizationName = "Jane's";
            var organization = new Organization(organizationName);
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);
            organization.AddPhone(phone);
            phone = new Phone(number, phoneType, true);

            Action action = () => organization.AddPhone(phone);

            action.Should().Throw<InvalidOperationException>()
                           .WithMessage(Contactable.DuplicatePhoneExistsMessage);
        }

        [Fact]
        public void AddEmail()
        {
            var organization = new Organization("Jane's");
            var address = "jane@doe.com";
            var email = new Email(address, true);

            organization.AddEmail(email);

            organization.Emails.Should().Contain(email);
        }

        [Fact]
        public void RemoveEmail()
        {
            var organization = new Organization("Jane's");
            var address = "jane@doe.com";
            var email0 = new Email(address, true);
            organization.AddEmail(email0);
            address = "june@doe.com";
            var email1 = new Email(address, false);
            organization.AddEmail(email1);

            organization.Emails.Count.Should().Be(2);
            organization.RemoveEmail(email0);

            organization.Emails.Count.Should().Be(1);
            organization.Emails.Should().Contain(email1);
        }

        [Fact]
        public void SetEmails()
        {
            var organization = new Organization("Jane's");
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email0 = new Email(address, true);
            emails.Add(email0);
            address = "june@done.com";
            var email1 = new Email(address, false);
            emails.Add(email1);

            organization.SetEmails(emails);

            organization.Emails.Count.Should().Be(2);
            organization.Emails.Should().Contain(email0);
            organization.Emails.Should().Contain(email1);
        }

        [Fact]
        public void NotSetEmailsHavingMoreThanOnePrimaryEmail()
        {
            var organization = new Organization("Jane's");
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = new Email(address, true);
            emails.Add(email);
            address = "june@done.com";
            email = new Email(address, true);
            emails.Add(email);

            Action action = () => organization.SetEmails(emails);

            action.Should().Throw<InvalidOperationException>()
                           .WithMessage(Organization.PrimaryEmailExistsMessage);
        }

        [Fact]
        public void NotSetEmailsWithDuplicateEmails()
        {
            var organization = new Organization("Jane's");
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = new Email(address, false);
            emails.Add(email);
            emails.Add(email);

            Action action = () => organization.SetEmails(emails);

            action.Should().Throw<InvalidOperationException>()
                           .WithMessage(Contactable.DuplicateEmailExistsMessage);
        }

        [Fact]
        public void NotSetEmailsToNull()
        {
            var organization = new Organization("Jane's");
            List<Email> emails = null;

            Action action = () => organization.SetEmails(emails);

            action.Should().Throw<ArgumentException>()
                           .WithMessage($"{Organization.EmptyEmailCollectionMessage} (Parameter 'emails')")
                           .And
                           .ParamName.Should().Be("emails");
        }

        [Fact]
        public void NotCreateMoreThanOnePrimaryEmail()
        {
            var organization = new Organization("Jane's");
            var address = "jane@doe.com";
            var email = new Email(address, true);
            organization.AddEmail(email);
            address = "june@done.com";
            email = new Email(address, true);

            Action action = () => organization.AddEmail(email);

            action.Should().Throw<InvalidOperationException>()
                           .WithMessage(Organization.PrimaryEmailExistsMessage);
        }

        [Fact]
        public void NotAddDuplicateEmail()
        {
            var organization = new Organization("Jane's");
            var address = "jane@doe.com";
            var email = new Email(address, false);

            organization.AddEmail(email);
            email = new Email(address, true);

            Action action = () => organization.AddEmail(email);

            action.Should().Throw<InvalidOperationException>()
                           .WithMessage(Contactable.DuplicateEmailExistsMessage);
        }

        [Fact]
        public void SetName()
        {
            var organizationName = "Jane's";
            var organization = new Organization(organizationName);
            organizationName = "June's";

            organization.SetName(organizationName);

            organization.Name.Should().Be(organizationName);
        }

        [Fact]
        public void SetContact()
        {
            var organizationName = "Jane's";
            var firstName = "Jane";
            var lastName = "Doe";
            var personName = new PersonName(lastName, firstName);
            var contact = new Person(personName, Gender.Female);
            var organization = new Organization(organizationName);

            organization.SetContact(contact);

            organization.Contact.Name.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void SetAddress()
        {
            var organizationName = "Jane's";
            var organization = new Organization(organizationName);
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";
            var address = new Address(addressLine, city, state, postalCode);

            organization.SetAddress(address);

            using (new AssertionScope())
            {
                organization.Address.AddressLine.Should().Be(addressLine);
                organization.Address.City.Should().Be(city);
                organization.Address.State.Should().Be(state);
                organization.Address.PostalCode.Should().Be(postalCode);
            }
        }

        [Fact]
        public void SetNotes()
        {
            var organizationName = "Jane's";
            var notes = "Behold, notes!";
            var organization = new Organization(organizationName);

            organization.SetNotes(notes);

            organization.Notes.Should().Be(notes);
        }

        [Fact]
        public void UpdateOrganizationAsync()
        {
            var organizationName = "Jane's";
            var organization = new Organization(organizationName);

            organization.Name.Should().Be(organizationName);

        }
    }
}
