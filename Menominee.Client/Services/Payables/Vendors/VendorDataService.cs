using Blazored.Toast.Services;
using Menominee.Common.Http;
using Menominee.Shared.Models.Payables.Vendors;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Menominee.Client.Services.Payables.Vendors
{
    public class VendorDataService : IVendorDataService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<VendorDataService> logger;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/vendors";

        public VendorDataService(HttpClient httpClient, ILogger<VendorDataService> logger, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.toastService = toastService;
        }

        public async Task<PostResponse> AddVendorAsync(VendorToWrite vendor)
        {
            var content = new StringContent(JsonSerializer.Serialize(vendor), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess($"{vendor.Name} added successfully", "Added");

                try
                {
                    return await response.Content.ReadFromJsonAsync<PostResponse>();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to add new vendor with vendor code {code}", vendor.VendorCode);
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            toastService.ShowError($"{vendor.Name} failed to add. {response.ReasonPhrase}.", "Add Failed");

            return new() { Id = 0 };
        }

        public async Task<IReadOnlyList<VendorToRead>> GetAllVendorsAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<VendorToRead>>($"{UriSegment}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get all vendors");
            }

            return null;
        }

        public async Task<VendorToRead> GetVendorAsync(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<VendorToRead>($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                Console.WriteLine($"Error getting vendor #{id}: {ex.Message}");
            }

            return null;
        }

        public async Task UpdateVendorAsync(VendorToWrite vendor, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(vendor), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess($"{vendor.Name} updated successfully", "Saved");
            }

            toastService.ShowError($"{vendor.Name} failed to update", "Save Failed");
        }
    }
}
