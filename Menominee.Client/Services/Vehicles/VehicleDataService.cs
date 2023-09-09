using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Shared.Models;
using Menominee.Shared.Models.Vehicles;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;

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

    public async Task<Result<IReadOnlyList<VehicleToRead>>> GetVehicles(long customerId, SortOrder sortOrder, VehicleSortColumn sortColumn, bool includeInactive, string searchTerm)
    {
        try
        {
            var uriBuilder = new UriBuilder(
                "https",
                httpClient.BaseAddress!.Host,
                httpClient.BaseAddress.Port,
                $"{UriSegment}/list/{customerId}");

            var queryBuilder = new QueryBuilder
            {
                { "sortOrder", sortOrder.ToString() },
                { "sortColumn", sortColumn.ToString() },
                { "includeInactive", includeInactive.ToString() }
            };

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                queryBuilder.Add("searchTerm", searchTerm);
            }

            uriBuilder.Query = queryBuilder.ToString();

            var data = await httpClient.GetFromJsonAsync<IReadOnlyList<VehicleToRead>>(uriBuilder.Uri);

            return Result.Success(data!);
        }
        catch (Exception ex)
        {
            var errorMessage = "Failed to get vehicles";
            logger.LogError(ex, errorMessage);
            return Result.Failure<IReadOnlyList<VehicleToRead>>(errorMessage);
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
