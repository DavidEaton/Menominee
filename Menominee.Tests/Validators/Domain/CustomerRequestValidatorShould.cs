using FluentAssertions;
using Menominee.Api.Data;
using Menominee.Api.Features.Customers;
using Menominee.Common.Enums;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Menominee.Tests.Validators.Domain
{
    public class CustomerRequestValidatorShould
    {
        private readonly CustomerRequestValidator validator;
        private readonly Mock<ApplicationDbContext> contextMock;

        public CustomerRequestValidatorShould()
        {
            contextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            validator = new CustomerRequestValidator(contextMock.Object);
        }

        [Fact]
        public void Validate_With_Valid_PersonRequest()
        {
            var customer = new CustomerToWrite
            {
                EntityType = EntityType.Person,
                CustomerType = CustomerType.Retail,
                Person = TestDataFactory.CreatePersonRequest(),
                Code = "12345",
                Vehicles = TestDataFactory.CreateVehiclesList()
            };

            var result = validator.Validate(customer);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_With_Valid_Business_Entity()
        {
            var customer = new CustomerToWrite
            {
                Business = TestDataFactory.CreateBusinessRequest(),
                EntityType = EntityType.Business,
                CustomerType = CustomerType.Retail,
                Code = "12345",
                Vehicles = TestDataFactory.CreateVehiclesList()
            };

            var result = validator.Validate(customer);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(EntityType.Person)]
        [InlineData(EntityType.Business)]
        public void Validate_With_Valid_EntityType(EntityType entityType)
        {
            var customer = TestDataFactory.CreateCustomerRequest(entityType);
            var result = validator.Validate(customer);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(EntityType.Person, false)]
        [InlineData(EntityType.Business, false)]
        public void Not_Validate_When_Details_Are_Invalid(EntityType entityType, bool validDetails)
        {
            var customer = TestDataFactory.CreateCustomerRequest(entityType, validDetails);
            var result = validator.Validate(customer);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_When_EntityType_Is_Invalid()
        {
            var customer = new CustomerToWrite
            {
                EntityType = (EntityType)999,
                Person = TestDataFactory.CreatePersonRequest(),
                CustomerType = CustomerType.Retail,
                Code = "12345",
            };

            var result = validator.Validate(customer);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_When_CustomerType_Is_Invalid()
        {
            var customer = new CustomerToWrite
            {
                EntityType = EntityType.Person,
                Person = TestDataFactory.CreatePersonRequest(),
                CustomerType = (CustomerType)999,
                Code = "12345",
            };

            var result = validator.Validate(customer);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_When_Person_Details_Are_Invalid()
        {
            var customer = new CustomerToWrite
            {
                EntityType = EntityType.Person,
                CustomerType = CustomerType.Retail,
                Person = TestDataFactory.CreatePersonRequest(invalid: true),
                Code = "12345"
            };

            var result = validator.Validate(customer);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_When_Business_Details_Are_Invalid()
        {
            var customer = new CustomerToWrite
            {
                EntityType = EntityType.Business,
                CustomerType = CustomerType.Retail,
                Code = "12345",
                Business = TestDataFactory.CreateBusinessRequest(invalid: true)
            };

            var result = validator.Validate(customer);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_When_Code_Exceeds_Maximum_Length()
        {
            var customer = new CustomerToWrite
            {
                EntityType = EntityType.Person,
                Person = TestDataFactory.CreatePersonRequest(),
                CustomerType = CustomerType.Retail,
                Code = new string('A', Customer.MaximumCodeLength + 1)
            };

            var result = validator.Validate(customer);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Not_Validate_When_Vehicles_List_Has_Invalid_Vehicle()
        {
            var customer = new CustomerToWrite
            {
                EntityType = EntityType.Person,
                Person = TestDataFactory.CreatePersonRequest(),
                CustomerType = CustomerType.Retail,
                Code = "12345",
                Vehicles = TestDataFactory.CreateVehiclesList(invalid: true)
            };

            var result = validator.Validate(customer);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_With_Empty_Vehicles_List()
        {
            var customer = new CustomerToWrite
            {
                EntityType = EntityType.Person,
                Person = TestDataFactory.CreatePersonRequest(),
                CustomerType = CustomerType.Retail,
                Code = "12345",
                Vehicles = new List<VehicleToWrite> { }
            };

            var result = validator.Validate(customer);
            result.IsValid.Should().BeTrue();
        }
    }
}