using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using Xunit;
using static CustomerVehicleManagement.Tests.Utilities;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class OrganizationShould
    {
        [Fact]
        public void Create_Organization()
        {
            // Arrange
            var name = "   Jane's";

            // Act
            var organizationOrError = Organization.Create(OrganizationName.Create(name).Value, null, null);

            // Assert
            organizationOrError.Value.Should().BeOfType<Organization>();
            organizationOrError.IsFailure.Should().BeFalse();
            organizationOrError.Value.Name.Name.Should().Be(name.Trim());
        }

        [Fact]
        public void Not_Create_Empty_Organization()
        {
            var organizationOrError = Organization.Create(null, null, null);

            organizationOrError.IsFailure.Should().BeTrue();
            organizationOrError.Error.Should().Contain(Organization.InvalidMessage);
        }

        [Fact]
        public void Not_Create_Organization_With_Null_Name()
        {
            string name = null;
            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsFailure.Should().BeTrue();
            organizationNameOrError.Error.Should().Contain(OrganizationName.InvalidMessage);
        }

        [Fact]
        public void Create_Organization_With_Address()
        {
            var organization = CreateTestOrganization();
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
        public void Create_Organization_With_Contact()
        {
            var organization = CreateTestOrganization();
            var firstName = "Jane";
            var lastName = "Doe";
            var personName = PersonName.Create(lastName, firstName).Value;
            var contact = Person.Create(personName, Gender.Female).Value;

            organization.SetContact(contact);

            organization.Name.Should().NotBeNull();
            organization.Contact.Name.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void Create_Organization_With_Phones()
        {
            var phoneCount = 10;
            var phones = CreateTestPhones(phoneCount);
            var organization = Organization.Create(OrganizationName.Create("   Jane's").Value, "note", phones: phones).Value;

            organization.Phones.Count.Should().BeGreaterThanOrEqualTo(phoneCount);
        }

        [Fact]
        public void Create_Organization_With_Emails()
        {
            var emailCount = 10;
            var emails = CreateTestEmails(emailCount);
            var organization = Organization.Create(OrganizationName.Create("   Jane's").Value, "note", emails: emails).Value;

            organization.Emails.Count.Should().BeGreaterThanOrEqualTo(emailCount);

        }

        [Fact]
        public void Have_Empty_Phones_On_Create()
        {
            var organization = CreateTestOrganization();
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
            var organization = CreateTestOrganization();
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            organization.AddPhone(phone);

            organization.Phones.Should().Contain(phone);
        }

        [Fact]
        public void RemovePhone()
        {
            var organization = CreateTestOrganization();
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
        public void Not_Add_More_Than_One_Primary_Phone()
        {
            var organization = CreateTestOrganization();
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
        public void AddEmail()
        {
            var organization = CreateTestOrganization();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            organization.AddEmail(email);

            organization.Emails.Should().Contain(email);
        }

        [Fact]
        public void RemoveEmail()
        {
            var organization = CreateTestOrganization();
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
        public void Not_Add_More_Than_One_Primary_Email()
        {
            var organization = CreateTestOrganization();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            organization.AddEmail(email);
            address = "june@done.com";
            email = Email.Create(address, true).Value;

            var result = organization.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Add_Duplicate_Email()
        {
            var organization = CreateTestOrganization();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            organization.AddEmail(email);
            email = Email.Create(address, true).Value;

            var result = organization.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetName()
        {
            var organization = CreateTestOrganization();
            var name = "jane's";
            var organizationNameOrError = OrganizationName.Create(name);

            organization.SetName(organizationNameOrError.Value);

            organization.Name.Should().Be(organizationNameOrError.Value);
        }

        [Fact]
        public void SetContact()
        {
            var organization = CreateTestOrganization();
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
            var organization = CreateTestOrganization();
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
            var organization = CreateTestOrganization();
            var notes = "Behold, notes!";

            organization.SetNote(notes);

            organization.Note.Should().Be(notes);
        }

        [Fact]
        public void Truncate_Note_To_Note_Maximum_Length()
        {
            var organization = CreateTestOrganization();
            var notes = $"Lorem ipsum {Utilities.LoremIpsum(Organization.NoteMaximumLength)}";

            organization.SetNote(notes);

            organization.Note.Length.Should().Be(Organization.NoteMaximumLength);
        }
    }
}