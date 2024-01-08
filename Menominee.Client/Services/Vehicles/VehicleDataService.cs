using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Vehicles;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Vehicles;

public class VehicleDataService : DataServiceBase<VehicleDataService>, IVehicleDataService
{
    private readonly HttpClient httpClient;
    private const string UriSegment = "api/vehicles";

    public VehicleDataService(HttpClient httpClient,
        ILogger<VehicleDataService> logger,
        UriBuilderFactory uriBuilderFactory)
        : base(uriBuilderFactory, logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<Result<PostResponse>> AddAsync(VehicleToWrite fromCaller)
    {
        var entityType = "Vehicle";
        try
        {
            var result = await httpClient.AddAsync(
                UriSegment,
                fromCaller,
                Logger);

            return result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Failed to add {entityType}");
            return Result.Failure<PostResponse>("An unexpected error occurred");
        }
    }

    public async Task<Result> DeleteAsync(long id)
    {
        var failureMessage = "Failed to delete vehicle";

        try
        {
            var response = await httpClient.DeleteAsync($"{UriSegment}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Logger.LogError(message: errorMessage);
                return Result.Failure(failureMessage);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, failureMessage);
            return Result.Failure(failureMessage);
        }
    }

    public async Task<Result<VehicleToRead>> GetAsync(long id)
    {
        var errorMessage = $"Failed to get vehicle with id {id}";

        try
        {
            var result = await httpClient.GetFromJsonAsync<VehicleToRead>(UriSegment + $"/{id}");
            return result is not null
                ? Result.Success(result)
                : Result.Failure<VehicleToRead>(errorMessage);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, errorMessage);
            return Result.Failure<VehicleToRead>(errorMessage);
        }
    }

    public async Task<Result<IReadOnlyList<VehicleToRead>>> GetByParametersAsync(long customerId, SortOrder sortOrder, VehicleSortColumn sortColumn, bool includeInactive, string searchTerm)
    {
        var queryParams = new Dictionary<string, long>
        {
            {"customerId", customerId},
        };

        return await GetVehiclesAsync(queryParams, sortOrder, sortColumn, includeInactive, searchTerm);
    }

    private async Task<Result<IReadOnlyList<VehicleToRead>>> GetVehiclesAsync(Dictionary<string, long> queryParams, SortOrder sortOrder, VehicleSortColumn sortColumn, bool includeInactive, string searchTerm)
    {
        var errorMessage = "Failed to get Vehicles";

        try
        {
            var uriBuilder = CreateBaseUriBuilder($"{UriSegment}/list/{queryParams["customerId"]}");

            queryParams.Add("sortOrder", (long)sortOrder);
            queryParams.Add("sortColumn", (long)sortColumn);
            queryParams.Add("includeInactive", includeInactive ? 1 : 0);

            searchTerm = (searchTerm ?? string.Empty).Trim();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                queryParams.Add("searchTerm", long.Parse(searchTerm));

            uriBuilder = uriBuilder.CreateUriBuilderWithQueryParams(queryParams);

            var result = await httpClient.GetFromJsonAsync<IReadOnlyList<VehicleToRead>>(uriBuilder.ToString());

            return result is not null
                ? Result.Success(result)
                : Result.Failure<IReadOnlyList<VehicleToRead>>(errorMessage);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, errorMessage);
            return Result.Failure<IReadOnlyList<VehicleToRead>>(errorMessage);
        }
    }

    public async Task<Result> UpdateAsync(VehicleToWrite fromCaller)
    {
        return await httpClient.UpdateAsync(
            UriSegment,
            fromCaller,
            Logger,
            vehicle => $"{vehicle.ToString}",
            vehicle => vehicle.Id);
    }
}
