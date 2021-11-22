using CustomerVehicleManagement.Api.Customers;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Menominee.Common.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.ControllerTests
{
    /// <summary>
    /// These tests use mocks, no database and no data
    /// </summary>
    public class CustomersControllerWithMocksShould
    {
        private readonly CustomersController controller;
        private readonly Mock<IPersonRepository> moqPersonRepository;
        private readonly Mock<IOrganizationRepository> moqOrganizationRepository;
        private readonly Mock<ICustomerRepository> moqCustomerRepository;

        public CustomersControllerWithMocksShould()
        {
            moqPersonRepository = new Mock<IPersonRepository>();
            moqOrganizationRepository = new Mock<IOrganizationRepository>();
            moqCustomerRepository = new Mock<ICustomerRepository>();

            controller = new CustomersController(moqCustomerRepository.Object, moqPersonRepository.Object, moqOrganizationRepository.Object);
        }

        [Fact]
        public async Task Return_ActionResult_Of_CustomerToRead_On_GetCustomerAsync()
        {
            var result = await controller.GetCustomerAsync(10000);

            result.Result.Should().BeOfType<NotFoundResult>();
            result.Should().BeOfType<ActionResult<CustomerToRead>>();
        }

        [Fact]
        public async Task Return_NotFoundResult_On_GetCustomerAsyncWithInvalidId()
        {
            var result = await controller.GetCustomerAsync(0);

            result.Result.Should().BeOfType<NotFoundResult>();
            result.Should().BeOfType<ActionResult<CustomerToRead>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_IReadOnlyList_Of_CustomerToRead_On_GetCustomersAsync()
        {
            var result = await controller.GetCustomersAsync();

            result.Result.Should().BeOfType<NotFoundResult>();
            result.Should().BeOfType<ActionResult<IReadOnlyList<CustomerToRead>>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_CustomerToRead_On_CreateCustomerAsync()
        {
            var person = new PersonToAdd { Name = new PersonNameToAdd { LastName = "Doe", FirstName = "Jane" }, Gender = Gender.Female };
            CustomerToAdd customerCreateDto = new(person, null, CustomerType.Retail);

            var result = await controller.CreateCustomerAsync(customerCreateDto);

            result.Should().BeOfType<ActionResult<CustomerToRead>>();
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_CreateCustomerAsync_When_ModelState_Invalid()
        {
            controller.ModelState.AddModelError("x", "Test Error Message");
            var person = new PersonToAdd { Name = new PersonNameToAdd { LastName = "Doe", FirstName = "Jane" }, Gender = Gender.Female };
            CustomerToAdd customerCreateDto = new(person, null, CustomerType.Retail);

            var result = await controller.CreateCustomerAsync(customerCreateDto);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Not_Save_On_CreateCustomerAsync_When_ModelState_Invalid()
        {
            controller.ModelState.AddModelError("x", "Test Error Message");
            var person = new PersonToAdd { Name = new PersonNameToAdd { LastName = "Doe", FirstName = "Jane" }, Gender = Gender.Female };
            CustomerToAdd customerCreateDto = new(person, null, CustomerType.Retail);

            var result = await controller.CreateCustomerAsync(customerCreateDto);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Save_On_CreateCustomerAsync_When_ModelState_Valid()
        {
            var person = new PersonToAdd { Name = new PersonNameToAdd { LastName = "Doe", FirstName = "Jane" }, Gender = Gender.Female };
            CustomerToAdd customerCreateDto = new(person, null, CustomerType.Retail);

            var result = await controller.CreateCustomerAsync(customerCreateDto);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Return_CustomerToRead_On_CreateCustomerAsync_When_ModelState_Valid()
        {
            moqCustomerRepository.Setup(repo => repo.AddCustomerAsync(It.IsAny<Customer>()));

            var person = new PersonToAdd { Name = new PersonNameToAdd { LastName = "Doe", FirstName = "Jane" }, Gender = Gender.Female };
            CustomerToAdd customerCreateDto = new(person, null, CustomerType.Retail);

            var result = await controller.CreateCustomerAsync(customerCreateDto);

            result.Should().BeOfType<ActionResult<CustomerToRead>>();
        }

        [Fact]
        public async Task Return_NotFoundObjectResult_On_UpdateCustomerAsync_With_Invalid_Id()
        {
            var invalidId = 0;

            CustomerToEdit customer = new()
            {
                CustomerType = CustomerType.Retail
            };

            var result = await controller.UpdateCustomerAsync(invalidId, customer);

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
