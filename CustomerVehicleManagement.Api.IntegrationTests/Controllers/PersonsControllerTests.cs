using CustomerVehicleManagement.Api.Data.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.Controllers
{
    /// <summary>
    /// Integrations tests will alert us to breaking changes in our API
    /// Tests controller action methods and underlying business logic
    /// </summary>
    public class PersonsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string Path = "https://localhost/api/persons/list";
        private const string UriSegment = "";
        private const int MaxCacheAge = 300;
        private const int Minute = 60;
        private readonly HttpClient httpClient;

        public PersonsControllerTests(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateDefaultClient(new Uri(Path));
        }

        [Fact]
        public async Task GetReturnsExpectedJson()
        {
            /* Uses case-insensitive deserialization
               Confirms that endpoint exists at the expected uri
               Confirms that response has success status code
               Confirms Content-Type header
               Confirms that response includes content (!= null && length > 0)
            */

            var persons = await httpClient.GetFromJsonAsync<List<PersonInListDto>>(UriSegment);

            // TEST DEPENDS ON A PERSON EXISTING IN THE DATABASE WITH Id == 1 AT ROW ONE (INDEX ZERO)
            // Modify to use test database instead of production
            Assert.Equal(1, persons[0]?.Id);
        }

        [Fact]
        public async Task GetSetsExpectedCacheControlHeader()
        {
            var response = await httpClient.GetAsync(string.Empty);

            var header = response.Headers.CacheControl;

            Assert.True(header.MaxAge.HasValue);
            Assert.Equal(TimeSpan.FromMinutes(MaxCacheAge / Minute), header.MaxAge);
            Assert.True(header.Public);
        }
    }
}
