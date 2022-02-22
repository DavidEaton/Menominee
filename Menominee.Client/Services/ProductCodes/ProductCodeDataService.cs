using CustomerVehicleManagement.Shared.Models.ProductCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
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
        private const string MediaType = "application/json";
        private const string UriSegment = "api/productcodes";

        public ProductCodeDataService(HttpClient httpClient, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.toastService = toastService;
        }

        public async Task<ProductCodeToRead> AddManufacturer(ProductCodeToWrite productCode)
        {
            var content = new StringContent(JsonSerializer.Serialize(productCode), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<ProductCodeToRead>(await response.Content.ReadAsStreamAsync());
            }

            toastService.ShowError($"Failed to add Manufacturer. {response.ReasonPhrase}.", "Add Failed");
            return null;
        }

        public async Task<IReadOnlyList<ProductCodeToReadInList>> GetAllProductCodes()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<ProductCodeToReadInList>>($"{UriSegment}/listing");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
            }

            return null;
        }

        public async Task<IReadOnlyList<ProductCodeToReadInList>> GetAllProductCodes(long mfrId, long saleCodeId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<ProductCodeToReadInList>>($"{UriSegment}/listing/{mfrId}/{saleCodeId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
            }

            return null;
        }

        public async Task<ProductCodeToRead> GetProductCode(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<ProductCodeToRead>($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
            }
            return null;
        }

        public async Task UpdateProductCode(ProductCodeToWrite productCode, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(productCode), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess("Manufacturer saved successfully", "Saved");
                return;
            }

            toastService.ShowError($"Product Code failed to update.  Id = {id}", "Save Failed");
        }
    }
}
