using FluentAssertions;
using Menominee.Common.Http;
using Menominee.Shared.Models.Vehicles;
using Menominee.TestingHelperLibrary.Fakers;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Menominee.Tests.Integration.Tests;

[Collection("Integration")]
public class VehiclesControllerShould : IntegrationTestBase
{
    protected readonly string Route = "vehicles";

    public VehiclesControllerShould(IntegrationTestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_Invalid_Id_Returns_NotFound()
    {
        var response = await HttpClient.GetAsync($"{Route}/0");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Returns_Expected_Response()
    {
        var vehicleFromDatabase = DbContext.Vehicles.First();

        var response = await HttpClient.GetFromJsonAsync<VehicleToRead>($"{Route}/{vehicleFromDatabase.Id}");

        response.Should().BeOfType<VehicleToRead>();
    }

    [Fact]
    public async Task Add_a_Vehicle()
    {
        var vehicle = new VehicleFaker(false).Generate();
        var vehicleToAdd = VehicleHelper.ConvertToWriteDto(vehicle);
        var result = await HttpClient.PostAsJsonAsync(Route, vehicleToAdd);

        var id = (await result.Content.ReadFromJsonAsync<PostResponse>()).Id;
        var vehicleFromEndpoint = await HttpClient
            .GetFromJsonAsync<VehicleToRead>($"{Route}/{id}");

        vehicleFromEndpoint.Should().BeOfType<VehicleToRead>();
    }

    [Fact]
    public async Task Update_a_Vehicle()
    {
        var vehicleToUpdate = DbContext.Vehicles.First();
        var updatedYear = 2008;
        var updatedVehicle = new VehicleToWrite()
        {
            Id = vehicleToUpdate.Id,
            VIN = vehicleToUpdate.VIN,
            Year = updatedYear,
            Make = vehicleToUpdate.Make,
            Model = vehicleToUpdate.Model
        };

        var response = await HttpClient.PutAsJsonAsync($"{Route}/{vehicleToUpdate.Id}", updatedVehicle);
        response.EnsureSuccessStatusCode();

        var vehicleFromEndpoint = await HttpClient
            .GetFromJsonAsync<VehicleToRead>($"{Route}/{vehicleToUpdate.Id}");

        vehicleFromEndpoint.Should().NotBeNull();
        vehicleFromEndpoint.Year.Should().Be(updatedVehicle.Year);
    }

    [Fact]
    public async Task Delete_a_Vehicle()
    {
        var vehicleToDelete = DbContext.Vehicles.First();
        vehicleToDelete.Should().NotBeNull();

        var response = await HttpClient.DeleteAsync($"{Route}/{vehicleToDelete.Id}");
        response.EnsureSuccessStatusCode();
        var deletedVehicleFromDatabase = DbContext.Vehicles
            .FirstOrDefault(vehicle => vehicle.Id.Equals(vehicleToDelete.Id));

        deletedVehicleFromDatabase.Should().BeNull();
    }

    public override void SeedData()
    {
        var count = 2;
        var vehicles = new VehicleFaker(false).Generate(count);
        DataSeeder.Save(vehicles);
    }
}