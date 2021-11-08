using CustomerVehicleManagement.Api.IntegrationTests.Helpers;
using FluentAssertions;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.Application
{
    public class BasicTests : SharedInstanceTest
    {
        private const string Path = "https://localhost/api/persons/list";
        private readonly HttpClient httpClient;
        public BasicTests(TestApplicationFactory<Startup, TestStartup> factory) : base(factory)
        {
            httpClient = factory.CreateDefaultClient(new Uri(Path));
        }

        [Fact]
        public async Task Should_Return_Content_On_Get()
        {
            var provider = TestClaimsProvider.WithUserClaims();
            var client = Factory.CreateClientWithTestAuth(provider);

            var response = await client.GetAsync(Path);

            // Confirm that endpoint returns content (!= null && length > 0)
            response.Content.Should().NotBeNull();
            response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
        }
    }
}
