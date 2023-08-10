using Blazored.Toast.Services;
using Menominee.Shared.Models.Customers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static Menominee.Common.Enums.EntityType;

namespace Menominee.Client.Services.Customers
{
    public class CustomerDataService : ICustomerDataService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<CustomerDataService> logger;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/customers";

        public CustomerDataService(HttpClient httpClient, ILogger<CustomerDataService> logger, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.toastService = toastService;
        }

        public async Task<CustomerToRead> AddCustomer(CustomerToWrite customer)
        {
            var content = new StringContent(JsonSerializer.Serialize(customer), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                var customerName = customer.EntityType == Person
                                              ? $"{customer.Person.Name.LastName}, {customer.Person.Name.FirstName}"
                                              : customer.Business.Name;

                toastService.ShowSuccess($"{customerName} added successfully", "Added");
                return await JsonSerializer.DeserializeAsync<CustomerToRead>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task<IReadOnlyList<CustomerToReadInList>> GetAllCustomers()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<CustomerToReadInList>>($"{UriSegment}/list");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get all customers");
            }

            return null;
        }

        public async Task<CustomerToRead> GetCustomer(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<CustomerToRead>(UriSegment + $"/{id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get customer with id {id}", id);
            }

            return null;
        }

        public async Task UpdateCustomer(CustomerToWrite customer, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(customer), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync(UriSegment + $"/{id}", content);
            var name =
                customer.EntityType == Person
                ? $"{customer.Person.Name.LastName}, {customer.Person.Name.FirstName}"
                : customer.Business.Name;

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess($"{name} updated successfully", "Saved");
                return;
            }

            toastService.ShowError($"{name} failed to update", "Save Failed");
        }

        public async Task DeleteCustomer(long id)
        {
            try
            {
                await httpClient.DeleteAsync($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete customer with id {id}", id);
            }
        }
    }
}