using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Shared.Models.CreditCards;
using Menominee.Shared.Models.Http;
using System.Net.Http.Json;

namespace Menominee.Client.Services.CreditCards
{
    public class CreditCardDataService : DataServiceBase<CreditCardDataService>, ICreditCardDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string UriSegment = "api/creditcards";

        public CreditCardDataService(HttpClient httpClient,
            ILogger<CreditCardDataService> logger,
            IToastService toastService,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task<Result<PostResponse>> AddAsync(CreditCardToWrite fromCaller)
        {
            var entityType = "Credit Card";
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

        public async Task<Result<IReadOnlyList<CreditCardToReadInList>>> GetAllAsync()
        {
            var errorMessage = "Failed to get all credit cards";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<CreditCardToReadInList>>($"{UriSegment}/listing");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<CreditCardToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<CreditCardToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<CreditCardToRead>> GetAsync(long id)
        {
            var errorMessage = $"Failed to get credit card with id {id}";

            try
            {
                var path = UriSegment + $"/{id}";
                var result = await httpClient.GetFromJsonAsync<CreditCardToRead>(UriSegment + $"/{id}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<CreditCardToRead>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<CreditCardToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateAsync(CreditCardToWrite fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                Logger,
                card => $"{card.Name}",
                card => card.Id);
        }
    }
}
