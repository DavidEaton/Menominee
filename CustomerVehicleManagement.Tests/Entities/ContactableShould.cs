using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System.Linq;
using TestingHelperLibrary;
using TestingHelperLibrary.Payables;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class ContactableShould
    {
        // one for each {MethodName}, named Not_{MethodName}_With_Invalid/Null{PropertyName}

        [Fact]
        public void AddEmail()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var address = "m@m.m";
            var email = Email.Create(address, true).Value;

            var result = organization.AddEmail(email);

            result.IsSuccess.Should().BeTrue();
            organization.Emails.Should().Contain(email);
        }


        [Fact]
        public void Not_Add_Null_Email()
        {
            var organization = ContactableTestHelper.CreateOrganization();

            var result = organization.AddEmail(null);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Remove_Null_Email()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            organization.AddEmail(email);

            var result = organization.RemoveEmail(null);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Add_Duplicate_Email()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var address = "m@m.m";
            var email = Email.Create(address, true).Value;
            vendor.AddEmail(email);
            email = Email.Create(address, true).Value;

            var result = vendor.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Add_More_Tha_One_Primary_Email()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var address = "m@m.m";
            var email = Email.Create(address, true).Value;
            vendor.AddEmail(email);
            email = Email.Create(address, true).Value;

            var result = vendor.AddEmail(email);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void RemoveEmail()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var address = "m@m.m";
            var email = Email.Create(address, true).Value;
            var result = organization.AddEmail(email);
            result.IsSuccess.Should().BeTrue();
            organization.Emails.Should().Contain(email);

            var removeResult = organization.RemoveEmail(email);
            removeResult.IsFailure.Should().BeFalse();
            var emailWasRemoved = !organization.Emails.Contains(email);

            emailWasRemoved.Should().BeTrue();
        }

        [Fact]
        public void AddPhone()
        {
            var organization = ContactableTestHelper.CreateOrganization();
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
            var organization = ContactableTestHelper.CreateOrganization();

            var result = organization.AddPhone(null);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Add_Duplicate_Phone()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var number = "555.627.9206";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            vendor.AddPhone(phone);
            phone = Phone.Create(number, phoneType, true).Value;

            var result = vendor.AddPhone(phone);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Add_More_Tha_One_Primary_Phone()
        {
            var vendor = VendorTestHelper.CreateVendor();
            var number = "555.627.9206";
            var phone = Phone.Create(number, PhoneType.Home, true).Value;
            vendor.AddPhone(phone);
            phone = Phone.Create(number, PhoneType.Mobile, true).Value;

            var result = vendor.AddPhone(phone);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void RemovePhone()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            var result = organization.AddPhone(phone);
            result.IsSuccess.Should().BeTrue();
            organization.Phones.Should().Contain(phone);

            var removeResult = organization.RemovePhone(phone);
            removeResult.IsFailure.Should().BeFalse();
            var phoneWasRemoved = !organization.Phones.Contains(phone);

            phoneWasRemoved.Should().BeTrue();
        }

        [Fact]
        public void Not_Remove_Null_Phone()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            organization.AddPhone(phone);

            var result = organization.RemovePhone(null);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetAddress()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";
            var addressOrError = Address.Create(addressLine, city, state, postalCode);
            var organization = ContactableTestHelper.CreateOrganization();

            organization.SetAddress(addressOrError.Value);
            var customerOrError = Customer.Create(organization, CustomerType.Retail).Value;
            var janes = customerOrError.Organization;

            customerOrError.Should().BeOfType<Customer>();
            customerOrError.EntityType.Should().Be(EntityType.Organization);
            janes.Address.AddressLine.Should().Be(addressLine);
            janes.Address.City.Should().Be(city);
            janes.Address.State.Should().Be(state);
            janes.Address.PostalCode.Should().Be(postalCode);
        }

        [Fact]
        public void ClearAddress()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";
            var addressOrError = Address.Create(addressLine, city, state, postalCode);
            var organization = ContactableTestHelper.CreateOrganization();
            organization.SetAddress(addressOrError.Value);
            var customerOrError = Customer.Create(organization, CustomerType.Retail).Value;
            var janes = customerOrError.Organization;
            customerOrError.Should().BeOfType<Customer>();
            customerOrError.EntityType.Should().Be(EntityType.Organization);
            janes.Address.AddressLine.Should().Be(addressLine);
            janes.Address.City.Should().Be(city);
            janes.Address.State.Should().Be(state);
            janes.Address.PostalCode.Should().Be(postalCode);

            organization.ClearAddress();

            organization.Address.Should().BeNull();
            janes.Address.Should().BeNull();
        }
    }
}
