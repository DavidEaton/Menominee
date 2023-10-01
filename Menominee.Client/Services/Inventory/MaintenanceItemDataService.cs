using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Common.Http;
using Menominee.Shared.Models.Inventory.MaintenanceItems;
using Menominee.Shared.Models.Vehicles;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Inventory
{
    public class MaintenanceItemDataService : DataServiceBase<MaintenanceItemDataService>, IMaintenanceItemDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string UriSegment = "api/maintenanceitems";

        public MaintenanceItemDataService(HttpClient httpClient,
            ILogger<MaintenanceItemDataService> logger,
            IToastService toastService,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task<Result<PostResponse>> AddAsync(MaintenanceItemToWrite fromCaller)
        {
            var entityType = "Maintenance Item";
            try
            {
                var result = await httpClient.AddAsync(
                    UriSegment,
                    fromCaller,
                    Logger);

                if (result.IsSuccess)
                    toastService.ShowSuccess($"{entityType} added successfully", "Saved");

                if (result.IsFailure)
                    toastService.ShowError($"{fromCaller.Item.ItemNumber} failed to update", "Save Failed");

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to add {entityType}");
                return Result.Failure<PostResponse>("An unexpected error occurred");
            }
        }

        public async Task<Result<IReadOnlyList<MaintenanceItemToReadInList>>> GetAllAsync()
        {
            var errorMessage = "Failed to get all maintenance items";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<MaintenanceItemToReadInList>>($"{UriSegment}/listing");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<MaintenanceItemToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<MaintenanceItemToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<MaintenanceItemToRead>> GetAsync(long id)
        {
            var errorMessage = $"Failed to get maintenance item with id {id}";

            try
            {
                var result = await httpClient.GetFromJsonAsync<MaintenanceItemToRead>(UriSegment + $"/{id}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<MaintenanceItemToRead>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<MaintenanceItemToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateAsync(MaintenanceItemToWrite fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                Logger,
                item => $"{item.Item.ItemNumber}",
                item => item.Id);
        }

        public async Task<Result> DeleteAsync(long id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{UriSegment}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Logger.LogError(message: errorMessage);
                    return Result.Failure<VehicleToRead>(errorMessage);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                var errorMessage = "Failed to delete item";
                Logger.LogError(ex, errorMessage);
                return Result.Failure<VehicleToRead>(errorMessage);
            }
        }
    }
}
