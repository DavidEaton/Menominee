using Blazored.Toast.Services;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Manufacturers
{
    public class ManufacturerDataService : IManufacturerDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/manufacturers";

        public ManufacturerDataService(HttpClient httpClient, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.toastService = toastService;
        }

        public async Task<ManufacturerToRead> AddManufacturerAsync(ManufacturerToWrite manufacturer)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var content = new StringContent(JsonSerializer.Serialize(manufacturer), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<ManufacturerToRead>(await response.Content.ReadAsStreamAsync(), options);
            }

            toastService.ShowError($"Failed to add Manufacturer. {response.ReasonPhrase}.", "Add Failed");
            return null;
        }

        public async Task<IReadOnlyList<ManufacturerToReadInList>> GetAllManufacturersAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<ManufacturerToReadInList>>($"{UriSegment}/listing");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
            }

            return null;
        }

        public async Task<ManufacturerToRead> GetManufacturerAsync(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<ManufacturerToRead>($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
            }
            return null;
        }

        public async Task UpdateManufacturerAsync(ManufacturerToWrite manufacturer, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(manufacturer), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess("Manufacturer saved successfully", "Saved");
                return;
            }

            toastService.ShowError($"Manufacturer failed to update:  Id = {id}", "Save Failed");
        }
    }
}
