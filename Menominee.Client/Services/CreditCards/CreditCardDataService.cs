using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Shared.Models.CreditCards;
using Menominee.Shared.Models.Vehicles;
using System.Text;
using System.Text.Json;

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

        public async Task<Result<CreditCardToRead>> AddCreditCardAsync(CreditCardToWrite creditCard)
        {
            var content = new StringContent(JsonSerializer.Serialize(creditCard), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess($"{creditCard.Name} added successfully", "Added");
                return await JsonSerializer.DeserializeAsync<Result<CreditCardToRead>>(await response.Content.ReadAsStreamAsync());
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            logger.LogError(errorMessage, errorMessage);
            toastService.ShowError($"{creditCard.Name} failed to add. {response.ReasonPhrase}.", "Add Failed");

            return Result.Failure<CreditCardToRead>(errorMessage);
        }

        public async Task<Result<IReadOnlyList<CreditCardToReadInList>>> GetAllCreditCardsAsync()
        {
            try
            {
                var response = await httpClient.GetAsync($"{UriSegment}/listing");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    logger.LogError(message: errorMessage);
                    return Result.Failure<IReadOnlyList<CreditCardToReadInList>>(errorMessage);
                }

                var content = await response.Content.ReadAsStreamAsync();
                var creditCards = await JsonSerializer.DeserializeAsync<IReadOnlyList<CreditCardToReadInList>>(content);
                return Result.Success(creditCards!);

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
                var response = await httpClient.GetAsync($"{UriSegment}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = response.Content.ReadAsStringAsync().Result;
                    logger.LogError(message: errorMessage);
                    return Result.Failure<CreditCardToRead>(errorMessage);
                }

                var content = await response.Content.ReadAsStreamAsync();
                var creditCard = await JsonSerializer.DeserializeAsync<CreditCardToRead>(content);
                return Result.Success(creditCard!);

            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                logger.LogError(ex, "Failed to get credit card with id {id}", id);
                return Result.Failure<CreditCardToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateCreditCardAsync(CreditCardToWrite creditCard, long id)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(creditCard), Encoding.UTF8, MediaType);
                var response = await httpClient.PutAsync(UriSegment + $"/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = response.Content.ReadAsStringAsync().Result;
                    logger.LogError(message: errorMessage);
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
