using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Inventory.InventoryItems;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Inventory
{
    public class InventoryItemDataService : DataServiceBase<InventoryItemDataService>, IInventoryItemDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string UriSegment = "api/inventoryitems";

        public InventoryItemDataService(HttpClient httpClient,
            ILogger<InventoryItemDataService> logger,
                IToastService toastService,
                UriBuilderFactory uriBuilderFactory)
                : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task<Result<PostResponse>> AddAsync(InventoryItemToWrite fromCaller)
        {
            var entityType = "Inventory Item";
            try
            {
                var result = await httpClient.AddAsync(
                    UriSegment,
                    fromCaller,
                    Logger);

                if (result.IsSuccess)
                    toastService.ShowSuccess($"{entityType} added successfully", "Saved");

                if (result.IsFailure)
                    toastService.ShowError($"{fromCaller.ItemNumber} failed to update", "Save Failed");

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to add {entityType}");
                return Result.Failure<PostResponse>("An unexpected error occurred");
            }
        }

        public async Task<Result<IReadOnlyList<InventoryItemToReadInList>>> GetAllAsync()
        {
            var errorMessage = "Failed to get all inventory items";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<InventoryItemToReadInList>>($"{UriSegment}/listing");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<InventoryItemToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<InventoryItemToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<IReadOnlyList<InventoryItemToReadInList>>> GetByManufacturerAsync(long manufacturerId)
        {
            if (manufacturerId <= 0)
                return Result.Failure<IReadOnlyList<InventoryItemToReadInList>>("Invalid manufacturer Id.");

            var errorMessage = "Failed to get manufacturer";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<InventoryItemToReadInList>>($"{UriSegment}/listing?manufacturerId={manufacturerId}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<InventoryItemToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<InventoryItemToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<InventoryItemToRead>> GetAsync(long id)
        {
            var errorMessage = $"Failed to get item with id {id}";

            try
            {
                var result = await httpClient.GetFromJsonAsync<InventoryItemToRead>(UriSegment + $"/{id}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<InventoryItemToRead>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<InventoryItemToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateAsync(InventoryItemToWrite fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                Logger,
                item => $"{item.ItemNumber}",
                item => item.Id);
        }
    }
}