using CSharpFunctionalExtensions;
using Menominee.Shared.Models;
using Menominee.Shared.Models.Vehicles;
using System.Net.Http.Json;
using System.Text.Json;

namespace Menominee.Client.Services.Vehicles;

public class VehicleDataService : IVehicleDataService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<VehicleDataService> logger;
    private const string UriSegment = "api/vehicles";

    public VehicleDataService(HttpClient httpClient, ILogger<VehicleDataService> logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<PostResult>> AddVehicle(VehicleToWrite vehicle)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(UriSegment, vehicle);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result;
                logger.LogError(message: errorMessage);
                return Result.Failure<PostResult>(errorMessage);
            }

            var data = await response.Content.ReadFromJsonAsync<PostResult>();

            return Result.Success(data!);
        }
        catch (Exception ex)
        {
            var errorMessage = "Failed to add vehicle";
            logger.LogError(ex, errorMessage);
            return Result.Failure<PostResult>(errorMessage);
        }
    }

    public async Task<Result> DeleteVehicle(long id)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"{UriSegment}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result;
                logger.LogError(message: errorMessage);
                return Result.Failure<VehicleToRead>(errorMessage);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            var errorMessage = "Failed to delete vehicle";
            logger.LogError(ex, errorMessage);
            return Result.Failure<VehicleToRead>(errorMessage);
        }
    }

    public async Task<Result<VehicleToRead>> GetVehicle(long id)
    {
        try
        {
            var response = await httpClient.GetAsync($"{UriSegment}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result;
                logger.LogError(message: errorMessage);
                return Result.Failure<VehicleToRead>(errorMessage);
            }

            var data = await JsonSerializer.DeserializeAsync<VehicleToRead>(await response.Content.ReadAsStreamAsync());
            return Result.Success(data!);
        }
        catch (Exception ex)
        {
            var errorMessage = "Failed to get vehicle";
            logger.LogError(ex, errorMessage);
            return Result.Failure<VehicleToRead>(errorMessage);
        }
    }

    public async Task<Result> UpdateVehicle(VehicleToWrite vehicle)
    {
        try
        {
            var response = await httpClient.PutAsJsonAsync($"{UriSegment}/{vehicle.Id}", vehicle);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result;
                logger.LogError(message: errorMessage);
                return Result.Failure<VehicleToRead>(errorMessage);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            var errorMessage = "Failed to update vehicle";
            logger.LogError(ex, errorMessage);
            return Result.Failure<VehicleToRead>(errorMessage);
        }
    }
}
