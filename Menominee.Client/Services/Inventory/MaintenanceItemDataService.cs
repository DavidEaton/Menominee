using Blazored.Toast.Services;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;
using Menominee.Shared.Models.Inventory.MaintenanceItems;

namespace Menominee.Client.Services.Inventory
{
    public class MaintenanceItemDataService : IMaintenanceItemDataService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<MaintenanceItemDataService> logger;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/maintenanceitems";

        public MaintenanceItemDataService(HttpClient httpClient, ILogger<MaintenanceItemDataService> logger, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.toastService = toastService;
        }

        public async Task<MaintenanceItemToRead> AddItemAsync(MaintenanceItemToWrite item)
        {
            var content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                //toastService.ShowSuccess($"{organization.Name} added successfully", "Added");
                return await JsonSerializer.DeserializeAsync<MaintenanceItemToRead>(await response.Content.ReadAsStreamAsync());
            }

            //toastService.ShowError($"{organization.Name} failed to add. {response.ReasonPhrase}.", "Add Failed");
            return null;
        }

        public async Task<IReadOnlyList<MaintenanceItemToReadInList>> GetAllItemsAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<MaintenanceItemToReadInList>>($"{UriSegment}/listing");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get all maintenance items");
                Console.WriteLine($"Message: {ex.Message}");
            }

            return null;
        }

        public async Task<MaintenanceItemToRead> GetItemAsync(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<MaintenanceItemToRead>($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get maintenance item with id {id}", id);
                Console.WriteLine($"Message: {ex.Message}");
            }

            return null;
        }

        public async Task UpdateItemAsync(MaintenanceItemToWrite item, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                //toastService.ShowSuccess("Inventory Item saved successfully", "Saved");
                return;
            }

            //toastService.ShowError($"Inventory Item failed to update:  Id = {id}", "Save Failed");
        }

        public async Task DeleteItemAsync(long id)
        {
            try
            {
                await httpClient.DeleteAsync($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete maintenance item with id {id}", id);
                Console.WriteLine($"Message :{ex.Message}");
            }
        }
    }
}
