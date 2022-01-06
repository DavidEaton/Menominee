using CustomerVehicleManagement.Api.IntegrationTests.Helpers;
using FluentAssertions;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.ControllerTests
{
    public class CustomersControllerShould : SharedInstanceTestFixture
    {
        private const string Path = "https://localhost/api/customers/";
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
            response.Content.Should().NotBeNull();
            response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
        }

        // These next two tests depend on hard-coded entity Id values.
        // Refactor to remove that dependency.

        //[Fact]
        //public async Task Return_Organization_Customer_On_GetCustomer()
        //{
        //    HttpContent content = (await httpClient.GetAsync(CustomersControllerPath + "/11")).Content;
        //    string jsonContent = content.ReadAsStringAsync().Result;
        //    CustomerToRead customer = JsonSerializer.Deserialize<CustomerToRead>(jsonContent);

        //    customer.Should().NotBeNull();
        //}

        //[Fact]
        //public async Task Return_Person_Customer_On_GetCustomer()
        //{
        //    HttpContent content = (await httpClient.GetAsync(CustomersControllerPath + "/10")).Content;
        //    string jsonContent = content.ReadAsStringAsync().Result;
        //    CustomerToRead customer = JsonSerializer.Deserialize<CustomerToRead>(jsonContent);

        //    // hits controller action
        //    var response = await client.GetAsync(url);

        //    response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        //    response.Headers.Location.LocalPath.Should().Be("/auth/login");
        //}
    }
}
