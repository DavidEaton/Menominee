using CustomerVehicleManagement.Api.Data.Dtos;
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
    public class BasicTests
        : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly HttpClient client;
        public BasicTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost:44378/api")
            });
        }

        [Fact]
        public async Task Post_DeleteAllMessagesHandler_ReturnsRedirectToRoot()
        {
            var persons = await client.GetFromJsonAsync<IEnumerable<PersonInListDto>>("/api/persons/list");


            persons.Should().HaveCountGreaterOrEqualTo(2);
        }



        //[Theory]
        //[InlineData("/")]
        //[InlineData("/Index")]
        //[InlineData("/About")]
        //[InlineData("/Privacy")]
        //[InlineData("/Contact")]
        //public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        //{
        //    // Arrange
        //    var client = factory.CreateClient();

        //    // Act
        //    var response = await client.GetAsync(url);

        //    // Assert
        //    response.EnsureSuccessStatusCode(); // Status Code 200-299
        //    Assert.Equal("text/html; charset=utf-8",
        //        response.Content.Headers.ContentType.ToString());
        //}



    }
}
