using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.Application
{
    public class TestServerFixtureTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture fixture;

        public TestServerFixtureTests(TestServerFixture fixture)
        {
            this.fixture = fixture;
        }
        public TestServer Server { get; }
        public HttpClient Client { get; }

        [Fact]
        public async Task SomethingAsync()
        {
            var url = "/persons/list";
            var response = await fixture.Client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}
