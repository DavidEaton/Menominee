using Blazored.Toast.Services;
using CustomerVehicleManagement.Shared.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Inventory
{
    public class InventoryItemDataService : IInventoryItemDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/inventoryitems";

        public InventoryItemDataService(HttpClient httpClient, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.toastService = toastService;
        }

        public async Task<InventoryItemToRead> AddItemAsync(InventoryItemToWrite item)
        {
            var content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<InventoryItemToRead>(await response.Content.ReadAsStreamAsync());
            }

            toastService.ShowError($"Failed to add Inventory Item. {response.ReasonPhrase}.", "Add Failed");
            return null;
        }

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetAllItemsAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<InventoryItemToReadInList>>($"{UriSegment}/listing");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
            }

            return null;
        }

        public async Task<IReadOnlyList<InventoryItemToReadInList>> GetAllItemsAsync(long mfrId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<InventoryItemToReadInList>>($"{UriSegment}/listing/{mfrId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
            }

            return null;
        }

        public async Task<InventoryItemToRead> GetItemAsync(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<InventoryItemToRead>($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
            }
            return null;
        }

        public async Task UpdateItemAsync(InventoryItemToWrite item, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess("Inventory Item saved successfully", "Saved");
                return;
            }

            toastService.ShowError($"Inventory Item failed to update:  Id = {id}", "Save Failed");
        }
    }
}
