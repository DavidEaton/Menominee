﻿using CustomerVehicleManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.Controllers
{
    /// <summary>
    /// Uses CustomerVehicleManagement.Api.Startup::const string Connection = "Server=localhost;Database=Menominee;Trusted_Connection=True;";
    /// </summary>
    public class CustomersControllerShould : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string Path = "https://localhost/api/customers";
        private const int MaxCacheAge = 300;
        private const int Minute = 60;
        private readonly HttpClient httpClient;

        public CustomersControllerShould(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateDefaultClient(new Uri(Path));
        }

        [Fact]
        public async Task Return_Success_Status_On_GetCustomers()
        {
            var response = await httpClient.GetAsync(string.Empty);

            // Confirm that endpoint exists at, and returns data from the expected uri
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Return_Expected_MediaType_On_GetCustomers()
        {
            var response = await httpClient.GetAsync(string.Empty);

            // Confirm that endpoint returns JSON ContentType
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task Return_Content_On_GetCustomers()
        {
            var response = await httpClient.GetAsync(string.Empty);

            // Confirm that endpoint returns content (!= null && length > 0)
            Assert.NotNull(response.Content);
            Assert.True(response.Content.Headers.ContentLength > 0);
        }

        [Fact]
        public async Task Sets_Expected_CacheControl_Header_On_GetCustomers()
        {
            var response = await httpClient.GetAsync(string.Empty);

            var header = response.Headers.CacheControl;

            Assert.True(header.MaxAge.HasValue);
            Assert.Equal(TimeSpan.FromMinutes(MaxCacheAge / Minute), header.MaxAge);
            Assert.True(header.Public);
        }

        [Fact]
        public async Task Return_Customer_On_GetCustomer()
        {
            HttpContent content = (await httpClient.GetAsync(Path + "/1")).Content;
            string jsonContent = content.ReadAsStringAsync().Result;
            Customer customer = JsonSerializer.Deserialize<Customer>(jsonContent);

            Assert.NotNull(customer);
        }
    }
}