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
        public async Task Return_ActionResult_Of_OrganizationToRead_On_AddOrganizationAsync()
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
            OrganizationToAdd organizationToAdd = Utilities.CreateOrganizationToAdd();

            var result = await controller.AddOrganizationAsync(organizationToAdd);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_Name_is_too_short()
        {
            var organization = new OrganizationToAdd
            {
                Name = "M"
            };

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(OrganizationName.MinimumLengthMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_Name_is_too_long()
        {

            var organization = new OrganizationToAdd
            {
                Name = Utilities.RandomCharacters(256)
            };

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(OrganizationName.MaximumLengthMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_Name_is_empty()
        {
            var organization = new OrganizationToAdd
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
            var organization = new OrganizationToAdd
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
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_Note_is_too_long()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Note = Utilities.RandomCharacters(10001)
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.Should().Be($"Failed to add {organization.Name}.");
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_AddressLine_is_empty()
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

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Address.AddressRequiredMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_State_is_invalid()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "Traverse City",
                    State = (State)99,
                    PostalCode = "49999"
                }
            };

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.Should().Be($"Failed to add {organization.Name}.");
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_AddressLine_is_null()
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

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Address.AddressRequiredMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_AddressLine_is_too_short()
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

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Address.AddressMinimumLengthMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_AddressLine_is_too_long()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = Utilities.RandomCharacters(256),
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Address.AddressMaximumLengthMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_City_is_too_long()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = Utilities.RandomCharacters(256),
                    State = State.MI,
                    PostalCode = "49999"
                }
            };

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Address.CityMaximumLengthMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_City_is_too_short()
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

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Address.CityMinimumLengthMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_City_is_empty()
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

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Address.CityRequiredMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_City_is_null()
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

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Address.CityRequiredMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_PostalCode_is_null()
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

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Address.PostalCodeRequiredMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_PostalCode_is_empty()
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

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Address.PostalCodeRequiredMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_PostalCode_is_too_short()
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

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Address.PostalCodeMinimumLengthMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_PostalCode_is_too_long()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "12345678910"
                }
            };

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Address.PostalCodeMaximumLengthMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_organization_PostalCode_is_invalid()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "Z4566"
                }
            };

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Address.PostalCodeInvalidMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_Emails_collection_has_more_than_one_Primary()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops"
            };

            organization.Emails.Add(new EmailToAdd
            {
                Address = "a@a.a",
                IsPrimary = true
            });

            organization.Emails.Add(new EmailToAdd
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
            var organization = new OrganizationToAdd
            {
                Name = "Moops"
            };

            organization.Emails.Add(new EmailToAdd
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
                ex.Error.Should().Be(Email.EmailErrorMessage);
            }
        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_Phones_collection_has_invalid_Number()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops"
            };

            organization.Phones.Add(new PhoneToAdd
            {
                Number = "aa.a",
                IsPrimary = true
            });

            var result = await controller.AddOrganizationAsync(organization);

            BadRequestObjectResult badRequestObjectResult = (BadRequestObjectResult)result.Result;
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            badRequestObjectResult.StatusCode.Should().Be(400);
            badRequestObjectResult.Value.Should().Be($"Failed to add {organization.Name}.");

        }

        [Fact]
        public async Task Return_BadRequestObjectResult_on_AddOrganizationAsync_when_Phones_collection_has_more_than_one_Primary()
        {
            var organization = new OrganizationToAdd
            {
                Name = "Moops"
            };

            organization.Phones.Add(new PhoneToAdd
            {
                Number = "9896279206",
                IsPrimary = true
            });

            organization.Phones.Add(new PhoneToAdd
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
            var organization = new OrganizationToAdd();

            try
            {
                var result = await controller.AddOrganizationAsync(organization);

            }
            catch (CSharpFunctionalExtensions.ResultFailureException ex)
            {
                ex.Error.Should().Be(Organization.NameRequiredMessage);
            }
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
        public async Task Save_object_graph_on_AddOrganizationAsync()
        {
            Organization savedOrganization = null;

            moqRepository.Setup(organizationRepository =>
                                organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()))
                                    .Returns(Task.CompletedTask)
                                    .Callback<Organization>(organization => savedOrganization = organization);

            var organization = new OrganizationToAdd
            {
                Name = "Moops",
                Address = new AddressToAdd
                {
                    AddressLine = "1234 Five Ave.",
                    City = "Traverse City",
                    State = State.MI,
                    PostalCode = "49999"
                },
                Note = "a note"
            };

            organization.Phones.Add(new PhoneToAdd
            {
                Number = "9896279206",
                IsPrimary = true
            });

            organization.Phones.Add(new PhoneToAdd
            {
                Number = "2315462102",
                IsPrimary = false
            });

            organization.Emails.Add(new EmailToAdd
            {
                Address = "a@a.a",
                IsPrimary = true
            });

            organization.Emails.Add(new EmailToAdd
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
        public async Task Return_OrganizationToRead_On_AddOrganizationAsync_When_ModelState_Valid()
        {
            moqRepository.Setup(organizationRepository =>
                                organizationRepository
                                    .AddOrganizationAsync(It.IsAny<Organization>()));

            var organization = Utilities.CreateOrganizationToAdd();
            var result = await controller.AddOrganizationAsync(organization);

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
