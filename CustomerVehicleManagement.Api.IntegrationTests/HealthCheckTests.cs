using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests
{
    public class HealthCheckTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient httpclient;
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

            var response = await httpclient.GetAsync("/healthcheck");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
