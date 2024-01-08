using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Taxes;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Taxes
{
    public class SalesTaxDataService : DataServiceBase<SalesTaxDataService>, ISalesTaxDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string UriSegment = "api/salestaxes";

        public SalesTaxDataService(HttpClient httpClient,
            ILogger<SalesTaxDataService> logger,
            IToastService toastService,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task<Result<PostResponse>> AddAsync(SalesTaxToWrite fromCaller)
        {
            var entityType = "Sales Tax";
            try
            {
                var result = await httpClient.AddAsync(
                    UriSegment,
                    fromCaller,
                    Logger);

                if (result.IsSuccess)
                    toastService.ShowSuccess($"{entityType} added successfully", "Saved");

                if (result.IsFailure)
                    toastService.ShowError($"{fromCaller.Description} failed to update", "Save Failed");

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to add {entityType}");
                return Result.Failure<PostResponse>("An unexpected error occurred");
            }
        }

        public async Task<Result<IReadOnlyList<SalesTaxToReadInList>>> GetAllAsync()
        {
            var errorMessage = "Failed to get all Sales Taxes";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<SalesTaxToReadInList>>($"{UriSegment}/list");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<SalesTaxToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<SalesTaxToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<SalesTaxToRead>> GetAsync(long id)
        {
            var errorMessage = $"Failed to get Sales Tax with id {id}";

            try
            {
                var result = await httpClient.GetFromJsonAsync<SalesTaxToRead>(UriSegment + $"/{id}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<SalesTaxToRead>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<SalesTaxToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateAsync(SalesTaxToWrite fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                Logger,
                salesTax => $"{salesTax.ToString}",
                salesTax => salesTax.Id);
        }
    }
}
