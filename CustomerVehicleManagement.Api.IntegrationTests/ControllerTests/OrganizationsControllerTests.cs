using AutoMapper;
using CustomerVehicleManagement.Api.Organizations;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.Controllers
{
    public class OrganizationsControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private static IMapper mapper;
        private const string Path = "https://localhost/api/organizations/list";
        private const int MaxCacheAge = 300;
        private const int Minute = 60;
        private readonly HttpClient httpClient;

        public OrganizationsControllerTests(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateDefaultClient(new Uri(Path));

            if (mapper == null)
            {
                var mapperConfiguration = new MapperConfiguration(configuration =>
                {
                    configuration.AddProfile(new OrganizationProfile());
                });

                mapper = mapperConfiguration.CreateMapper();
            }
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

            // Confirm that endpoint returns JSON ContentType
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task Get_Returns_Content()
        {
            var response = await httpClient.GetAsync(string.Empty);

            // Confirm that endpoint returns content (!= null && length > 0)
            Assert.NotNull(response.Content);
            Assert.True(response.Content.Headers.ContentLength > 0);
        }

        [Fact]
        public async Task GetList_Returns_Content()
        {
            var response = await httpClient.GetAsync("list");

            // Confirm that endpoint returns content (!= null && length > 0)
            Assert.NotNull(response.Content);
            Assert.True(response.Content.Headers.ContentLength > 0);
        }

        [Fact]
        public async Task Get_Sets_Expected_CacheControl_Header()
        {
            var response = await httpClient.GetAsync(string.Empty);

            var header = response.Headers.CacheControl;

            Assert.True(header.MaxAge.HasValue);
            Assert.Equal(TimeSpan.FromMinutes(MaxCacheAge / Minute), header.MaxAge);
            Assert.True(header.Public);
        }
    }
}
