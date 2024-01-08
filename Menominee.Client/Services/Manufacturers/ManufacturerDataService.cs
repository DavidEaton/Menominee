using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Manufacturers;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Manufacturers
{
    public class ManufacturerDataService : DataServiceBase<ManufacturerDataService>, IManufacturerDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string UriSegment = "api/manufacturers";

        public ManufacturerDataService(HttpClient httpClient,
            ILogger<ManufacturerDataService> logger,
            IToastService toastService,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task<Result<PostResponse>> AddAsync(ManufacturerToWrite fromCaller)
        {
            var entityType = "Manufacturer";
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

        public async Task<Result<IReadOnlyList<ManufacturerToReadInList>>> GetAllAsync()
        {
            var errorMessage = "Failed to get all manufacturers";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<ManufacturerToReadInList>>($"{UriSegment}/list");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<ManufacturerToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<ManufacturerToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<ManufacturerToRead>> GetAsync(long id)
        {
            return await GetManufacturerFromUrlAsync($"{UriSegment}/{id}");
        }

        public async Task<Result<ManufacturerToRead>> GetAsync(string code)
        {
            return await GetManufacturerFromUrlAsync($"{UriSegment}/code/{code}");
        }

        private async Task<Result<ManufacturerToRead>> GetManufacturerFromUrlAsync(string url)
        {
            var errorMessage = $"Failed to get Manufacturer from URL: {url}";

            try
            {
                var result = await httpClient.GetFromJsonAsync<ManufacturerToRead>(url);
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<ManufacturerToRead>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<ManufacturerToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateAsync(ManufacturerToWrite fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                Logger,
                manufacturer => $"{manufacturer.Name}",
                manufacturer => manufacturer.Id);
        }
    }
}
