using CustomerVehicleManagement.Api.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.ControllerTests
{
    /// <summary>
    /// Integrations tests will alert us to breaking changes in our API
    /// Tests controller action methods and underlying business logic
    /// </summary>

    // With IClassFixture, create a shared instance of this test class used for all of the tests within.
    // By default, xUnit creates a new instance of the test class for every test method. When using a class
    // fixture, a single instance is created before any of the test methods are executed. After all of the
    // tests have completed, the class fixture will be cleaned up by calling its dispose method.
    public class OrganizationsControllerShould : SharedInstanceTest
    {
        private const string Path = "https://localhost/api/organizations/";
        private const string ListPath = "https://localhost/api/organizations/list";
        private readonly HttpClient httpClient;

        public OrganizationsControllerShould(TestApplicationFactory<Startup, TestStartup> factory) : base(factory)
        {
            httpClient = factory.CreateDefaultClient(new Uri(Path));
        }

        [Fact]
        public async Task Return_Success_And_Expected_MediaType_For_Regular_User_On_Get()
        {
            // Arrange
            var provider = TestClaimsProvider.WithUserClaims();
            var client = Factory.CreateClientWithTestAuth(provider);
            var mediaType = "application/json";

            // Act
            var response = await client.GetAsync(Path);

            // Assert
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.MediaType.Should().Be(mediaType);
        }

        [Fact]
        public async Task Return_Content_On_Get()
        {
            var provider = TestClaimsProvider.WithUserClaims();
            var client = Factory.CreateClientWithTestAuth(provider);

            var response = await client.GetAsync(ListPath);

            // Confirm that endpoint returns content (!= null && length > 0)
            response.Content.Should().NotBeNull();
            response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(Path)]
        [InlineData(ListPath)]
        [InlineData(Path + "1")]
        public async Task Get_EndpointsReturnFailToAnonymousUserForSecureUrls(string url)
        {
            var client = Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            // NOT hits controller action
            var response = await client.GetAsync(url);
            var redirectUrl = response.Headers.Location.LocalPath;

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/auth/login", redirectUrl);
        }

    }
}
