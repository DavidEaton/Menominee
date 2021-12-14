using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.ControllerTests
{
    public class OrganizationsControllerShould
    {
        private readonly OrganizationsController controller;
        private readonly Mock<IOrganizationRepository> moqRepository;

        /*
          Validators should only be tested with integration tests. Moreover, they shouldn't be tested directly, only via integration tests that cover corresponding controllers. Otherwise, the tests risk raising false negatives (e.g when you write a validator but forget to tie it to the appropriate controller, hence the tests don't turn red when they should) and false positives (e.g you move some of the validations to the controller but tests fail because they expect those validations to be present in the validator). -Vladimir Khorikov
        */

        public OrganizationsControllerShould()
        {
            moqRepository = new Mock<IOrganizationRepository>();
            controller = new OrganizationsController(moqRepository.Object);
        }

        #region ********************************Get***********************************

        [Fact]
        public async Task Return_ActionResult_Of_OrganizationToRead_On_GetOrganizationAsync()
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
        public async Task Return_ActionResult_Of_IEnumerable_Of_OrganizationToRead_On_GetOrganizationsAsync()
        {
            var result = await controller.GetOrganizationsAsync();

            result.Should().BeOfType<ActionResult<IReadOnlyList<OrganizationToRead>>>();
        }

        [Fact]
        public async Task Return_ActionResult_Of_IEnumerable_Of_OrganizationToRead_On_GetOrganizationsListAsync()
        {
            var result = await controller.GetOrganizationsListAsync();

            result.Should().BeOfType<ActionResult<IReadOnlyList<OrganizationToReadInList>>>();
        }

        #endregion Get

        #region ********************************Post**********************************

        [Fact]
        public async Task Return_CreatedResult_On_AddOrganizationAsync()
        {
            var Organization = new OrganizationToWrite
            {
                Name = "Doe"
            };

            var result = await controller.AddOrganizationAsync(Organization);

            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_Name_is_too_short()
        {
            var organization = new OrganizationToWrite
            {
                Name = "M"
            };

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Contain(OrganizationName.MinimumLengthMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_Name_is_too_long()
        {

            var organization = new OrganizationToWrite
            {
                Name = Utilities.RandomCharacters(256)
            };

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Contain(OrganizationName.MaximumLengthMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_Name_is_empty()
        {
            var organization = new OrganizationToWrite
            {
                Name = ""
            };

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(OrganizationName.RequiredMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_Name_is_null()
        {
            var organization = new OrganizationToWrite
            {
                Name = null
            };

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(OrganizationName.RequiredMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_Emails_collection_has_more_than_one_Primary()
        {
            var organization = new OrganizationToWrite
            {
                Name = "Moops"
            };

            organization.Emails.Add(new EmailToWrite
            {
                Address = "a@a.a",
                IsPrimary = true
            });

            organization.Emails.Add(new EmailToWrite
            {
                Address = "b@b.b",
                IsPrimary = true
            });

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (Exception ex)
            {
                ex.Message.Should().Be("Contactable already has primary email.");
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_Emails_collection_has_invalid_Address()
        {
            var organization = new OrganizationToWrite
            {
                Name = "Moops"
            };

            organization.Emails.Add(new EmailToWrite
            {
                Address = "aa.a",
                IsPrimary = true
            });


            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Email.InvalidMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_Phones_collection_has_more_than_one_Primary()
        {
            var organization = new OrganizationToWrite
            {
                Name = "Moops"
            };

            organization.Phones.Add(new PhoneToWrite
            {
                Number = "9896279206",
                IsPrimary = true
            });

            organization.Phones.Add(new PhoneToWrite
            {
                Number = "2315462102",
                IsPrimary = true
            });

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (Exception ex)
            {
                ex.Message.Should().Be("Contactable already has primary phone.");
            }
        }

        [Fact]
        public async Task Not_Save_On_AddOrganizationAsync_When_ModelState_Invalid()
        {
            controller.ModelState.AddModelError("x", "Test Error Message");
            var organization = new OrganizationToWrite();

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Contain(OrganizationName.RequiredMessage);
            }
        }

        [Fact]
        public async Task Save_object_graph_on_AddOrganizationAsync()
        {
            Organization savedOrganization = null;

            moqRepository.Setup(organizationRepository =>
                                organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()))
                                    .Returns(Task.CompletedTask)
                                    .Callback<Organization>(organization => savedOrganization = organization);

            var organization = new OrganizationToWrite
            {
                Name = "Moops"
            };

            organization.Phones.Add(new PhoneToWrite
            {
                Number = "9896279206",
                IsPrimary = true
            });

            organization.Phones.Add(new PhoneToWrite
            {
                Number = "2315462102",
                IsPrimary = false
            });

            organization.Emails.Add(new EmailToWrite
            {
                Address = "a@a.a",
                IsPrimary = true
            });

            organization.Emails.Add(new EmailToWrite
            {
                Address = "b@b.b",
                IsPrimary = false
            });

            var result = await controller.AddOrganizationAsync(organization);

            moqRepository.Verify(organizationRepository =>
                                 organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()), Times.Once);

            organization.Name.Should().Be(savedOrganization.Name.Name.ToString());
        }

        [Fact]
        public async Task Return_CreatedResult_On_CreateOrganizationAsync_When_ModelState_Valid()
        {
            moqRepository.Setup(organizationRepository =>
                                organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()));

            var organization = new OrganizationToWrite
            {
                Name = "Moops"
            };

            var result = await controller.AddOrganizationAsync(organization);

            result.Should().BeOfType<CreatedResult>();
        }

        #endregion Post

        #region ********************************Put***********************************

        [Fact]
        public async Task Return_NotFoundObjectResult_On_UpdateOrganizationAsync_With_Invalid_Id()
        {
            var invaldId = 0;
            var Organization = new OrganizationToWrite
            {
                Note = "note"
            };

            var result = await controller.UpdateOrganizationAsync(invaldId, Organization);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        #endregion Put

    }
}
