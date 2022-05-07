using Blazored.Toast.Services;
using CustomerVehicleManagement.Shared.Models.Taxes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Taxes
{
    public class SalesTaxDataService : ISalesTaxDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/salestaxes";

        public SalesTaxDataService(HttpClient httpClient, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.toastService = toastService;
        }

        public async Task<SalesTaxToRead> AddSalesTaxAsync(SalesTaxToWrite salesTax)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var content = new StringContent(JsonSerializer.Serialize(salesTax), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<SalesTaxToRead>(await response.Content.ReadAsStreamAsync(), options);
            }

            toastService.ShowError($"Failed to add Sales Tax. {response.ReasonPhrase}.", "Add Failed");
            return null;
        }

        public async Task<IReadOnlyList<SalesTaxToReadInList>> GetAllSalesTaxesAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<SalesTaxToReadInList>>($"{UriSegment}/listing");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
            }

            return null;
        }

        public async Task<SalesTaxToRead> GetSalesTaxAsync(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<SalesTaxToRead>($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
            }
            return null;
        }

        public async Task UpdateSalesTaxAsync(long id, SalesTaxToWrite salesTax)
        {
            var content = new StringContent(JsonSerializer.Serialize(salesTax), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess("Sales Tax saved successfully", "Saved");
                return;
            }

            toastService.ShowError($"Sales Tax failed to update.  Id = {id}", "Save Failed");
        }
    }
}
