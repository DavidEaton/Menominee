using CustomerVehicleManagement.Api.Common;
using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using FluentAssertions;
using TestingHelperLibrary;
using Xunit;

namespace CustomerVehicleManagement.Tests.ValueObjects
{
    public class ContactDetailsTests
    {
        [Fact]
        public void Can_Create_ContactDetails()
        {
            var phones = ContactableTestHelper.CreatePhones(5);
            var emails = ContactableTestHelper.CreateEmails(5);
            var address = ContactableTestHelper.CreateAddress();

            var phonesToWrite = PhoneHelper.ConvertEntitiesToWriteDtos(phones);
            var emailsToWrite = EmailHelper.ConvertEntitiesToWriteDtos(emails);
            var addressToWrite = AddressHelper.ConvertEntityToWriteDto(address);

            var details = ContactDetailsFactory.Create(phonesToWrite, emailsToWrite, addressToWrite);

            details.Phones.Should().BeEquivalentTo(phonesToWrite);
            details.Emails.Should().BeEquivalentTo(emailsToWrite);
            details.Address.Should().BeEquivalentTo(address);
        }

    }
}
