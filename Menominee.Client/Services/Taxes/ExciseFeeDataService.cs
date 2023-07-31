using Blazored.Toast.Services;
using Menominee.Shared.Models.Taxes;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Menominee.Client.Services.Taxes
{
    public class ExciseFeeDataService : IExciseFeeDataService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<ExciseFeeDataService> logger;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/excisefees";

        public ExciseFeeDataService(HttpClient httpClient, ILogger<ExciseFeeDataService> logger, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.toastService = toastService;
        }

        public async Task<ExciseFeeToRead> AddExciseFeeAsync(ExciseFeeToWrite exciseFee)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var content = new StringContent(JsonSerializer.Serialize(exciseFee), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<ExciseFeeToRead>(await response.Content.ReadAsStreamAsync(), options);
            }

            toastService.ShowError($"Failed to add Excise Fee. {response.ReasonPhrase}.", "Add Failed");

            return null;
        }

        public async Task<IReadOnlyList<ExciseFeeToReadInList>> GetAllExciseFeesAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<ExciseFeeToReadInList>>($"{UriSegment}/listing");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get all excise fees");
            }

            return null;
        }

        public async Task<ExciseFeeToRead> GetExciseFeeAsync(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<ExciseFeeToRead>($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get excise fee with id {id}", id);
            }

            return null;
        }

        public async Task UpdateExciseFeeAsync(ExciseFeeToWrite exciseFee, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(exciseFee), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess("Excise Fee saved successfully", "Saved");
                return;
            }

            toastService.ShowError($"Excise Fee failed to update.  Id = {id}", "Save Failed");
        }
    }
}
