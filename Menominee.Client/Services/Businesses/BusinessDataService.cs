using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Vehicles;
using System.Text;
using System.Text.Json;

namespace Menominee.Client.Services.Businesses
{
    public class BusinessDataService : IBusinessDataService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<BusinessDataService> logger;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/businesses";

        public BusinessDataService(HttpClient httpClient, ILogger<BusinessDataService> logger, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.toastService = toastService;
        }

        public async Task<Result<BusinessToRead>> AddBusiness(BusinessToWrite business)
        {
            var content = new StringContent(JsonSerializer.Serialize(business), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess($"{business.Name} added successfully", "Added");
                return await JsonSerializer.DeserializeAsync<BusinessToRead>(await response.Content.ReadAsStreamAsync());
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            logger.LogError(errorMessage, errorMessage);
            toastService.ShowError($"{business.Name} failed to add. {response.ReasonPhrase}.", "Add Failed");

            return Result.Failure<BusinessToRead>(errorMessage);
        }

        public async Task<Result<IReadOnlyList<BusinessToReadInList>>> GetAllBusinesses()
        {
            try
            {
                var response = await httpClient.GetAsync($"{UriSegment}/list");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    logger.LogError(message: errorMessage);
                    return Result.Failure<IReadOnlyList<BusinessToReadInList>>(errorMessage);
                }

                var content = await response.Content.ReadAsStreamAsync();
                var businesses = await JsonSerializer.DeserializeAsync<IReadOnlyList<BusinessToReadInList>>(content);
                return Result.Success(businesses!);

            }
            catch (Exception ex)
            {
                var errorMessage = "Failed to get Businesses";
                logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<BusinessToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<BusinessToRead>> GetBusiness(long id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{UriSegment}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = response.Content.ReadAsStringAsync().Result;
                    logger.LogError(message: errorMessage);
                    return Result.Failure<BusinessToRead>(errorMessage);
                }

                var content = await response.Content.ReadAsStreamAsync();
                var business = await JsonSerializer.DeserializeAsync<BusinessToRead>(content);
                return Result.Success(business!);

            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                // This LogError invocation does NOT produce compiler warning
                // TODO: fix the LogError invocations that DO produce compiler warnings
                logger.LogError(ex, "Failed to get business with id {id}", id);
                return Result.Failure<BusinessToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateBusiness(BusinessToWrite business, long id)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(business), Encoding.UTF8, MediaType);
                var response = await httpClient.PutAsync(UriSegment + $"/{id}", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = response.Content.ReadAsStringAsync().Result;
                    logger.LogError(message: errorMessage);
                    return Result.Failure<VehicleToRead>(errorMessage);
                }

                toastService.ShowSuccess($"{business.Name} updated successfully", "Saved");
                return Result.Success();
            }
            catch (Exception ex)
            {
                var errorMessage = "Failed to update business";
                logger.LogError(ex, errorMessage);
                toastService.ShowError($"{business.Name} failed to update", "Save Failed");
                return Result.Failure<VehicleToRead>(errorMessage);
            }
        }
    }
}
