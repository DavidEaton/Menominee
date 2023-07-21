using Menominee.Shared.Models.ProductCodes;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.Toast.Services;

namespace Menominee.Client.Services.ProductCodes
{
    public class ProductCodeDataService : IProductCodeDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private readonly ILogger<ProductCodeDataService> logger;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/productcodes";

        public ProductCodeDataService(HttpClient httpClient, IToastService toastService, ILogger<ProductCodeDataService> logger)
        {
            this.httpClient = httpClient;
            this.toastService = toastService;
            this.logger = logger;
        }

        public async Task<ProductCodeToRead> AddProductCodeAsync(ProductCodeToWrite productCode)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var content = new StringContent(JsonSerializer.Serialize(productCode), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<ProductCodeToRead>(await response.Content.ReadAsStreamAsync(), options);
            }

            toastService.ShowError($"Failed to add Product Code. {response.ReasonPhrase}.", "Add Failed");
            return null;
        }

        public async Task<IReadOnlyList<ProductCodeToReadInList>> GetAllProductCodesAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<ProductCodeToReadInList>>($"{UriSegment}/listing");
            }
            catch (Exception)
            {
                // TODO: log exception
            }

            return null;
        }

        public async Task<IReadOnlyList<ProductCodeToReadInList>> GetAllProductCodesAsync(long mfrId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<ProductCodeToReadInList>>($"{UriSegment}/listing/{mfrId}");
            }
            catch (Exception)
            {
                // TODO: log exception
            }

            return null;
        }

        public async Task<ProductCodeToRead> GetProductCodeAsync(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<ProductCodeToRead>($"{UriSegment}/{id}");
            }
            catch (Exception)
            {
                // TODO: log exception
            }
            return null;
        }

        public async Task UpdateProductCodeAsync(ProductCodeToWrite productCode, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(productCode), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess("Product Code saved successfully", "Saved");
                return;
            }

            toastService.ShowError($"Product Code failed to update.  Id = {id}", "Save Failed");
        }

        public async Task<IReadOnlyList<ProductCodeToReadInList>> GetAllProductCodesAsync(long mfrId, long saleCodeId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<ProductCodeToReadInList>>($"{UriSegment}/listing/{mfrId}/{saleCodeId}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
            return null;
        }
    }
}
