using Menominee.Shared.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.Toast.Services;
using Menominee.Shared.Models.Businesses;

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

        public async Task<BusinessToRead> AddBusiness(BusinessToWrite business)
        {
            var content = new StringContent(JsonSerializer.Serialize(business), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess($"{business.Name} added successfully", "Added");
                return await JsonSerializer.DeserializeAsync<BusinessToRead>(await response.Content.ReadAsStreamAsync());
            }

            toastService.ShowError($"{business.Name} failed to add. {response.ReasonPhrase}.", "Add Failed");

            return null;
        }

        public async Task<IReadOnlyList<BusinessToReadInList>> GetAllBusinesses()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<BusinessToReadInList>>($"{UriSegment}/list");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get all businesses");
            }

            return null;
        }

        public async Task<BusinessToRead> GetBusiness(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<BusinessToRead>(UriSegment + $"/{id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get business with id {id}", id);
            }

            return null;
        }

        public async Task UpdateBusiness(BusinessToWrite business, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(business), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync(UriSegment + $"/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess($"{business.Name} updated successfully", "Saved");
                return;
            }

            toastService.ShowError($"{business.Name} failed to update", "Save Failed");
        }
    }
}
