using Blazored.Toast.Services;
using Menominee.Shared.Models.Payables.Invoices;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Menominee.Client.Services.Payables.Invoices
{
    public class VendorInvoiceDataService : IVendorInvoiceDataService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<VendorInvoiceDataService> logger;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        //private const string UriSegment = "api/payables/vendorinvoices";
        private const string UriSegment = "api/vendorinvoices";

        public VendorInvoiceDataService(HttpClient httpClient, ILogger<VendorInvoiceDataService> logger, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.toastService = toastService;
        }

        public async Task<IReadOnlyList<VendorInvoiceToReadInList>> GetInvoices(ResourceParameters resourceParameters)
        {
            // TODO: Pass resourceParameters, NOT query string built from resourceParameters
            var parameters = $"VendorId={resourceParameters?.VendorId}&Status={resourceParameters.Status}";

            try
            {
                return await httpClient
                    .GetFromJsonAsync<IReadOnlyList<VendorInvoiceToReadInList>>($"{UriSegment}/listing?{parameters}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get all vendor invoices");
            }

            return null;
        }

        public async Task<VendorInvoiceToRead> GetInvoice(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<VendorInvoiceToRead>($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get vendoir invoice with id {id}", id);
                Console.WriteLine($"Error getting invoice #{id}: {ex.Message}");
            }

            return null;
        }

        public async Task<VendorInvoiceToRead> AddInvoice(VendorInvoiceToWrite invoice)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var content = new StringContent(JsonSerializer.Serialize(invoice), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess($"Added vendor invoice.", "Added");
                return await JsonSerializer.DeserializeAsync<VendorInvoiceToRead>(await response.Content.ReadAsStreamAsync(), options);
            }

            toastService.ShowError($"Failed to add vendor invoice. {response.ReasonPhrase}.", "Add Failed");

            return null;
        }

        public async Task UpdateInvoice(VendorInvoiceToWrite invoice, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(invoice), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess("Invoice saved successfully", "Saved");
                return;
            }

            toastService.ShowError($"Invoice failed to update:  Id = {id}", "Save Failed");
        }
    }
}
