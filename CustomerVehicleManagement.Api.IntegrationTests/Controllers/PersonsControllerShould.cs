using AutoMapper;
using CustomerVehicleManagement.Api.Phones;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.Emails;

namespace CustomerVehicleManagement.Api.IntegrationTests.Controllers
{
    public class PersonsControllerShould
    {
        private readonly IMapper mapper;
        private readonly PersonsController controller;
        private readonly Mock<IPersonRepository> moqRepository;

        public PersonsControllerShould()
        {
            moqRepository = new Mock<IPersonRepository>();

            if (mapper == null)
            {
                var mapperConfiguration = new MapperConfiguration(configuration =>
                {
                    configuration.AddProfile(new PersonProfile());
                    configuration.AddProfile(new EmailProfile());
                    configuration.AddProfile(new PhoneProfile());
                });

                mapper = mapperConfiguration.CreateMapper();
                controller = new PersonsController(moqRepository.Object, mapper);
            }
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

            result.Result.Should().BeOfType<NotFoundResult>();
            result.Should().BeOfType<ActionResult<PersonReadDto>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_IEnumerable_Of_PersonReadDto_On_GetPersonsAsync()
        {
            var result = await controller.GetPersonsAsync();

            result.Result.Should().BeOfType<OkObjectResult>();
            result.Should().BeOfType<ActionResult<IEnumerable<PersonReadDto>>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_IEnumerable_Of_PersonReadDto_On_GetPersonsListAsync()
        {
            var result = await controller.GetPersonsListAsync();

            result.Should().BeOfType<ActionResult<IEnumerable<PersonInListDto>>>();
        }

        #endregion Get

        #region ********************************Post**********************************

        [Fact]
        public async Task Return_ActionResult_Of_PersonReadDto_On_CreatePersonAsync()
        {
            var person = new PersonCreateDto(new PersonName("Doe", "Jane"), Gender.Female);

            var result = await controller.CreatePersonAsync(person);

            result.Should().BeOfType<ActionResult<PersonReadDto>>();
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_CreatePersonAsync_When_ModelState_Invalid()
        {
            controller.ModelState.AddModelError("x", "Test Error Message");
            var person = new PersonCreateDto(new PersonName("Doe", "Jane"), Gender.Female);

            var result = await controller.CreatePersonAsync(person);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
            moqRepository.Verify(x => x.AddPersonAsync(It.IsAny<PersonCreateDto>()), Times.Never);
        }

        [Fact]
        public async Task Not_Save_On_CreatePersonAsync_When_ModelState_Invalid()
        {
            controller.ModelState.AddModelError("x", "Test Error Message");
            var person = new PersonCreateDto(new PersonName("Doe", "Jane"), Gender.Female);

            var result = await controller.CreatePersonAsync(person);

            moqRepository.Verify(x => x.AddPersonAsync(It.IsAny<PersonCreateDto>()), Times.Never);
        }

        [Fact]
        public async Task Save_On_CreatePersonAsync_When_ModelState_Valid()
        {
            PersonCreateDto savedPerson = null;

            moqRepository.Setup(x => x.AddPersonAsync(It.IsAny<PersonCreateDto>()))
                          .Returns(Task.CompletedTask)
                          .Callback<PersonCreateDto>(x => savedPerson = x);

            var person = new PersonCreateDto(new PersonName("Doe", "Jane"), Gender.Female);

            var result = await controller.CreatePersonAsync(person);

            moqRepository.Verify(x => x.AddPersonAsync(It.IsAny<PersonCreateDto>()), Times.Once);
            person.Name.Should().Be(savedPerson.Name);
            person.Gender.Should().Be(savedPerson.Gender);
            person.Birthday.Should().Be(savedPerson.Birthday);
        }

        [Fact]
        public async Task Return_PersonReadDto_On_CreatePersonAsync_When_ModelState_Valid()
        {
            moqRepository.Setup(x => x.AddPersonAsync(It.IsAny<PersonCreateDto>()));

            var person = new PersonCreateDto(new PersonName("Doe", "Jane"), Gender.Female);
            var result = await controller.CreatePersonAsync(person);

            result.Should().BeOfType<ActionResult<PersonReadDto>>();
        }

        #endregion Post

        #region ********************************Put***********************************

        [Fact]
        public async Task Return_NotFoundObjectResult_On_UpdatePersonAsync_With_Invalid_Id()
        {
            var person = new PersonUpdateDto { 
                Gender = Gender.Female
            };

            var result = await controller.UpdatePersonAsync(person.Id, person);

            result.Should().BeOfType<NotFoundObjectResult>();
        }


        #endregion Put
    }
}
