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
    public class EmployeesControllerShould : SharedInstanceTestFixture
    {
        private const string Path = "https://localhost/api/employees/";
        private readonly HttpClient httpClient;

        public EmployeesControllerShould(TestApplicationFactory<Startup, TestStartup> factory) : base(factory)
        {
            httpClient = factory.CreateDefaultClient(new Uri(Path));
        }

        [Fact]
        public async Task Return_Success_And_Expected_MediaType_For_ShopAdmin_User_On_Get()
        {
            var provider = TestClaimsProvider.WithShopAdminClaims();
            var client = Factory.CreateClientWithTestAuth(provider);
            var mediaType = "application/json";

            var response = await client.GetAsync(Path);

            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.MediaType.Should().Be(mediaType);
            response.Content.Should().NotBeNull();
            response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Return_Success_And_Expected_MediaType_For_ShopOwner_User_On_Get()
        {
            var provider = TestClaimsProvider.WithShopOwnerClaims();
            var client = Factory.CreateClientWithTestAuth(provider);
            var mediaType = "application/json";

            var response = await client.GetAsync(Path);

            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.MediaType.Should().Be(mediaType);
            response.Content.Should().NotBeNull();
            response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Return_Success_And_Expected_MediaType_For_CanManageHumanResources_User_On_Get()
        {
            var provider = TestClaimsProvider.WithHumanResourcesClaims();
            var client = Factory.CreateClientWithTestAuth(provider);
            var mediaType = "application/json";

            var response = await client.GetAsync(Path);

            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.MediaType.Should().Be(mediaType);
            response.Content.Should().NotBeNull();
            response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Return_Forbidden_To_Regular_User_On_Get()
        {
            var provider = TestClaimsProvider.WithUserClaims();
            var client = Factory.CreateClientWithTestAuth(provider);

            var response = await client.GetAsync(Path);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [InlineData(Path)]
        public async Task Return_Redirect_To_Anonymous_User_On_Get_Secure_Urls(string url)
        {
            var client = Factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

            var response = await client.GetAsync(url);

            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.Headers.Location.LocalPath.Should().Be("/auth/login");
        }

    }
}
