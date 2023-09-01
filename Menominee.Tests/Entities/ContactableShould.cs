using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Menominee.Domain.Entities;
using System.Linq;
using TestingHelperLibrary;
using TestingHelperLibrary.Payables;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class ContactableShould
    {
        // one for each {MethodName}, named Not_{MethodName}_With_Invalid/Null{PropertyName}

        [Fact]
        public void AddEmail()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var address = "m@m.m";
            var email = Email.Create(address, true).Value;

            var result = business.AddEmail(email);

            result.IsSuccess.Should().BeTrue();
            business.Emails.Should().Contain(email);
        }


        [Fact]
        public void Not_Add_Null_Email()
        {
            var business = ContactableTestHelper.CreateBusiness();

            var result = business.AddEmail(null);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Remove_Null_Email()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var address = "jane@doe.com";
            var email = Email.Create(address, true).Value;
            business.AddEmail(email);

            var result = business.RemoveEmail(null);

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
            var business = ContactableTestHelper.CreateBusiness();
            var address = "m@m.m";
            var email = Email.Create(address, true).Value;
            var result = business.AddEmail(email);
            result.IsSuccess.Should().BeTrue();
            business.Emails.Should().Contain(email);

            var removeResult = business.RemoveEmail(email);
            removeResult.IsFailure.Should().BeFalse();
            var emailWasRemoved = !business.Emails.Contains(email);

            emailWasRemoved.Should().BeTrue();
        }

        [Fact]
        public void AddPhone()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;

            var result = business.AddPhone(phone);

            result.IsSuccess.Should().BeTrue();
            business.Phones.Should().Contain(phone);
        }

        [Fact]
        public void Not_Add_Null_Phone()
        {
            var business = ContactableTestHelper.CreateBusiness();

            var result = business.AddPhone(null);

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
            var business = ContactableTestHelper.CreateBusiness();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            var result = business.AddPhone(phone);
            result.IsSuccess.Should().BeTrue();
            business.Phones.Should().Contain(phone);

            var removeResult = business.RemovePhone(phone);
            removeResult.IsFailure.Should().BeFalse();
            var phoneWasRemoved = !business.Phones.Contains(phone);

            phoneWasRemoved.Should().BeTrue();
        }

        [Fact]
        public void Not_Remove_Null_Phone()
        {
            var business = ContactableTestHelper.CreateBusiness();
            var number = "555.444.3333";
            var phoneType = PhoneType.Home;
            var phone = Phone.Create(number, phoneType, true).Value;
            business.AddPhone(phone);

            var result = business.RemovePhone(null);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetAddress()
        {
            var addressLine1 = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";
            var addressOrError = Address.Create(addressLine1, city, state, postalCode);
            var business = ContactableTestHelper.CreateBusiness();

            business.SetAddress(addressOrError.Value);
            var customerOrError = Customer.Create(business, CustomerType.Retail, null).Value;
            var janes = customerOrError.Business;

            customerOrError.Should().BeOfType<Customer>();
            customerOrError.EntityType.Should().Be(EntityType.Business);
            janes.Address.AddressLine1.Should().Be(addressLine1);
            janes.Address.City.Should().Be(city);
            janes.Address.State.Should().Be(state);
            janes.Address.PostalCode.Should().Be(postalCode);
        }

        [Fact]
        public void ClearAddress()
        {
            var addressLine1 = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";
            var addressOrError = Address.Create(addressLine1, city, state, postalCode);
            var business = ContactableTestHelper.CreateBusiness();
            business.SetAddress(addressOrError.Value);
            var customerOrError = Customer.Create(business, CustomerType.Retail, null).Value;
            var janes = customerOrError.Business;
            customerOrError.Should().BeOfType<Customer>();
            customerOrError.EntityType.Should().Be(EntityType.Business);
            janes.Address.AddressLine1.Should().Be(addressLine1);
            janes.Address.City.Should().Be(city);
            janes.Address.State.Should().Be(state);
            janes.Address.PostalCode.Should().Be(postalCode);

            business.ClearAddress();

            business.Address.Should().BeNull();
            janes.Address.Should().BeNull();
        }
    }
}
