using FluentAssertions;
using Menominee.Shared.Models.Vehicles;
using Menominee.TestingHelperLibrary.Fakers;
using Menominee.Tests.Helpers;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestingHelperLibrary;
using Xunit;

namespace Menominee.Tests.Integration.Tests;

[Collection("Integration")]
public class VehiclesControllerShould : IntegrationTestBase
{
    private const string route = "vehicles";

    public VehiclesControllerShould(IntegrationTestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_Invalid_Route_Returns_NotFound()
    {
        var response = await httpClient.GetAsync("vehicle");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
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
        var response = await httpClient
            .GetFromJsonAsync<VehicleToRead>($"{route}/{vehicleFromDatabase.Id}");

        response.Should().BeOfType<VehicleToRead>();
    }

    [Fact]
    public async Task Add_a_Vehicle()
    {
        var vehicle = CreateVehicleToPost();
        var result = await PostVehicle(vehicle);
        var id = JsonSerializerHelper.GetIdFromString(result);

        var vehicleFromEndpoint = await httpClient
            .GetFromJsonAsync<VehicleToRead>($"{route}/{id}");

        vehicleFromEndpoint.Should().BeOfType<VehicleToRead>();
    }

    [Fact]
    public async Task Update_a_Vehicle()
    {
        var vehicleToUpdate = dbContext.Vehicles.First();
        var updatedYear = 2008;
        var updatedVehicle = new VehicleToWrite()
        {
            VIN = vehicleToUpdate.VIN,
            Year = updatedYear,
            Make = vehicleToUpdate.Make,
            Model = vehicleToUpdate.Model
        };

        var response = await httpClient.PutAsJsonAsync($"{route}/{vehicleToUpdate.Id}", updatedVehicle);
        response.EnsureSuccessStatusCode();

        var vehicleFromEndpoint = await httpClient
            .GetFromJsonAsync<VehicleToRead>($"{route}/{vehicleToUpdate.Id}");

        vehicleFromEndpoint.Should().NotBeNull();
        vehicleFromEndpoint.Year.Should().Be(updatedVehicle.Year);
    }

    [Fact]
    public async Task Delete_a_Vehicle()
    {
        var vehicleToDelete = dbContext.Vehicles.First();

        vehicleToDelete.Should().NotBeNull();

        var response = await httpClient.DeleteAsync($"{route}/{vehicleToDelete.Id}");
        response.EnsureSuccessStatusCode();

        var deletedVehicleFromDatabase = dbContext.Vehicles
            .FirstOrDefault(e => e.Id == vehicleToDelete.Id);

        deletedVehicleFromDatabase.Should().BeNull();
    }

    private VehicleToWrite CreateVehicleToPost() => new()
    {
        VIN = "1M8GDM9AXKP042788",
        Year = 1997,
        Make = "Toyota",
        Model = "Land Cruiser"
    };

    private async Task<string> PostVehicle(VehicleToWrite vehicle)
    {
        var json = JsonSerializer.Serialize(vehicle, JsonSerializerHelper.DefaultSerializerOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await httpClient.PostAsync(route, content);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsStringAsync();

        var errorContent = await response.Content.ReadAsStringAsync();

        var (success, apiError) = JsonSerializerHelper.DeserializeApiError(errorContent);

        return success
            ? $"Error: {response.StatusCode} - {response.ReasonPhrase}. Message: {apiError.Message}"
            : throw new JsonException("Failed to deserialize ApiError");
    }

    public override void SeedData()
    {
        var count = 2;
        var vehicles = new VehicleFaker(false).Generate(count);

        dataSeeder.Save(vehicles);
    }

    public override void Dispose()
    {
        dbContext.Vehicles.RemoveRange(dbContext.Vehicles.ToList());
        DbContextHelper.SaveChangesWithConcurrencyHandling(dbContext);
    }
}