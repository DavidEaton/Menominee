using CustomerVehicleManagement.Api.IntegrationTests.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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
        const string PATH = "https://localhost/api/persons/list";
        private const int MAXAGE = 300;
        private const int MINUTE = 60;
        private readonly HttpClient httpClient;

        public PersonsControllerTests(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateDefaultClient(new Uri(PATH));
        }

        [Fact]
        public async Task GetReturnsSuccessStatusCode()
        {
            var response = await httpClient.GetAsync(string.Empty);

            // Confirm that endpoint exists at the expected uri
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetReturnsExpectedMediaType()
        {
            var response = await httpClient.GetAsync(string.Empty);

            // Confirm that endpoint returns JSON ContentType
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task GetReturnsContent()
        {
            var response = await httpClient.GetAsync(string.Empty);

            // Confirm that endpoint returns content (!= null && length > 0)
            Assert.NotNull(response.Content);
            Assert.True(response.Content.Headers.ContentLength > 0);
        }

        //[Fact]
        //public async Task GetReturnsExpectedJson()
        //{
        //    var expected = new List<string> { "Here", "Are", "Some", "Values" };
        //    var responseStream = await httpclient.GetStreamAsync(string.Empty);
        //    var model = await JsonSerializer.DeserializeAsync<ExpectedJsonModel>(responseStream,
        //        JsonSerializerHelper.DefaultDeserializationOptions);

        //    // Confirm that endpoint returns the expected JSON content
        //    Assert.NotNull(model?.ExpectedProperty);
        //    //Assert.Equal(expected.OrderBy(s => s), model.ExpectedProperty.OrderBy(s => s));
        //}

        //[Fact]
        //public async Task GetReturnsExpectedResponse()
        //{


        [Fact]
        public async Task GetSetsExpectedCacheControlHeader()
        {
            var response = await httpClient.GetAsync(string.Empty);

            var header = response.Headers.CacheControl;

            Assert.True(header.MaxAge.HasValue);
            Assert.Equal(TimeSpan.FromMinutes(MAXAGE / MINUTE), header.MaxAge);
            Assert.True(header.Public);
        }

        [Fact]
        public async Task GetPersonsTotalReturnsExpectedJson()
        {
            var model = await httpClient.GetFromJsonAsync<ExpectedPersonsTotalOutputModel>("total");

            Assert.True(model?.PersonsTotal > 0);
        }
    }
}
