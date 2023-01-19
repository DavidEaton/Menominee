using Blazored.Toast.Services;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Payables.PaymentMethods
{
    public class VendorInvoicePaymentMethodDataService : IVendorInvoicePaymentMethodDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/vendorinvoicepaymentmethods";

        public VendorInvoicePaymentMethodDataService(HttpClient httpClient, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.toastService = toastService;
        }

        public async Task<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>> GetAllPaymentMethodsAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<VendorInvoicePaymentMethodToReadInList>>($"{UriSegment}/listing");
            }
            catch (Exception)
            {
                // TODO: log exception
            }

            return null;
        }

        public async Task<VendorInvoicePaymentMethodToRead> GetPaymentMethodAsync(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<VendorInvoicePaymentMethodToRead>($"{UriSegment}/{id}");
            }
            catch (Exception)
            {
                // TODO: log exception
            }
            return null;
        }

        public async Task<VendorInvoicePaymentMethodToRead> AddPaymentMethodAsync(VendorInvoicePaymentMethodToWrite payMethod)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var content = new StringContent(JsonSerializer.Serialize(payMethod), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<VendorInvoicePaymentMethodToRead>(await response.Content.ReadAsStreamAsync(), options);
            }

            toastService.ShowError($"Failed to add payment method. {response.ReasonPhrase}.", "Add Failed");
            return null;
        }

        public async Task UpdatePaymentMethodAsync(VendorInvoicePaymentMethodToWrite payMethod, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(payMethod), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync($"{UriSegment}/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess("Payment method saved successfully", "Saved");
                return;
            }

            toastService.ShowError($"Payment method failed to update:  Id = {id}", "Save Failed");
        }

        public async Task DeletePaymentMethodAsync(long id)
        {
            var response = await httpClient.DeleteAsync($"{UriSegment}/{id}");

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess("Payment method deleted successfully", "Deleted");
                return;
            }

            toastService.ShowError($"Failed to delete payment method:  Id = {id}", "Delete Failed");
        }
    }
}
