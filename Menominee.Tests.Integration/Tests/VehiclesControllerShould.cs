using Azure;
using FluentAssertions;
using Menominee.Api.Data;
using Menominee.Shared.Models;
using Menominee.Shared.Models.Vehicles;
using Menominee.TestingHelperLibrary.Fakers;
using Menominee.Tests.Helpers;
using Menominee.Tests.Integration.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Menominee.Tests.Integration.Tests;

[Collection("Integration")]
public class VehiclesControllerShould : IClassFixture<IntegrationTestWebApplicationFactory>, IDisposable
{
    private readonly HttpClient httpClient;
    private readonly IDataSeeder dataSeeder;
    private readonly ApplicationDbContext dbContext;
    private const string route = "vehicles";

    public VehiclesControllerShould(IntegrationTestWebApplicationFactory factory)
    {
        httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("https://localhost/api/")
        });

        dataSeeder = factory.Services.GetRequiredService<IDataSeeder>();
        dbContext = factory.Services.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        SeedData();
    }

    [Fact]
    public async Task Get_Invalid_Id_Returns_NotFound()
    {
        var response = await httpClient.GetAsync($"{route}/0");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Returns_Expected_Response()
    {
        var vehicleFromDatabase = dbContext.Vehicles.First();

        var response = await httpClient.GetFromJsonAsync<VehicleToRead>($"{route}/{vehicleFromDatabase.Id}");

        response.Should().BeOfType<VehicleToRead>();
    }

    [Fact]
    public async Task Add_a_Vehicle()
    {
        var vehicle = new VehicleFaker(false).Generate();
        var vehicleToAdd = VehicleHelper.ConvertToWriteDto(vehicle);
        var result = await httpClient.PostAsJsonAsync(route, vehicleToAdd);

        var id = (await result.Content.ReadFromJsonAsync<PostResult>()).Id;
        var vehicleFromEndpoint = await httpClient
            .GetFromJsonAsync<VehicleToRead>($"{route}/{id}");

        vehicleFromEndpoint.Should().BeOfType<VehicleToRead>();
    }

    [Fact]
    public async Task Update_a_Vehicle()
    {
        var vehicleToUpdate = dbContext.Vehicles.First();
        var updatedYear = 2008;

        var updatedVehicle = VehicleHelper.ConvertToWriteDto(vehicleToUpdate);
        updatedVehicle.Year = updatedYear;

        var response = await httpClient.PutAsJsonAsync($"{route}/{vehicleToUpdate.Id}", updatedVehicle);
        response.EnsureSuccessStatusCode();

        var vehicleFromEndpoint = await httpClient
            .GetFromJsonAsync<VehicleToRead>($"{route}/{vehicleToUpdate.Id}");

        vehicleFromEndpoint.Should().NotBeNull();
        vehicleFromEndpoint.Year.Should().Be(updatedYear);
    }

    [Fact]
    public async Task Delete_a_Vehicle()
    {
        var vehicleToDelete = dbContext.Vehicles.First();

        vehicleToDelete.Should().NotBeNull();

        var response = await httpClient.DeleteAsync($"{route}/{vehicleToDelete.Id}");
        response.EnsureSuccessStatusCode();

        var deletedVehicleFromDatabase = dbContext.Vehicles
            .FirstOrDefault(e => e.Id.Equals(vehicleToDelete.Id));

        deletedVehicleFromDatabase.Should().BeNull();
    }

    private void SeedData()
    {
        var count = 2;
        var vehicles = new VehicleFaker(false).Generate(count);

        dataSeeder.Save(vehicles);
    }

    public void Dispose()
    {
        dbContext.Vehicles.RemoveRange(dbContext.Vehicles.ToList());
        DbContextHelper.SaveChangesWithConcurrencyHandling(dbContext);
    }
}