using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Xunit;
using static CustomerVehicleManagement.Tests.Utilities;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class ContactableShould
    {
        // Emails
        // AddEmail
        // Not_Add_Null_Phone
        // Not_Add_Duplicate_Phone()


        // RemoveEmail
        [Fact]
        public void Not_Remove_Null_Phone()
        {
            var organization = CreateTestOrganization();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            organization.AddPhone(phone);

            var result = organization.RemovePhone(null);

            result.IsFailure.Should().BeTrue();
        }

        // Phones
        [Fact]
        public void AddPhone()
        {
            var organization = CreateTestOrganization();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            var result = organization.AddPhone(phone);

            result.IsSuccess.Should().BeTrue();
            organization.Phones.Should().Contain(phone);
        }

        [Fact]
        public void Not_Add_Null_Phone()
        {
            var organization = CreateTestOrganization();

            var result = organization.AddPhone(null);

            result.IsFailure.Should().BeTrue();
        }
        // Not_Add_Duplicate_Phone()


        // RemovePhone


        [Fact]
        public void SetAddress()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";
            var addressOrError = Address.Create(addressLine, city, state, postalCode);
            var organization = CreateTestOrganization();

            organization.SetAddress(addressOrError.Value);
            var customerOrError = new Customer(organization, CustomerType.Retail);
            var janes = customerOrError.Organization;

            customerOrError.Should().BeOfType<Customer>();
            customerOrError.EntityType.Should().Be(EntityType.Organization);
            janes.Address.AddressLine.Should().Be(addressLine);
            janes.Address.City.Should().Be(city);
            janes.Address.State.Should().Be(state);
            janes.Address.PostalCode.Should().Be(postalCode);
        }

        // ClearAddress

        [Fact]
        public void Not_Add_Duplicate_Phone()
        {
            var vendor = CreateVendor();
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            vendor.AddPhone(phone);
            phone = Phone.Create(number, phoneType, true).Value;

            var result = vendor.AddPhone(phone);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_RemoveEmail_With_Null_Email()
        {
            var organization = CreateTestOrganization();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            organization.AddEmail(email);

            var result = organization.RemoveEmail(null);

            result.IsFailure.Should().BeTrue();
        }

        // one for each {MethodName}, named Not_{MethodName}_With_Invalid/Null{PropertyName}
    }
}
