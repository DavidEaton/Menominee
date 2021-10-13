using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using FluentAssertions;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.ControllerTests
{
    public class OrganizationsControllerShould
    {
        private readonly OrganizationsController controller;
        private readonly Mock<IOrganizationRepository> moqRepository;

        public OrganizationsControllerShould()
        {
            moqRepository = new Mock<IOrganizationRepository>();
            controller = new OrganizationsController(moqRepository.Object);
        }

        #region ********************************Get***********************************

        [Fact]
        public async Task Return_ActionResult_Of_OrganizationReadDto_On_GetOrganizationAsync()
        {
            var result = await controller.GetOrganizationAsync(1);

            result.Result.Should().BeOfType<NotFoundResult>();
            result.Should().BeOfType<ActionResult<OrganizationToRead>>();
        }

        [Fact]
        public async Task Return_NotFoundResult_On_GetOrganizationAsyncWithInvalidId()
        {
            var result = await controller.GetOrganizationAsync(0);

            result.Should().BeOfType<ActionResult<OrganizationToRead>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_IEnumerable_Of_OrganizationReadDto_On_GetOrganizationsAsync()
        {
            var result = await controller.GetOrganizationsAsync();

            result.Should().BeOfType<ActionResult<IReadOnlyList<OrganizationToRead>>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_IEnumerable_Of_OrganizationReadDto_On_GetOrganizationsListAsync()
        {
            var result = await controller.GetOrganizationsListAsync();

            result.Should().BeOfType<ActionResult<IReadOnlyList<OrganizationToReadInList>>>();
        }

        #endregion Get

        #region ********************************Post**********************************

        [Fact]
        public async Task Return_ActionResult_Of_OrganizationReadDto_On_AddOrganizationAsync()
        {
            var Organization = new OrganizationToAdd
            {
                Name = "Doe"
            };

            var result = await controller.AddOrganizationAsync(Organization);

            result.Should().BeOfType<ActionResult<OrganizationToRead>>();
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_ModelState_Invalid()
        {
            controller.ModelState.AddModelError("x", "Test Error Message");
            var organization = new OrganizationToAdd();

            var result = await controller.AddOrganizationAsync(organization);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_OrganizationName_TooShort()
        {
            var organization = new OrganizationToAdd
            {
                Name = "M"
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'Name' must be between 2 and 255 character");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_OrganizationName_TooLong()
        {
            var organization = new OrganizationToAdd
            {
                // RuleFor(organization => organization.Name).NotNull().Length(2, 255); OrganizationToAdd.Name has 256 characters:
                Name = "cum sociis natoque penatibus et magnis dis parturient montes nascetur ridiculus mus mauris vitae ultricies leo integer malesuada nunc vel risus commodo viverra maecenas accumsan lacus vel facilisis volutpat est velit egestas dui id ornare arcu odio ut semu"
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'Name' must be between 2 and 255 character");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_OrganizationName_Empty()
        {
            var organization = new OrganizationToAdd
            {
                // RuleFor(organization => organization.Name).NotNull().Length(2, 255); OrganizationToAdd.Name has 256 characters:
                Name = ""
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'Name' must not be empty.");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_Organization_Address_AddressLine_Empty()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'Address Line' must not be empty.");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_Organization_Address_AddressLine_Null()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = null,
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'Address Line' must not be empty.");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_Organization_Address_AddressLine_Too_Short()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'Address Line' must be between 2 and 255 characters.");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_Organization_Address_AddressLine_Too_Long()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "cum sociis natoque penatibus et magnis dis parturient montes nascetur ridiculus mus mauris vitae ultricies leo integer malesuada nunc vel risus commodo viverra maecenas accumsan lacus vel facilisis volutpat est velit egestas dui id ornare arcu odio ut semu",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'Address Line' must be between 2 and 255 characters.");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_Organization_Address_City_Too_Long()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "cum sociis natoque penatibus et magnis dis parturient montes nascetur ridiculus mus mauris vitae ultricies leo integer malesuada nunc vel risus commodo viverra maecenas accumsan lacus vel facilisis volutpat est velit egestas dui id ornare arcu odio ut semu",
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'City' must be between 2 and 255 characters.");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_Organization_Address_City_Too_Short()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "c",
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'City' must be between 2 and 255 characters.");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_Organization_Address_City_Empty()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "",
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'City' must not be empty.");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_Organization_Address_City_Null()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = null,
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'City' must not be empty.");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_Organization_Address_PostalCode_Null()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = null
                }
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'Postal Code' must not be empty.");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_Organization_Address_PostalCode_Empty()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = ""
                }
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'Postal Code' must not be empty.");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_Organization_Address_PostalCode_Too_Short()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "1"
                }
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'Postal Code' must be between 5 and 10 characters.");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_On_AddOrganizationAsync_When_Organization_Address_PostalCode_Too_Long()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "12345678912"
                }
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.ToString().Should().Contain("'Postal Code' must be between 5 and 10 characters.");
            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Not_Save_On_AddOrganizationAsync_When_ModelState_Invalid()
        {
            controller.ModelState.AddModelError("x", "Test Error Message");
            var Organization = new OrganizationToAdd();

            var result = await controller.AddOrganizationAsync(Organization);

            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Never);
        }

        [Fact]
        public async Task Save_On_AddOrganizationAsync_When_ModelState_Valid()
        {
            Organization savedOrganization = null;

            moqRepository.Setup(organizationRepository =>
                                organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()))
                                    .Returns(Task.CompletedTask)
                                    .Callback<Organization>(organization => savedOrganization = organization);

            var Organization = new OrganizationToAdd
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
        public async Task Return_OrganizationReadDto_On_AddOrganizationAsync_When_ModelState_Valid()
        {
            moqRepository.Setup(organizationRepository =>
                                organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()));

            var Organization = new OrganizationToAdd();
            var result = await controller.AddOrganizationAsync(Organization);

            result.Should().BeOfType<ActionResult<OrganizationToRead>>();
        }

        #endregion Post

        #region ********************************Put***********************************

        [Fact]
        public async Task Return_NotFoundObjectResult_On_UpdateOrganizationAsync_With_Invalid_Id()
        {
            var invaldId = 0;
            var Organization = new OrganizationToEdit
            {
                Note = "note"
            };

            var result = await controller.UpdateOrganizationAsync(invaldId, Organization);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        #endregion Put

    }
}
