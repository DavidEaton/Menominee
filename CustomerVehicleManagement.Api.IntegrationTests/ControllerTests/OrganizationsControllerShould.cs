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
    public class OrganizationsControllerShould : SharedInstanceTestFixture
    {
        private const string Path = "https://localhost/api/organizations/";
        private readonly HttpClient httpClient;

        public OrganizationsControllerShould(TestApplicationFactory<Startup, TestStartup> factory) : base(factory)
        {
            httpClient = factory.CreateDefaultClient(new Uri(Path));
        }

        [Fact]
        public async Task Return_Success_And_Expected_MediaType_For_Regular_User_On_Get()
        {
            /* Uses case-insensitive deserialization
               Confirms that endpoint exists at the expected uri
               Confirms that response has success status code
               Confirms Content-Type header
               Confirms that response includes content (!= null && length > 0)
            */

            // Arrange
            var provider = TestClaimsProvider.WithUserClaims();
            var client = Factory.CreateClientWithTestAuth(provider);
            var mediaType = "application/json";

            // Act
            var response = await client.GetAsync(Path);

            // Assert
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.MediaType.Should().Be(mediaType);
            response.Content.Should().NotBeNull();
            response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(Path)]
        [InlineData(Path + "1")]
        [InlineData(Path + "list")]
        public async Task Return_Redirect_To_Anonymous_User_On_Get_Secure_Urls(string url)
        {
            var client = Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            var response = await client.GetAsync(url);

            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.Headers.Location.LocalPath.Should().Be("/auth/login");
        }
    }
}
