using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Common.Http;
using Menominee.Shared.Models.RepairOrders;
using System.Net.Http.Json;

namespace Menominee.Client.Components.RepairOrders
{
    public class RepairOrderDataService : DataServiceBase<RepairOrderDataService>, IRepairOrderDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string UriSegment = "api/repairorders";

        public RepairOrderDataService(HttpClient httpClient,
            ILogger<RepairOrderDataService> logger,
            IToastService toastService,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task<Result<PostResponse>> AddAsync(RepairOrderToWrite fromCaller)
        {
            var entityType = "Repair Order";
            try
            {
                var result = await httpClient.AddAsync(
                    UriSegment,
                    fromCaller,
                    Logger);

                if (result.IsSuccess)
                    toastService.ShowSuccess($"{entityType} added successfully", "Saved");

                if (result.IsFailure)
                    toastService.ShowError($"{fromCaller.RepairOrderNumber} failed to update", "Save Failed");

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to add {entityType}");
                return Result.Failure<PostResponse>("An unexpected error occurred");
            }
        }

        public async Task<Result<IReadOnlyList<RepairOrderToReadInList>>> GetAllAsync()
        {
            var errorMessage = "Failed to get Repair Orders";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<RepairOrderToReadInList>>($"{UriSegment}/listing");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<RepairOrderToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<RepairOrderToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<RepairOrderToRead>> GetAsync(long id)
        {
            var errorMessage = $"Failed to get Repair Order with id {id}";

            try
            {
                var result = await httpClient.GetFromJsonAsync<RepairOrderToRead>(UriSegment + $"/{id}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<RepairOrderToRead>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<RepairOrderToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateAsync(RepairOrderToWrite fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                Logger,
                repairOrder => $"{repairOrder.RepairOrderNumber}",
                repairOrder => repairOrder.Id);
        }
    }
}
