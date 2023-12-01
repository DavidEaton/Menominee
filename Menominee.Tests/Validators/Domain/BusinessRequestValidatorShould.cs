using FluentAssertions;
using Menominee.Api.Data;
using Menominee.Api.Features.Contactables.Businesses;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Contactable;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Menominee.Tests.Validators.Domain
{
    public class BusinessRequestValidatorShould
    {
        private readonly BusinessRequestValidator validator;
        private readonly Mock<ApplicationDbContext> contextMock;

        public BusinessRequestValidatorShould()
        {
            contextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            validator = new BusinessRequestValidator(contextMock.Object);
        }

        [Fact]
        public void Validate_With_Valid_Business_Details()
        {
            var businessRequest = TestDataFactory.CreateBusinessRequest();

            var result = validator.Validate(businessRequest);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Not_Validate_With_Empty_Name()
        {
            var businessRequest = new BusinessToWrite
            {
                Name = new BusinessNameRequest { Name = "" },
                Notes = "Some notes"
            };

            var result = validator.Validate(businessRequest);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Not_Validate_With_Null_Name()
        {
            var businessRequest = TestDataFactory.CreateBusinessRequest();
            businessRequest.Name = null;

            var result = validator.Validate(businessRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Invalid_Address()
        {
            var businessRequest = TestDataFactory.CreateBusinessRequest();
            businessRequest.Address = new AddressToWrite { AddressLine1 = string.Empty, City = string.Empty };

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
