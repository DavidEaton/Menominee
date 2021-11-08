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
    public class CustomersControllerShould : SharedInstanceTest
    {
        private const string Path = "https://localhost/api/customers";
        private readonly HttpClient httpClient;

        public CustomersControllerShould(TestApplicationFactory<Startup, TestStartup> factory) : base(factory)
        {
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
        }

        [Fact]
        public async Task Return_Content_On_Get()
        {
            var provider = TestClaimsProvider.WithUserClaims();
            var client = Factory.CreateClientWithTestAuth(provider);

            var response = await client.GetAsync(Path);

            response.Content.Should().NotBeNull();
            response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(Path)]
        //[InlineData(Path + "1")]
        //[InlineData(Path + "list")]
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
