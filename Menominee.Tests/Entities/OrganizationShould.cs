using Bogus;
using Menominee.Api.Common;
using Menominee.Domain.BaseClasses;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using FluentAssertions;
using FluentAssertions.Execution;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System.Linq;
using TestingHelperLibrary;
using TestingHelperLibrary.Fakers;
using Xunit;
using static Menominee.Tests.Utilities;
using Person = Menominee.Domain.Entities.Person;

namespace Menominee.Tests.Entities
{
    public class OrganizationShould
    {
        private static readonly Faker Faker = new();

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
        public void Return_Failure_On_Create_Empty_Organization()
        {
            var organizationOrError = Organization.Create(null, null, null);

            organizationOrError.IsFailure.Should().BeTrue();
            organizationOrError.Error.Should().Contain(Organization.InvalidMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_Organization_With_Null_Name()
        {
            string name = null;
            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsFailure.Should().BeTrue();
            organizationNameOrError.Error.Should().Contain(OrganizationName.InvalidMessage);
        }

        [Fact]
        public void Create_Organization_With_Address()
        {
            var organization = ContactableTestHelper.CreateOrganization();
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
            var organization = ContactableTestHelper.CreateOrganization();
            var firstName = "Jane";
            var lastName = "Doe";
            var personName = PersonName.Create(lastName, firstName).Value;
            var notes = LoremIpsum(100);
            var contact = Person.Create(personName, Gender.Female, notes).Value;

            organization.SetContact(contact);

            organization.Name.Should().NotBeNull();
            organization.Contact.Name.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void Create_Organization_With_Phones()
        {
            var phoneCount = 10;
            var phones = ContactableTestHelper.CreatePhones(phoneCount);
            var organization = Organization.Create(
                OrganizationName.Create(
                    "   Jane's").Value, "note", phones: phones)
                .Value;

            organization.Phones.Count.Should().BeGreaterThanOrEqualTo(phoneCount);
        }

        [Fact]
        public void Create_Organization_With_Emails()
        {
            var emailCount = 10;
            var emails = ContactableTestHelper.CreateEmails(emailCount);
            var organization = Organization.Create(OrganizationName.Create("   Jane's").Value, "note", emails: emails).Value;

            organization.Emails.Count.Should().BeGreaterThanOrEqualTo(emailCount);

        }

        [Fact]
        public void Have_Empty_Phones_On_Create()
        {
            var organization = ContactableTestHelper.CreateOrganization();
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
            var organization = ContactableTestHelper.CreateOrganization();
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            organization.AddPhone(phone);

            organization.Phones.Should().Contain(phone);
        }

        [Fact]
        public void RemovePhone()
        {
            var organization = ContactableTestHelper.CreateOrganization();
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
        public void Return_Failure_On_Add_Null_Phone()
        {
            var organization = ContactableTestHelper.CreateOrganization();

            var result = organization.AddPhone(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Remove_Null_Phone()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var number = "989.627.9206";
            var phoneType = PhoneType.Mobile;
            var phone = Phone.Create(number, phoneType, true).Value;
            organization.AddPhone(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = Phone.Create(number, phoneType, false).Value;
            organization.AddPhone(phone);

            organization.Phones.Count.Should().Be(2);
            var result = organization.RemovePhone(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.RequiredMessage);
            organization.Phones.Count.Should().Be(2);
        }

        [Fact]
        public void Return_Failure_On_Remove_Nonexistent_Phone()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var number = "989.627.9206";
            var phoneType = PhoneType.Mobile;
            var phone = Phone.Create(number, phoneType, true).Value;
            organization.AddPhone(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = Phone.Create(number, phoneType, false).Value;

            organization.Phones.Count.Should().Be(1);
            var result = organization.RemovePhone(phone);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.NotFoundMessage);
            organization.Phones.Count.Should().Be(1);
        }

        [Fact]
        public void Return_Failure_On_Add_More_Than_One_Primary_Phone()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var number = "555.627.9206";
            var phone = Phone.Create(number, PhoneType.Home, true).Value;
            organization.AddPhone(phone);
            number = "444.627.9206";
            phone = Phone.Create(number, PhoneType.Mobile, true).Value;

            var result = organization.AddPhone(phone);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_Failure_On_Add_Duplicate_Phone()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            organization.AddPhone(phone);
            phone = Phone.Create(number, phoneType, true).Value;

            var result = organization.AddPhone(phone);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Have_Empty_Emails_On_Create()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            organization.Emails.Count.Should().Be(0);

            organization.AddEmail(email);
            organization.Emails.Count.Should().Be(1);
        }
        [Fact]

        public void AddEmail()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            organization.AddEmail(email);

            organization.Emails.Should().Contain(email);
        }

        [Fact]
        public void RemoveEmail()
        {
            var organization = ContactableTestHelper.CreateOrganization();
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
        public void Return_Failure_On_Remove_Null_Email()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var address = "jane@doe.com";
            var email0 = Email.Create(address, true).Value;
            organization.AddEmail(email0);
            address = "june@doe.com";
            var email1 = Email.Create(address, false).Value;
            organization.AddEmail(email1);

            organization.Emails.Count.Should().Be(2);
            var result = organization.RemoveEmail(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.RequiredMessage);
            organization.Emails.Count.Should().Be(2);
        }

        [Fact]
        public void Return_Failure_On_Add_Null_Email()
        {
            var organization = ContactableTestHelper.CreateOrganization();

            var result = organization.AddEmail(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Remove_Nonexistent_Email()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var address = "jane@doe.com";
            var email0 = Email.Create(address, true).Value;
            organization.AddEmail(email0);
            address = "june@doe.com";
            var email1 = Email.Create(address, false).Value;

            organization.Emails.Count.Should().Be(1);
            var result = organization.RemoveEmail(email1);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.NotFoundMessage);
            organization.Emails.Count.Should().Be(1);
        }

        [Fact]
        public void Return_Failure_On_Add_More_Than_One_Primary_Email()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            organization.AddEmail(email);
            address = "june@done.com";
            email = Email.Create(address, true).Value;

            var result = organization.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_Failure_On_Add_Duplicate_Email()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            organization.AddEmail(email);
            email = Email.Create(address, true).Value;

            var result = organization.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetAddress()
        {
            var organization = ContactableTestHelper.CreateOrganization();
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
        public void Return_Failure_On_Set_Inavlid_Address()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            Address address = null;

            var result = organization.SetAddress(address);
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.RequiredMessage);
        }

        [Fact]
        public void ClearAddress()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var address = Address.Create(
                "1234 Five Street",
                "Gaylord",
                State.MI,
                "49735").Value;
            organization.SetAddress(address);
            organization.Address.Should().Be(address);

            organization.ClearAddress();

            organization.Address.Should().BeNull();
        }

        [Fact]
        public void SetName()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var name = "jane's";
            var organizationNameOrError = OrganizationName.Create(name);

            organization.SetName(organizationNameOrError.Value);

            organization.Name.Should().Be(organizationNameOrError.Value);
        }

