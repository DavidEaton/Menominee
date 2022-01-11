using MenomineePlayWASM.Shared.Dtos.RepairOrders;
using Blazored.Toast.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Services.RepairOrders
{
    public class RepairOrderDataService : IRepairOrderDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/repairorders";

        public RepairOrderDataService(HttpClient httpClient, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.toastService = toastService;
        }

        public async Task<RepairOrderToRead> AddRepairOrder(RepairOrderToWrite repairOrder)
        {
            var content = new StringContent(JsonSerializer.Serialize(repairOrder), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<RepairOrderToRead>(await response.Content.ReadAsStreamAsync());
            }

            toastService.ShowError($"Failed to add repair order. {response.ReasonPhrase}.", "Add Failed");
            return null;
        }

        public async Task<IReadOnlyList<RepairOrderToReadInList>> GetAllRepairOrders()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<RepairOrderToReadInList>>($"{UriSegment}/listing");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
            }

            return null;
        }

        public async Task<RepairOrderToRead> GetRepairOrder(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<RepairOrderToRead>($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
            }
            return null;
        }

        public async Task UpdateRepairOrder(RepairOrderToWrite repairOrder, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(repairOrder), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess("Repair Order saved successfully", "Saved");
                return;
            }

            toastService.ShowError($"Repair Order failed to update:  Id = {repairOrder.Id}", "Save Failed");
        }
    }
}
