﻿using CustomerVehicleManagement.Api.Persons;
using FluentAssertions;
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
        public async Task Get_Returns_Expected_Json()
        {
            /* Uses case-insensitive deserialization
               Confirms that endpoint exists at the expected uri
               Confirms that response has success status code
               Confirms Content-Type header
               Confirms that response includes content (!= null && length > 0)
            */

            var persons = await httpClient.GetFromJsonAsync<List<PersonInListDto>>(UriSegment);

            // TEST DEPENDS ON AT LEAST ONE PERSON EXISTING IN THE DATABASE
            // Modify to use test database instead of production
            persons.Should().NotBeEmpty();
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
