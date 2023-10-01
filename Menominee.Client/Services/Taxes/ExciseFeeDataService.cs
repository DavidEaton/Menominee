using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Common.Http;
using Menominee.Shared.Models.Taxes;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Taxes
{
    public class ExciseFeeDataService : DataServiceBase<ExciseFeeDataService>, IExciseFeeDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string UriSegment = "api/excisefees";

        public ExciseFeeDataService(HttpClient httpClient,
            ILogger<ExciseFeeDataService> logger,
            IToastService toastService,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task<Result<PostResponse>> AddAsync(ExciseFeeToWrite fromCaller)
        {
            var entityType = "Excise Fee";
            try
            {
                var result = await httpClient.AddAsync(
                    UriSegment,
                    fromCaller,
                    Logger);

                if (result.IsSuccess)
                    toastService.ShowSuccess($"{entityType} added successfully", "Saved");

                if (result.IsFailure)
                    toastService.ShowError($"{fromCaller.FeeType} failed to update", "Save Failed");

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to add {entityType}");
                return Result.Failure<PostResponse>("An unexpected error occurred");
            }
        }

        public async Task<Result<IReadOnlyList<ExciseFeeToReadInList>>> GetAllAsync()
        {
            var errorMessage = "Failed to get all excise fees";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<ExciseFeeToReadInList>>($"{UriSegment}/listing");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<ExciseFeeToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<ExciseFeeToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<ExciseFeeToRead>> GetAsync(long id)
        {
            var errorMessage = $"Failed to get excise fee with id {id}";

            try
            {
                var result = await httpClient.GetFromJsonAsync<ExciseFeeToRead>(UriSegment + $"/{id}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<ExciseFeeToRead>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<ExciseFeeToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateAsync(ExciseFeeToWrite fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                Logger,
                exciseFee => $"{exciseFee.Description}",
                exciseFee => exciseFee.Id);
        }
    }
}
