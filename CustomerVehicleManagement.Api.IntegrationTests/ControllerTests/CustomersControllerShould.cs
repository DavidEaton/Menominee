using CustomerVehicleManagement.Shared.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.Controllers
{
    /// <summary>
    /// Uses CustomerVehicleManagement.Api.Startup.cs: const string Connection = "Server=localhost;Database=Menominee;Trusted_Connection=True;";
    /// Tests rely on local database having some rows
    /// TODO: Add setup and teardown to create and populate database before running tests,
    /// delete database after tests run
    /// </summary>
    public class CustomersControllerShould : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string CustomersControllerPath = "https://localhost/api/customers";
        private readonly HttpClient httpClient;

        public CustomersControllerShould(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateDefaultClient(new Uri(CustomersControllerPath));
        }

        [Fact]
        public async Task Return_Success_Status_On_GetCustomers()
        {
            var response = await httpClient.GetAsync(string.Empty);

            // Confirm that endpoint exists at, and returns success code from the expected uri
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Return_Expected_MediaType_On_GetCustomers()
        {
            var mediaType = "application/json";
            var response = await httpClient.GetAsync(string.Empty);

            // Confirm that endpoint returns JSON ContentType
            response.Content.Headers.ContentType.MediaType.Should().Be(mediaType);
        }

        [Fact]
        public async Task Return_Content_On_GetCustomers()
        {
            var response = await httpClient.GetAsync(string.Empty);

            response.Content.Should().NotBeNull();
            response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
        }


        [Fact]
        public async Task Return_Organization_Customer_On_GetCustomer()
        {
            HttpContent content = (await httpClient.GetAsync(CustomersControllerPath + "/11")).Content;
            string jsonContent = content.ReadAsStringAsync().Result;
            CustomerReadDto customer = JsonSerializer.Deserialize<CustomerReadDto>(jsonContent);

            customer.Should().NotBeNull();
        }

        [Fact]
        public async Task Return_Person_Customer_On_GetCustomer()
        {
            HttpContent content = (await httpClient.GetAsync(CustomersControllerPath + "/10")).Content;
            string jsonContent = content.ReadAsStringAsync().Result;
            CustomerReadDto customer = JsonSerializer.Deserialize<CustomerReadDto>(jsonContent);

            customer.Should().NotBeNull();
        }

    }
}
