using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.ProductCodes;
using System.Net.Http.Json;

namespace Menominee.Client.Services.ProductCodes
{
    public class ProductCodeDataService : DataServiceBase<ProductCodeDataService>, IProductCodeDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string UriSegment = "api/productcodes";

        public ProductCodeDataService(HttpClient httpClient,
            ILogger<ProductCodeDataService> logger,
            IToastService toastService,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task<Result<PostResponse>> AddAsync(ProductCodeToWrite fromCaller)
        {
            var entityType = "Product Code";
            try
            {
                var result = await httpClient.AddAsync(
                    UriSegment,
                    fromCaller,
                    Logger);

                if (result.IsSuccess)
                    toastService.ShowSuccess($"{entityType} added successfully", "Saved");

                if (result.IsFailure)
                    toastService.ShowError($"{fromCaller.Name} failed to update", "Save Failed");

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to add {entityType}");
                return Result.Failure<PostResponse>("An unexpected error occurred");
            }
        }

        public async Task<Result<IReadOnlyList<ProductCodeToReadInList>>> GetAllAsync()
        {
            var errorMessage = "Failed to get product codes";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<ProductCodeToReadInList>>($"{UriSegment}/list");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<ProductCodeToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<ProductCodeToReadInList>>(errorMessage);
            }

        }

        public async Task<Result<IReadOnlyList<ProductCodeToReadInList>>> GetByManufacturerAsync(long manufacturerId)
        {
            var errorMessage = $"Failed to get product codes with manufacturer id {manufacturerId}";

            try
            {
                var uri = BuildUriWithQueryParams(UriSegment + "/listing", new Dictionary<string, long> { { "manufacturerId", manufacturerId } });
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<ProductCodeToReadInList>>(uri);
                return result is not null
                                    ? Result.Success(result)
                                    : Result.Failure<IReadOnlyList<ProductCodeToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage, manufacturerId);
                return Result.Failure<IReadOnlyList<ProductCodeToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<ProductCodeToRead>> GetAsync(long id)
        {
            var errorMessage = $"Failed to get product code with id {id}";

            try
            {
                var result = await httpClient.GetFromJsonAsync<ProductCodeToRead>(UriSegment + $"/{id}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<ProductCodeToRead>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<ProductCodeToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateAsync(ProductCodeToWrite fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                Logger,
                productCode => $"{productCode.ToString}",
                productCode => productCode.Id);
        }

        public async Task<Result<IReadOnlyList<ProductCodeToReadInList>>> GetByManufacturerAndSaleCodeAsync(long manufacturerId, long saleCodeId)
        {
            return await GetProductCodesAsync(new Dictionary<string, long>
            {
                {"manufacturerId", manufacturerId},
                {"saleCodeId", saleCodeId}
            });
        }

        private async Task<Result<IReadOnlyList<ProductCodeToReadInList>>> GetProductCodesAsync(Dictionary<string, long> queryParams)
        {
            var errorMessage = "Failed to get Product Codes";

            try
            {
                var uri = BuildUriWithQueryParams(UriSegment, queryParams);
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<ProductCodeToReadInList>>(uri);

                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<ProductCodeToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<ProductCodeToReadInList>>(errorMessage);
            }
        }
    }
}
