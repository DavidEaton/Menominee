using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Organizations;
using CustomerVehicleManagement.Shared.TestUtilities;
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
            var name = "   Jane's";
            Organization organization = null;
            var organizationNameOrError = OrganizationName.Create(name);

            // Act
            if (!organizationNameOrError.IsFailure)
                organization = Organization.Create(organizationNameOrError.Value, null, null).Value;

            // Assert
            organization.Should().NotBeNull();
        }

        [Fact]
        public void NotCreateEmptyOrganization()
        {
            var organizationOrError = Organization.Create(null, null, null);

            organizationOrError.IsFailure.Should().BeTrue();
            organizationOrError.Error.Should().Contain(Organization.InvalidMessage);
        }

        [Fact]
        public void NotCreateOrganizationWithNullName()
        {
            string name = null;
            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsFailure.Should().BeTrue();
            organizationNameOrError.Error.Should().Contain(OrganizationName.InvalidMessage);
        }

        [Fact]
        public void CreateOrganizationWithAddress()
        {
            var organization = OrganizationHelper.CreateOrganization();
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
            var organization = OrganizationHelper.CreateOrganization();
            var firstName = "Jane";
            var lastName = "Doe";
            var personName = PersonName.Create(lastName, firstName).Value;
            var contact = Person.Create(personName, Gender.Female).Value;

            organization.SetContact(contact);

            organization.Name.Should().NotBeNull();
            organization.Contact.Name.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void CreateOrganizationWithPhones()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var phones = new List<Phone>();
            var number = "555.444.3333";
            var phoneType = PhoneType.Mobile;
            var phone = Phone.Create(number, phoneType, true).Value;
            phones.Add(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = Phone.Create(number, phoneType, false).Value;
            phones.Add(phone);

            organization.SetPhones(phones);

            organization.Phones.Count.Should().Be(2);
            organization.Phones.Contains(phone);
        }

        [Fact]
        public void CreateOrganizationWithEmails()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            emails.Add(email);
            address = "june@done.com";
            email = Email.Create(address, false).Value;
            emails.Add(email);

            organization.SetEmails(emails);

            organization.Emails.Count.Should().Be(2);
        }

        [Fact]
        public void HaveEmptyPhonesOnCreate()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            organization.Phones.Count.Should().Be(0);

            organization.AddPhone(phone);
            organization.Phones.Count.Should().Be(1);
        }

        [Fact]
        public void AddPhone()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            organization.AddPhone(phone);

            organization.Phones.Should().Contain(phone);
        }

        [Fact]
        public void RemovePhone()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var number = "989.627.9206";
            var phoneType = PhoneType.Mobile;
            var phone = Phone.Create(number, phoneType, true).Value;
            organization.AddPhone(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = Phone.Create(number, phoneType, false).Value;
            organization.AddPhone(phone);

            organization.Phones.Count.Should().Be(2);
            organization.RemovePhone(phone);

            organization.Phones.Count.Should().Be(1);

        }

        [Fact]
        public void SetPhones()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var phones = new List<Phone>();
            var number = "989.627.9206";
            var phoneType = PhoneType.Mobile;
            var phone = Phone.Create(number, phoneType, true).Value;
            phones.Add(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = Phone.Create(number, phoneType, false).Value;
            phones.Add(phone);

            organization.SetPhones(phones);

            organization.Phones.Count.Should().Be(2);
        }

        [Fact]
        public void NotCreateMoreThanOnePrimaryPhone()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            organization.AddPhone(phone);
            number = "444.627.9206";
            phone = Phone.Create(number, phoneType, true).Value;

            Action action = () => organization.AddPhone(phone);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void NotAddDuplicatePhone()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            organization.AddPhone(phone);
            phone = Phone.Create(number, phoneType, true).Value;

            Action action = () => organization.AddPhone(phone);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void AddEmail()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            organization.AddEmail(email);

            organization.Emails.Should().Contain(email);
        }

        [Fact]
        public void RemoveEmail()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var address = "jane@doe.com";
            var email0 = Email.Create(address, true).Value;
            organization.AddEmail(email0);
            address = "june@doe.com";
            var email1 = Email.Create(address, false).Value;
            organization.AddEmail(email1);

            organization.Emails.Count.Should().Be(2);
            organization.RemoveEmail(email0);

            organization.Emails.Count.Should().Be(1);
            organization.Emails.Should().Contain(email1);
        }

        [Fact]
        public void SetEmails()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email0 = Email.Create(address, true).Value;
            emails.Add(email0);
            address = "june@done.com";
            var email1 = Email.Create(address, false).Value;
            emails.Add(email1);

            organization.SetEmails(emails);

            organization.Emails.Count.Should().Be(2);
            organization.Emails.Should().Contain(email0);
            organization.Emails.Should().Contain(email1);
        }

        [Fact]
        public void NotSetEmailsHavingMoreThanOnePrimaryEmail()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            emails.Add(email);
            address = "june@done.com";
            email = Email.Create(address, true).Value;
            emails.Add(email);

            Action action = () => organization.SetEmails(emails);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void NotSetEmailsWithDuplicateEmails()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var emails = new List<Email>();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            emails.Add(email);
            emails.Add(email);

            Action action = () => organization.SetEmails(emails);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void SetEmailsToNull()
        {
            var organization = OrganizationHelper.CreateOrganization();
            List<Email> emails = new();

            organization.SetEmails(emails);

            organization.Emails.Count.Should().Be(0);
        }

        [Fact]
        public void NotCreateMoreThanOnePrimaryEmail()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            organization.AddEmail(email);
            address = "june@done.com";
            email = Email.Create(address, true).Value;

            Action action = () => organization.AddEmail(email);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void NotAddDuplicateEmail()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            organization.AddEmail(email);
            email = Email.Create(address, true).Value;

            Action action = () => organization.AddEmail(email);

            action.Should().Throw<Exception>();
        }

        [Fact]
        public void SetName()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var name = "jane's";
            var organizationNameOrError = OrganizationName.Create(name);

            organization.SetName(organizationNameOrError.Value);

            organization.Name.Should().Be(organizationNameOrError.Value);
        }

        [Fact]
        public void SetContact()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var firstName = "Jane";
            var lastName = "Doe";
            var personName = PersonName.Create(lastName, firstName).Value;
            var contact = Person.Create(personName, Gender.Female).Value;

            organization.SetContact(contact);

            organization.Contact.Name.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void SetAddress()
        {
            var organization = OrganizationHelper.CreateOrganization();
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
            var organization = OrganizationHelper.CreateOrganization();
            var notes = "Behold, notes!";

            organization.SetNote(notes);

            organization.Note.Should().Be(notes);
        }

        [Fact]
        public void TruncateNoteToNoteMaximumLength()
        {
            var organization = OrganizationHelper.CreateOrganization();
            var notes = $"Lorem ipsum {Utilities.LoremIpsum(10000)}";

            organization.SetNote(notes);

            organization.Note.Length.Should().Be(Organization.NoteMaximumLength);
        }
    }
}