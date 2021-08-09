using CustomerVehicleManagement.Shared.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.Application
{
    public class BasicTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string Path = "https://localhost/api/persons/list"; 
        private readonly HttpClient httpClient;
        public BasicTests(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateDefaultClient(new Uri(Path));
        }

        [Fact]
        public async Task Should_Have_Some_Persons()
        {
            var persons = await httpClient.GetFromJsonAsync<IEnumerable<PersonInListDto>>("");

            persons.Should().HaveCountGreaterOrEqualTo(1);
        }
    }
}
