using CustomerVehicleManagement.Api.IntegrationTests.Helpers;
using System;
using System.Net.Http;

namespace CustomerVehicleManagement.Api.IntegrationTests.Application
{
    public class BasicTests : SharedInstanceTestFixture
    {
        private const string Path = "https://localhost/api/persons/list";
        private readonly HttpClient httpClient;
        public BasicTests(TestApplicationFactory<Startup, TestStartup> factory) : base(factory)
        {
            httpClient = factory.CreateDefaultClient(new Uri(Path));
        }

        //[Fact]
        //public async Task Should_Return_Content_On_Get()
        //{
        //    var persons = await httpClient.GetFromJsonAsync<IEnumerable<PersonToReadInList>>("");

        //    // Confirm that endpoint returns content (!= null && length > 0)
        //    persons.Should().NotBeNull();
        //}
    }
}
