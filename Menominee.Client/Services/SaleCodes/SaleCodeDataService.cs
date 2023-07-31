using Menominee.Shared.Models.SaleCodes;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.Toast.Services;

namespace Menominee.Client.Services.SaleCodes
{
    public class SaleCodeDataService : ISaleCodeDataService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<SaleCodeDataService> logger;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/salecodes";

        public SaleCodeDataService(HttpClient httpClient, ILogger<SaleCodeDataService> logger, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.toastService = toastService;
        }

        public async Task<SaleCodeToRead> AddSaleCodeAsync(SaleCodeToWrite saleCode)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var content = new StringContent(JsonSerializer.Serialize(saleCode), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<SaleCodeToRead>(await response.Content.ReadAsStreamAsync(), options);
            }

            toastService.ShowError($"Failed to add Sale Code. {response.ReasonPhrase}.", "Add Failed");

            return null;
        }

        public async Task<IReadOnlyList<SaleCodeToReadInList>> GetAllSaleCodesAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<SaleCodeToReadInList>>($"{UriSegment}/listing");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get all sale codes");
            }

            return null;
        }

        public async Task<IReadOnlyList<SaleCodeShopSuppliesToReadInList>> GetAllSaleCodeShopSuppliesAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<SaleCodeShopSuppliesToReadInList>>($"{UriSegment}/shopsupplieslist");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get all shop supplies");
            }

            return null;
        }

        public async Task<SaleCodeToRead> GetSaleCodeAsync(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<SaleCodeToRead>($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get sale code with id {id}", id);
            }

            return null;
        }

        public async Task UpdateSaleCodeAsync(SaleCodeToWrite saleCode, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(saleCode), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                //toastService.ShowSuccess("Sale Code saved successfully", "Saved");
                return;
            }

            toastService.ShowError($"Sale Code {saleCode.Code} failed to update.", "Save Failed");
        }
    }
}
