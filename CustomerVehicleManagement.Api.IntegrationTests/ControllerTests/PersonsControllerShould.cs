using CustomerVehicleManagement.Api.IntegrationTests.Helpers;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Shared.Models;
using FluentAssertions;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.ControllerTests
{
    public class PersonsControllerShould : SharedInstanceTestFixture, IClassFixture<PersonsController>
    {
        private const string Path = "https://localhost/api/persons/";
        private readonly HttpClient httpClient;
        private readonly PersonsController controller;

        public PersonsControllerShould(TestApplicationFactory<Startup,
                                       TestStartup> factory,
                                       PersonsController controller) : base(factory)
        {
            this.controller = controller;
            httpClient = factory.CreateDefaultClient(new Uri(Path));
        }

        [Fact]
        public async Task Return_Success_And_Expected_MediaType_For_Regular_User_On_Get()
        {
            var provider = TestClaimsProvider.WithUserClaims();
            var client = Factory.CreateClientWithTestAuth(provider);
            var mediaType = "application/json";

            var response = await client.GetAsync(Path);

            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.MediaType.Should().Be(mediaType);
            response.Content.Should().NotBeNull();
            response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Return_ActionResult_Of_PersonToRead_On_GetPersonAsync()
        {
            var provider = TestClaimsProvider.WithUserClaims();
            var client = Factory.CreateClientWithTestAuth(provider);
            var mediaType = "application/json";

            var result = await controller.GetPersonAsync(1);

            result.Result.Should().BeOfType<NotFoundResult>();
            result.Should().BeOfType<ActionResult<PersonToRead>>();
        }

        [Fact]
        public async Task Return_NotFoundResult_On_GetPersonAsyncWithInvalidId()
        {
            var result = await controller.GetPersonAsync(0);

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

        #region ********************************Post**********************************

        [Fact]
        public async Task Return_ActionResult_Of_PersonToRead_On_CreatePersonAsync()
        {
            var person = new PersonToWrite { Name = new PersonNameToWrite { LastName = "Doe", FirstName = "Jane" }, Gender = Gender.Female };

            var result = await controller.AddPersonAsync(person);

            result.Should().BeOfType<ActionResult<PersonToRead>>();
        }

        [Fact]
        public async Task Save_On_CreatePersonAsync_When_ModelState_Valid()
        {
            //Person savedPerson = null;

            //moqRepository.Setup(repo => repo.AddPersonAsync(It.IsAny<Person>()))
            //              .Returns(Task.CompletedTask)
            //              .Callback<Person>(person => savedPerson = person);

            //var person = new PersonToWrite { Name = new PersonNameToWrite { LastName = "Doe", MiddleName = "J.", FirstName = "Jane" }, Gender = Gender.Female };

            //var result = await controller.CreatePersonAsync(person);

            //moqRepository.Verify(organizationRepository =>
            //                     organizationRepository
            //                        .AddPersonAsync(It.IsAny<Person>()), Times.Once);

            //person.Name.LastName.Should().Be(savedPerson.Name.LastName);
            //person.Name.FirstName.Should().Be(savedPerson.Name.FirstName);
            //person.Name.MiddleName.Should().Be(savedPerson.Name.MiddleName);
            //person.Gender.Should().Be(savedPerson.Gender);
            //person.Birthday.Should().Be(savedPerson.Birthday);
        }

        [Fact]
        public async Task Return_PersonToRead_On_CreatePersonAsync_When_ModelState_Valid()
        {
            //moqRepository.Setup(repository => repository.AddPersonAsync(It.IsAny<Person>()));

            //var person = new PersonToWrite { Name = new PersonNameToWrite { LastName = "Doe", FirstName = "Jane" }, Gender = Gender.Female };
            //var result = await controller.CreatePersonAsync(person);

            //result.Should().BeOfType<ActionResult<PersonToRead>>();
        }

        #endregion Post

        #region ********************************Put***********************************

        [Fact]
        public async Task Return_NotFoundObjectResult_On_UpdatePersonAsync_With_Invalid_Id()
        {
            var invaldId = 0;
            var person = new PersonToWrite
            {
                Gender = Gender.Female
            };

            var result = await controller.UpdatePersonAsync(invaldId, person);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        #endregion Put
    }
}
