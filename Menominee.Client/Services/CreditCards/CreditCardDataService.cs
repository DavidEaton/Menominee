using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Common.Extensions;
using Menominee.Common.Http;
using Menominee.Shared.Models.CreditCards;
using Menominee.Shared.Models.Vehicles;
using System.Net.Http.Json;

namespace Menominee.Client.Services.CreditCards
{
    public class CreditCardDataService : ICreditCardDataService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<CreditCardDataService> logger;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/creditcards";

        public CreditCardDataService(HttpClient httpClient, ILogger<CreditCardDataService> logger, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.toastService = toastService;
        }

        public async Task<Result<PostResponse>> AddCreditCardAsync(CreditCardToWrite creditCard)
        {
            var result = await PostCreditCard(creditCard)
                .Bind(HttpResponseMessageExtensions.CheckResponse)
                .Bind(response => response.ReadPostResult());

            if (result.IsSuccess)
                toastService.ShowSuccess($"Card added successfully", "Saved");

            if (result.IsFailure)
            {
                toastService.ShowError($"{creditCard.Name} failed to update", "Save Failed");
                logger.LogError(result.Error);
            }

            return result;
        }

        private async Task<Result<HttpResponseMessage>> PostCreditCard(CreditCardToWrite creditCard)
        {
            try
            {
                return Result.Success(await httpClient.PostAsJsonAsync(UriSegment, creditCard));
            }
            catch (Exception)
            {
                return Result.Failure<HttpResponseMessage>("Failed to add credit card");
            }
        }

        public async Task<Result<IReadOnlyList<CreditCardToReadInList>>> GetAllCreditCardsAsync()
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<CreditCardToReadInList>>($"{UriSegment}/listing");
                return Result.Success(result!);
            }
            catch (Exception ex)
            {
                var errorMessage = "Failed to get credit all cards";
                logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<CreditCardToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<CreditCardToRead>> GetCreditCardAsync(long id)
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<CreditCardToRead>(UriSegment + $"/{id}");
                return Result.Success(result!);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to get credit card with id {id}";
                logger.LogError(ex, errorMessage);
                return Result.Failure<CreditCardToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateCreditCardAsync(CreditCardToWrite creditCard)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"{UriSegment}/{creditCard.Id}", creditCard);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = response.Content.ReadAsStringAsync().Result;
                    logger.LogError(message: errorMessage);
                    toastService.ShowError($"{creditCard.Name} failed to update", "Save Failed");
                    return Result.Failure<VehicleToRead>(errorMessage);
                }

                toastService.ShowSuccess($"{creditCard.Name} updated successfully", "Saved");
                return Result.Success();
            }
            catch (Exception ex)
            {
                var errorMessage = "Failed to update business";
                logger.LogError(ex, errorMessage);
                toastService.ShowError($"{creditCard.Name} failed to update", "Save Failed");
                return Result.Failure<VehicleToRead>(errorMessage);
            }
        }
    }
}
