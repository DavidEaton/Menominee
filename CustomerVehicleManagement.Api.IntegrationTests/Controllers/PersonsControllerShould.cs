using AutoMapper;
using CustomerVehicleManagement.Api.Controllers;
using CustomerVehicleManagement.Api.Data.Interfaces;
using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Api.Profiles;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

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

        [Fact]
        public async Task Return_ActionResult_Of_PersonReadDto_On_GetPersonAsync()
        {
            ActionResult<PersonReadDto> result = await controller.GetPersonAsync(1);

            result.Should().BeOfType<ActionResult<PersonReadDto>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_IEnumerable_Of_PersonReadDto_On_GetPersonsAsync()
        {
            ActionResult<IEnumerable<PersonReadDto>> result = await controller.GetPersonsAsync();

            result.Should().BeOfType<ActionResult<IEnumerable<PersonReadDto>>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_IEnumerable_Of_PersonReadDto_On_GetPersonsListAsync()
        {
            ActionResult<IEnumerable<PersonInListDto>> result = await controller.GetPersonsListAsync();

            result.Should().BeOfType<ActionResult<IEnumerable<PersonInListDto>>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_PersonReadDto_On_CreatePersonAsync()
        {
            var person = new PersonCreateDto(new PersonName("Doe", "Jane"), Gender.Female);

            ActionResult<PersonReadDto> result = await controller.CreatePersonAsync(person);

            result.Should().BeOfType<ActionResult<PersonReadDto>>();
        }


        [Fact]
        public async Task Return_BadRequestObjectResult_On_CreatePersonAsync_When_ModelStateInvalid()
        {
            controller.ModelState.AddModelError("x", "Test Error Message");
            var person = new PersonCreateDto(new PersonName("Doe", "Jane"), Gender.Female);

            ActionResult<PersonReadDto> result = await controller.CreatePersonAsync(person);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