        [Fact]
        public void SetContact()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var firstName = "Jane";
            var lastName = "Doe";
            var personName = PersonName.Create(lastName, firstName).Value;
            var notes = LoremIpsum(100);
            var contact = Person.Create(personName, Gender.Female, notes).Value;

            organization.SetContact(contact);

            organization.Contact.Name.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void SetNotes()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var notes = "Behold, notes!";

            organization.SetNotes(notes);

            organization.Notes.Should().Be(notes);
        }

        [Fact]
        public void UpdateContactDetails()
        {
            var organization = new OrganizationFaker(true, emailsCount: 3, phonesCount: 3).Generate();

            var updatedAddress = new AddressFaker().Generate();

            var updatedPhones = organization.Phones.Select(phone =>
            {
                return new PhoneToWrite
                {
                    Id = phone.Id,
                    PhoneType = phone.PhoneType,
                    Number = Faker.Phone.PhoneNumber("##########"),
                    IsPrimary = phone.IsPrimary
                };
            }).ToList();

            var updatedEmails = organization.Emails.Select(email => new EmailToWrite
            {
                Id = email.Id,
                Address = $"updated-{email.Address}",
                IsPrimary = email.IsPrimary
            }).ToList();

            var updatedContactDetails = ContactDetailsFactory.Create(
                phonesToWrite: updatedPhones,
                emailsToWrite: updatedEmails,
                addressToWrite: AddressHelper.ConvertToWriteDto(updatedAddress)).Value;

            organization.Phones.Should().NotBeEquivalentTo(updatedPhones);
            organization.Emails.Should().NotBeEquivalentTo(updatedEmails);
            organization.Address.Should().NotBe(updatedAddress);

            organization.UpdateContactDetails(updatedContactDetails);

            organization.Phones.Should().BeEquivalentTo(updatedPhones);
            organization.Emails.Should().BeEquivalentTo(updatedEmails);
            organization.Address.Should().Be(updatedAddress);
        }

        [Fact]
        public void Truncate_Note_To_Note_Maximum_Length()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var notes = $"Lorem ipsum {LoremIpsum(Contactable.NoteMaximumLength)}";

            organization.SetNotes(notes);

            organization.Notes.Length.Should().Be(Contactable.NoteMaximumLength);
        }
    }
}