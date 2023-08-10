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
    public class BusinessShould
    {
        private static readonly Faker Faker = new();

        [Fact]
        public void Create_Business()
        {
            // Arrange
            var name = "   Jane's";

            // Act
            var businessOrError = Business.Create(BusinessName.Create(name).Value, null, null);

            // Assert
            businessOrError.Value.Should().BeOfType<Business>();
            businessOrError.IsFailure.Should().BeFalse();
            businessOrError.Value.Name.Name.Should().Be(name.Trim());
        }

        [Fact]
        public void Return_Failure_On_Create_Empty_Business()
        {
            var businessOrError = Business.Create(null, null, null);

            businessOrError.IsFailure.Should().BeTrue();
            businessOrError.Error.Should().Contain(Business.InvalidMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_Business_With_Null_Name()
        {
            string name = null;
            var businessNameOrError = BusinessName.Create(name);

            businessNameOrError.IsFailure.Should().BeTrue();
            businessNameOrError.Error.Should().Contain(BusinessName.InvalidMessage);
        }

        [Fact]
        public void Create_Business_With_Address()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";
            var address = Address.Create(addressLine, city, state, postalCode);

            business.SetAddress(address.Value);

            business.Address.AddressLine.Should().Be(addressLine);
            business.Address.City.Should().Be(city);
            business.Address.State.Should().Be(state);
            business.Address.PostalCode.Should().Be(postalCode);
        }

        [Fact]
        public void Create_Business_With_Contact()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var firstName = "Jane";
            var lastName = "Doe";
            var personName = PersonName.Create(lastName, firstName).Value;
            var notes = LoremIpsum(100);
            var contact = Person.Create(personName, Gender.Female, notes).Value;

            business.SetContact(contact);

            business.Name.Should().NotBeNull();
            business.Contact.Name.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void Create_Business_With_Phones()
        {
            var phoneCount = 10;
            var phones = ContactableTestHelper.CreatePhones(phoneCount);
            var business = Business.Create(
                BusinessName.Create(
                    "   Jane's").Value, "note", phones: phones)
                .Value;

            business.Phones.Count.Should().BeGreaterThanOrEqualTo(phoneCount);
        }

        [Fact]
        public void Create_Business_With_Emails()
        {
            var emailCount = 10;
            var emails = ContactableTestHelper.CreateEmails(emailCount);
            var business = Business.Create(BusinessName.Create("   Jane's").Value, "note", emails: emails).Value;

            business.Emails.Count.Should().BeGreaterThanOrEqualTo(emailCount);

        }

        [Fact]
        public void Have_Empty_Phones_On_Create()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            business.Phones.Count.Should().Be(0);

            business.AddPhone(phone);
            business.Phones.Count.Should().Be(1);
        }

        [Fact]
        public void AddPhone()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var number = "989.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            business.AddPhone(phone);

            business.Phones.Should().Contain(phone);
        }

        [Fact]
        public void RemovePhone()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var number = "989.627.9206";
            var phoneType = PhoneType.Mobile;
            var phone = Phone.Create(number, phoneType, true).Value;
            business.AddPhone(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = Phone.Create(number, phoneType, false).Value;
            business.AddPhone(phone);

            business.Phones.Count.Should().Be(2);
            business.RemovePhone(phone);

            business.Phones.Count.Should().Be(1);

        }

        [Fact]
        public void Return_Failure_On_Add_Null_Phone()
        {
            var business = ContactableTestHelper.CreateBusiness();

            var result = business.AddPhone(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Remove_Null_Phone()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var number = "989.627.9206";
            var phoneType = PhoneType.Mobile;
            var phone = Phone.Create(number, phoneType, true).Value;
            business.AddPhone(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = Phone.Create(number, phoneType, false).Value;
            business.AddPhone(phone);

            business.Phones.Count.Should().Be(2);
            var result = business.RemovePhone(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.RequiredMessage);
            business.Phones.Count.Should().Be(2);
        }

        [Fact]
        public void Return_Failure_On_Remove_Nonexistent_Phone()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var number = "989.627.9206";
            var phoneType = PhoneType.Mobile;
            var phone = Phone.Create(number, phoneType, true).Value;
            business.AddPhone(phone);
            number = "231.546.2102";
            phoneType = PhoneType.Home;
            phone = Phone.Create(number, phoneType, false).Value;

            business.Phones.Count.Should().Be(1);
            var result = business.RemovePhone(phone);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.NotFoundMessage);
            business.Phones.Count.Should().Be(1);
        }

        [Fact]
        public void Return_Failure_On_Add_More_Than_One_Primary_Phone()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var number = "555.627.9206";
            var phone = Phone.Create(number, PhoneType.Home, true).Value;
            business.AddPhone(phone);
            number = "444.627.9206";
            phone = Phone.Create(number, PhoneType.Mobile, true).Value;

            var result = business.AddPhone(phone);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_Failure_On_Add_Duplicate_Phone()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            business.AddPhone(phone);
            phone = Phone.Create(number, phoneType, true).Value;

            var result = business.AddPhone(phone);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Have_Empty_Emails_On_Create()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            business.Emails.Count.Should().Be(0);

            business.AddEmail(email);
            business.Emails.Count.Should().Be(1);
        }
        [Fact]

        public void AddEmail()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            business.AddEmail(email);

            business.Emails.Should().Contain(email);
        }

        [Fact]
        public void RemoveEmail()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var address = "jane@doe.com";
            var email0 = Email.Create(address, true).Value;
            business.AddEmail(email0);
            address = "june@doe.com";
            var email1 = Email.Create(address, false).Value;
            business.AddEmail(email1);

            business.Emails.Count.Should().Be(2);
            business.RemoveEmail(email0);

            business.Emails.Count.Should().Be(1);
            business.Emails.Should().Contain(email1);
        }

        [Fact]
        public void Return_Failure_On_Remove_Null_Email()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var address = "jane@doe.com";
            var email0 = Email.Create(address, true).Value;
            business.AddEmail(email0);
            address = "june@doe.com";
            var email1 = Email.Create(address, false).Value;
            business.AddEmail(email1);

            business.Emails.Count.Should().Be(2);
            var result = business.RemoveEmail(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.RequiredMessage);
            business.Emails.Count.Should().Be(2);
        }

        [Fact]
        public void Return_Failure_On_Add_Null_Email()
        {
            var business = ContactableTestHelper.CreateBusiness();

            var result = business.AddEmail(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Remove_Nonexistent_Email()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var address = "jane@doe.com";
            var email0 = Email.Create(address, true).Value;
            business.AddEmail(email0);
            address = "june@doe.com";
            var email1 = Email.Create(address, false).Value;

            business.Emails.Count.Should().Be(1);
            var result = business.RemoveEmail(email1);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.NotFoundMessage);
            business.Emails.Count.Should().Be(1);
        }

        [Fact]
        public void Return_Failure_On_Add_More_Than_One_Primary_Email()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            business.AddEmail(email);
            address = "june@done.com";
            email = Email.Create(address, true).Value;

            var result = business.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_Failure_On_Add_Duplicate_Email()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;

            business.AddEmail(email);
            email = Email.Create(address, true).Value;

            var result = business.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetAddress()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";
            var address = Address.Create(addressLine, city, state, postalCode);

            business.SetAddress(address.Value);

            using (new AssertionScope())
            {
                business.Address.AddressLine.Should().Be(addressLine);
                business.Address.City.Should().Be(city);
                business.Address.State.Should().Be(state);
                business.Address.PostalCode.Should().Be(postalCode);
            }
        }

        [Fact]
        public void Return_Failure_On_Set_Inavlid_Address()
        {
            var business = ContactableTestHelper.CreateBusiness();
            Address address = null;

            var result = business.SetAddress(address);
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.RequiredMessage);
        }

        [Fact]
        public void ClearAddress()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var address = Address.Create(
                "1234 Five Street",
                "Gaylord",
                State.MI,
                "49735").Value;
            business.SetAddress(address);
            business.Address.Should().Be(address);

            business.ClearAddress();

            business.Address.Should().BeNull();
        }

        [Fact]
        public void SetName()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var name = "jane's";
            var businessNameOrError = BusinessName.Create(name);

            business.SetName(businessNameOrError.Value);

            business.Name.Should().Be(businessNameOrError.Value);
        }

        [Fact]
        public void SetContact()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var firstName = "Jane";
            var lastName = "Doe";
            var personName = PersonName.Create(lastName, firstName).Value;
            var notes = LoremIpsum(100);
            var contact = Person.Create(personName, Gender.Female, notes).Value;

            business.SetContact(contact);

            business.Contact.Name.FirstName.Should().Be(firstName);
        }

        [Fact]
        public void SetNotes()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var notes = "Behold, notes!";

            business.SetNotes(notes);

            business.Notes.Should().Be(notes);
        }

        [Fact]
        public void UpdateContactDetails()
        {
            var business = new BusinessFaker(true, emailsCount: 3, phonesCount: 3).Generate();

            var updatedAddress = new AddressFaker().Generate();

            var updatedPhones = business.Phones.Select(phone =>
            {
                return new PhoneToWrite
                {
                    Id = phone.Id,
                    PhoneType = phone.PhoneType,
                    Number = Faker.Phone.PhoneNumber("##########"),
                    IsPrimary = phone.IsPrimary
                };
            }).ToList();

            var updatedEmails = business.Emails.Select(email => new EmailToWrite
            {
                Id = email.Id,
                Address = $"updated-{email.Address}",
                IsPrimary = email.IsPrimary
            }).ToList();

            var updatedContactDetails = ContactDetailsFactory.Create(
                phonesToWrite: updatedPhones,
                emailsToWrite: updatedEmails,
                addressToWrite: AddressHelper.ConvertToWriteDto(updatedAddress)).Value;

            business.Phones.Should().NotBeEquivalentTo(updatedPhones);
            business.Emails.Should().NotBeEquivalentTo(updatedEmails);
            business.Address.Should().NotBe(updatedAddress);

            business.UpdateContactDetails(updatedContactDetails);

            business.Phones.Should().BeEquivalentTo(updatedPhones);
            business.Emails.Should().BeEquivalentTo(updatedEmails);
            business.Address.Should().Be(updatedAddress);
        }

        [Fact]
        public void Truncate_Note_To_Note_Maximum_Length()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var notes = $"Lorem ipsum {LoremIpsum(Contactable.NoteMaximumLength)}";

            business.SetNotes(notes);

            business.Notes.Length.Should().Be(Contactable.NoteMaximumLength);
        }
    }
}