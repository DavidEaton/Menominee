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
    public class PersonsControllerShould : SharedInstanceTest
    {
        private const string Path = "https://localhost/api/persons/";
        private readonly HttpClient httpClient;

        public PersonsControllerShould(TestApplicationFactory<Startup, TestStartup> factory) : base(factory)
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

            var provider = TestClaimsProvider.WithUserClaims();
            var client = Factory.CreateClientWithTestAuth(provider);
            var mediaType = "application/json";

            var response = await client.GetAsync(Path);

            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.MediaType.Should().Be(mediaType);
        }

        [Fact]
        public async Task Return_Content_On_Get()
        {
            var provider = TestClaimsProvider.WithUserClaims();
            var client = Factory.CreateClientWithTestAuth(provider);

            var response = await client.GetAsync(Path);

            // Confirm that endpoint returns content (!= null && length > 0)
            response.Content.Should().NotBeNull();
            response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(Path)]
        [InlineData(Path + "1")]
        [InlineData(Path + "list")]
        public async Task Get_EndpointsReturnFailToAnonymousUserForSecureUrls(string url)
        {
            var client = Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            // hits controller action
            var response = await client.GetAsync(url);
            var redirectUrl = response.Headers.Location.LocalPath;

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/auth/login", redirectUrl);
        }

    }
}
