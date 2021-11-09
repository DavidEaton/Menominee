using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.ControllerTests
{
    public class OrganizationsControllerWithMocksShould
    {
        private readonly OrganizationsController controller;
        private readonly Mock<IOrganizationRepository> moqRepository;

        public OrganizationsControllerWithMocksShould()
        {
            moqRepository = new Mock<IOrganizationRepository>();
            controller = new OrganizationsController(moqRepository.Object);
        }

        #region ********************************Get***********************************

        [Fact]
        public async Task Return_ActionResult_Of_OrganizationReadDto_On_GetOrganizationAsync()
        {
            var result = await controller.GetOrganizationAsync(1);

            result.Should().BeOfType<ActionResult<OrganizationReadDto>>();
        }

        [Fact]
        public async Task Return_NotFoundResult_On_GetOrganizationAsyncWithInvalidId()
        {
            var result = await controller.GetOrganizationAsync(0);

            result.Result.Should().BeOfType<NotFoundResult>();
            result.Should().BeOfType<ActionResult<OrganizationReadDto>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_IEnumerable_Of_OrganizationReadDto_On_GetOrganizationsAsync()
        {
            var result = await controller.GetOrganizationsAsync();

            result.Should().BeOfType<ActionResult<IReadOnlyList<OrganizationReadDto>>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_IEnumerable_Of_OrganizationReadDto_On_GetOrganizationsListAsync()
        {
            var result = await controller.GetOrganizationsListAsync();

            result.Should().BeOfType<ActionResult<IReadOnlyList<OrganizationInListDto>>>();
        }

        #endregion Get

        #region ********************************Post**********************************

        [Fact]
        public async Task Return_ActionResult_Of_OrganizationReadDto_On_CreateOrganizationAsync()
        {
            var Organization = new OrganizationAddDto
            {
                Name = "Doe"
            };

            var result = await controller.AddOrganizationAsync(Organization);

            result.Should().BeOfType<ActionResult<OrganizationReadDto>>();
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_CreateOrganizationAsync_When_ModelState_Invalid()
        {
            controller.ModelState.AddModelError("x", "Test Error Message");
            var organization = new OrganizationAddDto();

            var result = await controller.AddOrganizationAsync(organization);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Not_Save_On_CreateOrganizationAsync_When_ModelState_Invalid()
        {
            controller.ModelState.AddModelError("x", "Test Error Message");
            var Organization = new OrganizationAddDto();

            var result = await controller.AddOrganizationAsync(Organization);

            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Save_On_CreateOrganizationAsync_When_ModelState_Valid()
        {
            Organization savedOrganization = null;

            moqRepository.Setup(organizationRepository =>
                                organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()))
                                    .Returns(Task.CompletedTask)
                                    .Callback<Organization>(organization => savedOrganization = organization);

            var Organization = new OrganizationAddDto
            {
                Name = "Moops"
            };

            var result = await controller.AddOrganizationAsync(Organization);

            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Once);

            Organization.Name.Should().Be(savedOrganization.Name.Name.ToString());
        }

        [Fact]
        public async Task Return_OrganizationReadDto_On_CreateOrganizationAsync_When_ModelState_Valid()
        {
            moqRepository.Setup(organizationRepository =>
                                organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()));

            var Organization = new OrganizationAddDto();
            var result = await controller.AddOrganizationAsync(Organization);

            result.Should().BeOfType<ActionResult<OrganizationReadDto>>();
        }

        #endregion Post

        #region ********************************Put***********************************

        [Fact]
        public async Task Return_NotFoundObjectResult_On_UpdateOrganizationAsync_With_Invalid_Id()
        {
            var invaldId = 0;
            var Organization = new OrganizationUpdateDto
            {
                Note = "note"
            };

            var result = await controller.UpdateOrganizationAsync(invaldId, Organization);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        #endregion Put

    }
}
