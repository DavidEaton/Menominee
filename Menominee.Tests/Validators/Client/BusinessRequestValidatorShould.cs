using FluentAssertions;
using Menominee.Client.Features.Contactables.Businesses;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Contactable;
using Menominee.TestingHelperLibrary;
using Xunit;

namespace Menominee.Tests.Validators.Client
{
    public class BusinessRequestValidatorShould
    {
        private readonly BusinessRequestValidator validator;

        public BusinessRequestValidatorShould()
        {
            validator = new BusinessRequestValidator();
        }

        [Fact]
        public void Validate_With_Valid_Business_Details()
        {
            var businessRequest = TestDataFactory.CreateBusinessRequest();

            var result = validator.Validate(businessRequest);

            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("B")]
        [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis,.")]
        public void Not_Validate_With_Invalid_Name(string name)
        {
            var request = new BusinessToWrite
            {
                Name = new BusinessNameRequest { Name = name },
                Notes = "Some notes"
            };

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Invalid_Address()
        {
            var businessRequest = TestDataFactory.CreateBusinessRequest();
            businessRequest.Address = new AddressToWrite { AddressLine1 = "1234 Five", City = string.Empty };

            var result = validator.Validate(businessRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void NotValidate_With_Address_Having_Invalid_Properties()
        {
            var businessRequest = TestDataFactory.CreateBusinessRequest();
            businessRequest.Address.City = null;

            var result = validator.Validate(businessRequest);

            result.IsValid.Should().BeFalse();
        }


        [Fact]
        public void Validate_With_Empty_Email_List()
        {
            var businessRequest = TestDataFactory.CreateBusinessRequest();
            businessRequest.Emails.Clear();

            var result = validator.Validate(businessRequest);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Not_Validate_With_Empty_Email()
        {
            var businessRequest = TestDataFactory.CreateBusinessRequest();
            businessRequest.Emails.Add(new EmailToWrite());

            var result = validator.Validate(businessRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Emails_List_Containing_Null()
        {
            var businessRequest = TestDataFactory.CreateBusinessRequest();
            businessRequest.Emails.Add(null);

            var result = validator.Validate(businessRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Multiple_Primary_Emails()
        {
            var businessRequest = TestDataFactory.CreateBusinessRequest();
            businessRequest.Emails.Clear();
            businessRequest.Emails.Add(new EmailToWrite { Address = "primary@example.com", IsPrimary = true });
            businessRequest.Emails.Add(new EmailToWrite { Address = "secondPrimary@example.com", IsPrimary = true });

            var result = validator.Validate(businessRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_With_Empty_Phone_List()
        {
            var businessRequest = TestDataFactory.CreateBusinessRequest();
            businessRequest.Phones.Clear();

            var result = validator.Validate(businessRequest);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Not_Validate_With_Empty_Phone()
        {
            var businessRequest = TestDataFactory.CreateBusinessRequest();
            businessRequest.Phones.Add(new PhoneToWrite());

            var result = validator.Validate(businessRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Multiple_Primary_Phones()
        {
            var businessRequest = TestDataFactory.CreateBusinessRequest();
            businessRequest.Phones.Clear();
            businessRequest.Phones.Add(new PhoneToWrite { Number = "123-456-7891", IsPrimary = true });
            businessRequest.Phones.Add(new PhoneToWrite { Number = "987-456-7891", IsPrimary = true });

            var result = validator.Validate(businessRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Phones_List_Containing_Null()
        {
            var businessRequest = TestDataFactory.CreateBusinessRequest();
            businessRequest.Phones.Add(null);

            var result = validator.Validate(businessRequest);

            result.IsValid.Should().BeFalse();
        }
    }

}
