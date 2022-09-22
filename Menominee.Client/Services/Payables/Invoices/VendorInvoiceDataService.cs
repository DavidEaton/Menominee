using Blazored.Toast.Services;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Payables.Invoices
{
    public class VendorInvoiceDataService : IVendorInvoiceDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        //private const string UriSegment = "api/payables/vendorinvoices";
        private const string UriSegment = "api/vendorinvoices";

        public VendorInvoiceDataService(HttpClient httpClient, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.toastService = toastService;
        }

        public async Task<IReadOnlyList<VendorInvoiceToReadInList>> GetAllInvoices()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<VendorInvoiceToReadInList>>($"{UriSegment}/listing");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}");
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
                Console.WriteLine($"Message: {ex.Message}");
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
