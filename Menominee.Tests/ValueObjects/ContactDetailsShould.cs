using Bogus;
using Menominee.Domain.BaseClasses;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using FluentAssertions;
using System.Linq;
using TestingHelperLibrary;
using Xunit;
using Menominee.Api.Features.Contactables;

namespace Menominee.Tests.ValueObjects
{
    public class ContactDetailsShould
    {
        private readonly Faker faker;

        public ContactDetailsShould()
        {
            faker = new Faker();
        }

        [Fact]
        public void Create_ContactDetails()
        {
            var phones = ContactableTestHelper.CreatePhones(5);
            var emails = ContactableTestHelper.CreateEmails(5);
            var address = ContactableTestHelper.CreateAddress();
            var phonesToWrite = PhoneHelper.ConvertToWriteDtos(phones);
            var emailsToWrite = EmailHelper.ConvertToWriteDtos(emails);
            var addressToWrite = AddressHelper.ConvertToWriteDto(address);

            var result = ContactDetailsFactory.Create(phonesToWrite, emailsToWrite, addressToWrite);

            result.Value.Phones.Should().BeEquivalentTo(phonesToWrite);
            result.Value.Emails.Should().BeEquivalentTo(emailsToWrite);
            result.Value.Address.Should().BeEquivalentTo(address);
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrderWarranty_With_Invalid_Phones()
        {
            var phones = ContactableTestHelper.CreatePhones(5);
            var emails = ContactableTestHelper.CreateEmails(5);
            var address = ContactableTestHelper.CreateAddress();
            var phonesToWrite = PhoneHelper.ConvertToWriteDtos(phones);
            var emailsToWrite = EmailHelper.ConvertToWriteDtos(emails);
            var addressToWrite = AddressHelper.ConvertToWriteDto(address);
            var invalidPhonesToWrite = phones.Select(
                phone => PhoneHelper.ConvertToWriteDto(phone)).ToList();
            foreach (var phone in invalidPhonesToWrite)
                phone.IsPrimary = true;

            var result = ContactDetailsFactory.Create(invalidPhonesToWrite, emailsToWrite, addressToWrite);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.PrimaryExistsMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrderWarranty_With_Invalid_Emails()
        {
            var phones = ContactableTestHelper.CreatePhones(5);
            var emails = ContactableTestHelper.CreateEmails(5);
            var address = ContactableTestHelper.CreateAddress();
            var phonesToWrite = PhoneHelper.ConvertToWriteDtos(phones);
            var addressToWrite = AddressHelper.ConvertToWriteDto(address);
            var invalidEmailsToWrite = emails.Select(
                email => EmailHelper.ConvertToWriteDto(email)).ToList();
            foreach (var email in invalidEmailsToWrite)
                email.IsPrimary = true;

            var result = ContactDetailsFactory.Create(phonesToWrite, invalidEmailsToWrite, addressToWrite);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Contactable.PrimaryExistsMessage);
        }

    }
}
