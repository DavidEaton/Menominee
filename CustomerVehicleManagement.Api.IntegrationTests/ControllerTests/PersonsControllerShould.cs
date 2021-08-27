using CustomerVehicleManagement.Api.Persons;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;

namespace CustomerVehicleManagement.Api.IntegrationTests.Controllers
{
    public class PersonsControllerShould
    {
        private readonly PersonsController controller;
        private readonly Mock<IPersonRepository> moqRepository;

        public PersonsControllerShould()
        {
            moqRepository = new Mock<IPersonRepository>();
            controller = new PersonsController(moqRepository.Object);
        }

        #region ********************************Get***********************************

        [Fact]
        public async Task Return_ActionResult_Of_PersonReadDto_On_GetPersonAsync()
        {
            var result = await controller.GetPersonAsync(1);

            result.Result.Should().BeOfType<NotFoundResult>();
            result.Should().BeOfType<ActionResult<PersonReadDto>>();
        }

        [Fact]
        public async Task Return_NotFoundResult_On_GetPersonAsyncWithInvalidId()
        {
            var result = await controller.GetPersonAsync(0);

            result.Should().BeOfType<ActionResult<PersonReadDto>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_IEnumerable_Of_PersonReadDto_On_GetPersonsAsync()
        {
            var result = await controller.GetPersonsAsync();

            result.Should().BeOfType<ActionResult<IReadOnlyList<PersonReadDto>>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_IEnumerable_Of_PersonReadDto_On_GetPersonsListAsync()
        {
            var result = await controller.GetPersonsListAsync();

            result.Should().BeOfType<ActionResult<IReadOnlyList<PersonInListDto>>>();
        }

        #endregion Get

        #region ********************************Post**********************************

        [Fact]
        public async Task Return_ActionResult_Of_PersonReadDto_On_CreatePersonAsync()
        {
            var person = new PersonCreateDto(PersonName.Create("Doe", "Jane").Value, Gender.Female);

            var result = await controller.CreatePersonAsync(person);

            result.Should().BeOfType<ActionResult<PersonReadDto>>();
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_CreatePersonAsync_When_ModelState_Invalid()
        {
            controller.ModelState.AddModelError("x", "Test Error Message");
            var person = new PersonCreateDto(PersonName.Create("Doe", "Jane").Value, Gender.Female);

            var result = await controller.CreatePersonAsync(person);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
            moqRepository.Verify(x => x.AddPersonAsync(It.IsAny<Person>()), Times.Never);
        }

        [Fact]
        public async Task Not_Save_On_CreatePersonAsync_When_ModelState_Invalid()
        {
            controller.ModelState.AddModelError("x", "Test Error Message");
            var person = new PersonCreateDto(PersonName.Create("Doe", "Jane").Value, Gender.Female);

            var result = await controller.CreatePersonAsync(person);

            moqRepository.Verify(x => x.AddPersonAsync(It.IsAny<Person>()), Times.Never);
        }

        [Fact]
        public async Task Save_On_CreatePersonAsync_When_ModelState_Valid()
        {
            Person savedPerson = null;

            moqRepository.Setup(x => x.AddPersonAsync(It.IsAny<Person>()))
                          .Returns(Task.CompletedTask)
                          .Callback<Person>(x => savedPerson = x);

            var person = new PersonCreateDto(PersonName.Create("Doe", "Jane").Value, Gender.Female);

            var result = await controller.CreatePersonAsync(person);

            moqRepository.Verify(x => x.AddPersonAsync(It.IsAny<Person>()), Times.Once);
            person.Name.Should().Be(savedPerson.Name);
            person.Gender.Should().Be(savedPerson.Gender);
            person.Birthday.Should().Be(savedPerson.Birthday);
        }

        [Fact]
        public async Task Return_PersonReadDto_On_CreatePersonAsync_When_ModelState_Valid()
        {
            moqRepository.Setup(x => x.AddPersonAsync(It.IsAny<Person>()));

            var person = new PersonCreateDto(PersonName.Create("Doe", "Jane").Value, Gender.Female);
            var result = await controller.CreatePersonAsync(person);

            result.Should().BeOfType<ActionResult<PersonReadDto>>();
        }

        #endregion Post

        #region ********************************Put***********************************

        [Fact]
        public async Task Return_NotFoundObjectResult_On_UpdatePersonAsync_With_Invalid_Id()
        {
            var invaldId = 0;
            var person = new PersonUpdateDto
            {
                Gender = Gender.Female
            };

            var result = await controller.UpdatePersonAsync(invaldId, person);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        //[Fact]
        //public async Task Return_NoContent_On_UpdatePersonAsync()
        //{
        //}
        #endregion Put
    }
}
