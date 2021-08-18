using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.Controllers
{
    public class OrganizationsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string OrganizationsControllerListPath = "https://localhost/api/organizations/list";
        private readonly HttpClient httpClient;

        public OrganizationsControllerTests(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateDefaultClient(new Uri(OrganizationsControllerListPath));
        }

        [Fact]
        public async Task Get_Returns_Success_StatusCode()
        {
            var response = await httpClient.GetAsync(string.Empty);

            // Confirm that endpoint exists at the expected uri
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_Returns_Expected_MediaType()
        {
            var response = await httpClient.GetAsync(string.Empty);
            var mediaType = "application/json";

            // Confirm that endpoint returns JSON ContentType
            response.Content.Headers.ContentType.MediaType.Should().Be(mediaType);
        }

        [Fact]
        public async Task Get_Returns_Content()
        {
            var response = await httpClient.GetAsync(string.Empty);

            // Confirm that endpoint returns content (!= null && length > 0)
            response.Content.Should().NotBeNull();
            response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetList_Returns_Content()
        {
            var response = await httpClient.GetAsync("list");

            response.Content.Should().NotBeNull();
            response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
        }
    }
}
