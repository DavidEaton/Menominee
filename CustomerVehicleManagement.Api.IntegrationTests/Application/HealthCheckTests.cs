using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.Application
{
    public class HealthCheckTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient httpclient;
        private const string UriSegment = "/healthcheck";

        /*
        By default, xUnit creates a new instance of a test class for each test method. 
        When using an xUnit Class Fixture:
          - a single, shared instance is created
          - the same test server is used by each test method on the class
          - once tests are complete, it will clean up by calling Dispose (if present)
         
        More efficient when test setup or teardown is expensive
        */
        public HealthCheckTests(WebApplicationFactory<Startup> factory)
        {
            httpclient = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task HealthCheck_Returns_Ok()
        {
            /***********************************************************************
            Test confirms that:
              Api application starts
              Server is running and can handle requests
              Required services are registered with the dependency injection container
              Middleware pipeline is correctly configured
              Routing sends requests to the expected endpoint
            ***********************************************************************/

            var response = await httpclient.GetAsync(UriSegment);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
