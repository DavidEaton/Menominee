using FluentAssertions;
using Menominee.Api.Data;
using Menominee.Api.Features.Contactables.Persons;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Persons;
using Menominee.Shared.Models.Persons.DriversLicenses;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using Xunit;

namespace Menominee.Tests.Validators.Domain
{
    public class PersonRequestValidatorShould
    {
        private readonly PersonRequestValidator validator;
        private readonly Mock<ApplicationDbContext> contextMock;
        public PersonRequestValidatorShould()
        {
            contextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            validator = new PersonRequestValidator(contextMock.Object);
        }

        [Fact]
        public void Validate_With_Valid_Person_Details()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Not_Validate_With_Empty_FirstName()
        {
            var person = new PersonToWrite
            {
                Name = new PersonNameToWrite { FirstName = "", LastName = "Doe" },
                Gender = Gender.Male,
                Birthday = new DateTime(1980, 1, 1),
                Notes = "Some notes"
            };

            var result = validator.Validate(person);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Not_Validate_With_Empty_LastName()
        {
            var person = new PersonToWrite
            {
                Name = new PersonNameToWrite { FirstName = "John", LastName = "" },
                Gender = Gender.Male,
                Birthday = new DateTime(1980, 1, 1),
                Notes = "Some notes"
            };

            var result = validator.Validate(person);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Not_Validate_With_Null_Name()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();
            personRequest.Name = null;

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_With_Null_Birthday()
        {
            var person = new PersonToWrite
            {
                Name = new PersonNameToWrite { FirstName = "John", LastName = "Doe" },
                Gender = Gender.Male,
                Birthday = null,
                Notes = "Some notes"
            };

            var result = validator.Validate(person);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Not_Validate_With_Future_Birthday()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();
            personRequest.Birthday = DateTime.Today.AddDays(1);

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Distant_Past_Birthday()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();
            personRequest.Birthday = DateTime.Today.AddYears(-150);

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Invalid_Address()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();
            personRequest.Address = new AddressToWrite { AddressLine1 = string.Empty, City = string.Empty };

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void NotValidate_WithAddressHavingPartialNullProperties()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();
            personRequest.Address.City = null;

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Invalid_DriversLicense()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();
            personRequest.DriversLicense = new DriversLicenseToWrite { Number = string.Empty };

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Invalid_DriversLicense_Dates()
        {
            var person = new PersonToWrite
            {
                DriversLicense = new DriversLicenseToWrite
                {
                    Issued = DateTime.Now,
                    Expiry = DateTime.Now.AddDays(-1)
                },
                Name = new PersonNameToWrite { FirstName = "John", LastName = "Doe" },
                Gender = Gender.Male,
                Notes = "Some notes"
            };

            var result = validator.Validate(person);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_With_Empty_Email_List()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();
            personRequest.Emails.Clear();

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Not_Validate_With_Empty_Email()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();
            personRequest.Emails.Add(new EmailToWrite());

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Emails_List_Containing_Null()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();
            personRequest.Emails.Add(null);

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Multiple_Primary_Emails()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();
            personRequest.Emails.Clear();
            personRequest.Emails.Add(new EmailToWrite { Address = "primary@example.com", IsPrimary = true });
            personRequest.Emails.Add(new EmailToWrite { Address = "secondPrimary@example.com", IsPrimary = true });

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_With_Empty_Phone_List()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();
            personRequest.Phones.Clear();

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Not_Validate_With_Empty_Phone()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();
            personRequest.Phones.Add(new PhoneToWrite());

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_With_Multiple_Primary_Phones()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();
            personRequest.Phones.Clear();
            personRequest.Phones.Add(new PhoneToWrite { Number = "123-456-7891", IsPrimary = true });
            personRequest.Phones.Add(new PhoneToWrite { Number = "987-456-7891", IsPrimary = true });

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeFalse();
        }


        [Fact]
        public void Not_Validate_With_Phones_List_Containing_Null()
        {
            var personRequest = TestDataFactory.CreatePersonRequest();
            personRequest.Phones.Add(null);

            var result = validator.Validate(personRequest);

            result.IsValid.Should().BeFalse();
        }
    }
}
