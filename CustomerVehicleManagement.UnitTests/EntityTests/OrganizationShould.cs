using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
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
            Organization organization = null;
            var organizationNameOrError = OrganizationName.Create(name);

            // Act
            if (!organizationNameOrError.IsFailure)
                organization = new Organization(organizationNameOrError.Value);

            // Assert
            organization.Should().NotBeNull();
        }

        [Fact]
        public void NotCreateOrganizationWithNullName()
        {
            string name = null;
            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsFailure.Should().BeTrue();
            organizationNameOrError.Error.Should().Be(OrganizationName.RequiredMessage);
        }

        [Fact]
        public void NotCreateOrganizationWithEmptyName()
        {
            var name = "";
            var organizationNameOrError = OrganizationName.Create(name);

            Action action = () => new Organization(organizationNameOrError.Value);

            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void CreateOrganizationWithAddress()
        {
            var organization = Helpers.CreateValidOrganization();
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";
            var address = Address.Create(addressLine, city, state, postalCode);

            organization.SetAddress(address.Value);

            organization.Address.AddressLine.Should().Be(addressLine);
            organization.Address.City.Should().Be(city);
            organization.Address.State.Should().Be(state);
            organization.Address.PostalCode.Should().Be(postalCode);
        }

        [Fact]
        public void CreateOrganizationWithContact()
        {
            var organization = Helpers.CreateValidOrganization();
            var firstName = "Jane";
            var lastName = "Doe";
            var personName = PersonName.Create(lastName, firstName).Value;
            var contact = new Person(personName, Gender.Female);

            organization.SetContact(contact);

            organization.Name.Should().NotBeNull();
            organization.Contact.Name.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void CreateOrganizationWithPhones()
        {
            var organization = Helpers.CreateValidOrganization();
            var phones = new List<Phone>();
            var number = "555.444.3333";
            var phoneType = PhoneType.Mobile;
            var phone = new Phone(number, phoneType, true);
            phones.Add(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = new Phone(number, phoneType, false);
            phones.Add(phone);

            organization.SetPhones(phones);

            organization.Phones.Count.Should().Be(2);
            organization.Phones.Contains(phone);
        }

        [Fact]
        public void CreateOrganizationWithEmails()
        {
            var organization = Helpers.CreateValidOrganization();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = new Email(address, true);
            emails.Add(email);
            address = "june@done.com";
            email = new Email(address, false);
            emails.Add(email);

            organization.SetEmails(emails);

            organization.Emails.Count.Should().Be(2);
        }

        [Fact]
        public void HaveEmptyPhonesOnCreate()
        {
            var organization = Helpers.CreateValidOrganization();
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
            var organization = Helpers.CreateValidOrganization();
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);

            organization.AddPhone(phone);

            organization.Phones.Should().Contain(phone);
        }

        [Fact]
        public void RemovePhone()
        {
            var organization = Helpers.CreateValidOrganization();
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
            var organization = Helpers.CreateValidOrganization();
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
            var organization = Helpers.CreateValidOrganization();
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);
            organization.AddPhone(phone);
            number = "444.627.9206";
            phone = new Phone(number, phoneType, true);

            Action action = () => organization.AddPhone(phone);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void NotAddDuplicatePhone()
        {
            var organization = Helpers.CreateValidOrganization();
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = new Phone(number, phoneType, true);
            organization.AddPhone(phone);
            phone = new Phone(number, phoneType, true);

            Action action = () => organization.AddPhone(phone);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void AddEmail()
        {
            var organization = Helpers.CreateValidOrganization();
            var address = "jane@doe.com";
            var email = new Email(address, true);

            organization.AddEmail(email);

            organization.Emails.Should().Contain(email);
        }

        [Fact]
        public void RemoveEmail()
        {
            var organization = Helpers.CreateValidOrganization();
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
            var organization = Helpers.CreateValidOrganization();
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
            var organization = Helpers.CreateValidOrganization();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = new Email(address, true);
            emails.Add(email);
            address = "june@done.com";
            email = new Email(address, true);
            emails.Add(email);

            Action action = () => organization.SetEmails(emails);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void NotSetEmailsWithDuplicateEmails()
        {
            var organization = Helpers.CreateValidOrganization();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = new Email(address, false);
            emails.Add(email);
            emails.Add(email);

            Action action = () => organization.SetEmails(emails);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void NotSetEmailsToNull()
        {
            var organization = Helpers.CreateValidOrganization();
            List<Email> emails = null;

            Action action = () => organization.SetEmails(emails);

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void NotCreateMoreThanOnePrimaryEmail()
        {
            var organization = Helpers.CreateValidOrganization();
            var address = "jane@doe.com";
            var email = new Email(address, true);
            organization.AddEmail(email);
            address = "june@done.com";
            email = new Email(address, true);

            Action action = () => organization.AddEmail(email);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void NotAddDuplicateEmail()
        {
            var organization = Helpers.CreateValidOrganization();
            var address = "jane@doe.com";
            var email = new Email(address, false);

            organization.AddEmail(email);
            email = new Email(address, true);

            Action action = () => organization.AddEmail(email);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void SetName()
        {
            var organization = Helpers.CreateValidOrganization();
            var name = "jane's";
            var organizationNameOrError = OrganizationName.Create(name);

            organization.SetName(organizationNameOrError.Value);

            organization.Name.Should().Be(organizationNameOrError.Value);
        }

        [Fact]
        public void SetContact()
        {
            var organization = Helpers.CreateValidOrganization();
            var firstName = "Jane";
            var lastName = "Doe";
            var personName = PersonName.Create(lastName, firstName).Value;
            var contact = new Person(personName, Gender.Female);

            organization.SetContact(contact);

            organization.Contact.Name.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void SetAddress()
        {
            var organization = Helpers.CreateValidOrganization();
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";
            var address = Address.Create(addressLine, city, state, postalCode);

            organization.SetAddress(address.Value);

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
            var organization = Helpers.CreateValidOrganization();
            var notes = "Behold, notes!";

            organization.SetNote(notes);

            organization.Note.Should().Be(notes);
        }
    }
}
