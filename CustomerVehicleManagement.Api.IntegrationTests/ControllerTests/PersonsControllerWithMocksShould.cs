using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using CustomerVehicleManagement.Shared.Models.Persons;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.ControllerTests
{
    public class PersonsControllerWithMocksShould
    {
        private readonly PersonsController controller;
        private readonly Mock<IPersonRepository> moqRepository;

        public PersonsControllerWithMocksShould()
        {
            moqRepository = new Mock<IPersonRepository>();
            controller = new PersonsController(moqRepository.Object);
        }

        #region ********************************Get***********************************

        [Fact]
        public async Task Return_ActionResult_Of_PersonToRead_On_GetPersonAsync()
        {
            var result = await controller.GetPersonAsync(1);

            result.Should().BeOfType<ActionResult<PersonToRead>>();
        }

        [Fact]
        public async Task Return_NotFoundResult_On_GetPersonAsyncWithInvalidId()
        {
            var result = await controller.GetPersonAsync(0);

            result.Result.Should().BeOfType<NotFoundResult>();
            result.Should().BeOfType<ActionResult<PersonToRead>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_IEnumerable_Of_PersonToRead_On_GetPersonsAsync()
        {
            var result = await controller.GetPersonsAsync();

            result.Should().BeOfType<ActionResult<IReadOnlyList<PersonToRead>>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_IEnumerable_Of_PersonToRead_On_GetPersonsListAsync()
        {
            var result = await controller.GetPersonsListAsync();

            result.Should().BeOfType<ActionResult<IReadOnlyList<PersonToReadInList>>>();
        }

        #endregion Get

        #region ********************************Post**********************************

        [Fact]
        public async Task Return_ActionResult_Of_PersonToRead_On_CreatePersonAsync()
        {
            var person = new PersonToWrite()
            {
                Name = new PersonNameToWrite()
                {
                    LastName = "Doe",
                    FirstName = "Jane"
                },
                Gender = Gender.Female
            };

            var result = await controller.AddPersonAsync(person);

            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task Save_On_CreatePersonAsync_When_ModelState_Valid()
        {
            Person savedPerson = null;

            moqRepository.Setup(repo => repo.AddPersonAsync(It.IsAny<Person>()))
                          .Returns(Task.CompletedTask)
                          .Callback<Person>(person => savedPerson = person);

            var person = new PersonToWrite()
            {
                Name = new PersonNameToWrite()
                {
                    LastName = "Doe",
                    FirstName = "Jane"
                },
                Gender = Gender.Female
            };

            var result = await controller.AddPersonAsync(person);

            moqRepository.Verify(repository => repository.AddPersonAsync(It.IsAny<Person>()), Times.Once);
            person.Name.FirstMiddleLast.Should().Be(savedPerson.Name.FirstMiddleLast);
            person.Gender.Should().Be(savedPerson.Gender);
            person.Birthday.Should().Be(savedPerson.Birthday);
        }

        [Fact]
        public async Task Return_PersonToRead_On_CreatePersonAsync_When_ModelState_Valid()
        {
            moqRepository.Setup(repository => repository.AddPersonAsync(It.IsAny<Person>()));

            var person = new PersonToWrite()
            {
                Name = new PersonNameToWrite()
                {
                    LastName = "Doe",
                    FirstName = "Jane"
                },
                Gender = Gender.Female
            };
            var result = await controller.AddPersonAsync(person);

            result.Should().BeOfType<CreatedResult>();
        }

        #endregion Post

        #region ********************************Put***********************************

        [Fact]
        public async Task Return_NotFoundObjectResult_On_UpdatePersonAsync_With_Invalid_Id()
        {
            var invaldId = 0;
            var person = new PersonToWrite()
            {
                Name = new PersonNameToWrite()
                {
                    LastName = "Doe",
                    FirstName = "Jane"
                },
                Gender = Gender.Female
            };

            var result = await controller.UpdatePersonAsync(invaldId, person);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        #endregion Put
    }
}
