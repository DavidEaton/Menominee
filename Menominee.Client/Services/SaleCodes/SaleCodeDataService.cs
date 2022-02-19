using CustomerVehicleManagement.Shared.Models.SaleCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.Toast.Services;

namespace Menominee.Client.Services.SaleCodes
{
    public class SaleCodeDataService : ISaleCodeDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/salecodes";

        public SaleCodeDataService(HttpClient httpClient, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.toastService = toastService;
        }

        public async Task<SaleCodeToRead> AddSaleCode(SaleCodeToWrite saleCode)
        {
            var content = new StringContent(JsonSerializer.Serialize(saleCode), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<SaleCodeToRead>(await response.Content.ReadAsStreamAsync());
            }

            toastService.ShowError($"Failed to add Manufacturer. {response.ReasonPhrase}.", "Add Failed");
            return null;
        }

        public async Task<IReadOnlyList<SaleCodeToReadInList>> GetAllSaleCodes()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<SaleCodeToReadInList>>($"{UriSegment}/listing");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
            }

            return null;
        }

        public async Task<SaleCodeToRead> GetSaleCode(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<SaleCodeToRead>($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
            }
            return null;
        }

        public async Task UpdateSaleCode(SaleCodeToWrite saleCode, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(saleCode), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess("Manufacturer saved successfully", "Saved");
                return;
            }

            toastService.ShowError($"Sale Code {saleCode.Code} failed to update.", "Save Failed");
        }
    }
}
