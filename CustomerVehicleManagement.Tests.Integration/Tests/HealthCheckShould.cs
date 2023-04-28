using CustomerVehicleManagement.Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Tests.Integration.Tests
{
    public class HealthCheckShould : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient httpclient;
        private const string HealthCheckRoute = "/healthcheck";

        /*
        By default, xUnit creates a new instance of a test class for each test method.
        When using an xUnit Class Fixture:
          - a single, shared instance is created
          - the same test server is used by each test method on the class
          - once tests are complete, it will clean up by calling Dispose (if present)

        More efficient to use IClassFixture when test setup or teardown is expensive,
        and running multiple tests.
        */
        public HealthCheckShould(WebApplicationFactory<Program> factory)
        {
            httpclient = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task Return_Ok()
        {
            /***********************************************************************
            Test confirms that:
              Api application starts
              Server is running and can handle requests
              Required services are registered with the dependency injection container
              Middleware pipeline is correctly configured
              Routing sends requests to the expected endpoint
            ***********************************************************************/

            var response = await httpclient.GetAsync(HealthCheckRoute);

            response.StatusCode.Should().Be(HttpStatusCode.Found);
        }
    }
}