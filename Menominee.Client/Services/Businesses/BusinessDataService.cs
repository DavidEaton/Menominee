using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Http;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Businesses
{
    public class BusinessDataService : DataServiceBase<BusinessDataService>, IBusinessDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string UriSegment = "api/businesses";

        public BusinessDataService(HttpClient httpClient,
            ILogger<BusinessDataService> logger,
            IToastService toastService,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task<Result<PostResponse>> AddAsync(BusinessToWrite fromCaller)
        {
            var entityType = "Business";
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

        public async Task<Result<IReadOnlyList<BusinessToReadInList>>> GetAllAsync()
        {
            var errorMessage = "Failed to get all businesses";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<BusinessToReadInList>>($"{UriSegment}/list");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<BusinessToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<BusinessToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<BusinessToRead>> GetAsync(long id)
        {
            var errorMessage = $"Failed to get business with id {id}";

            try
            {
                var result = await httpClient.GetFromJsonAsync<BusinessToRead>(UriSegment + $"/{id}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<BusinessToRead>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<BusinessToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateAsync(BusinessToWrite fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                Logger,
                business => $"{business.Name}",
                business => business.Id);
        }
    }
}
