using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Customers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

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
                var customerName = customer.EntityType == EntityType.Person
                                              ? $"{customer.Person.Name.LastName}, {customer.Person.Name.FirstName}"
                                              : customer.Business.Name;

                toastService.ShowSuccess($"{customerName} added successfully", "Added");
                return await JsonSerializer.DeserializeAsync<CustomerToRead>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task<Result<IReadOnlyList<CustomerToReadInList>>> GetAllCustomers()
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<CustomerToReadInList>>($"{UriSegment}/listing");
                return Result.Success(result!);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to get Customers";
                logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<CustomerToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<CustomerToRead>> GetCustomer(long id)
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<CustomerToRead>(UriSegment + $"/{id}");
                return Result.Success(result!);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to get customer with id {id}";
                logger.LogError(ex, errorMessage);
                return Result.Failure<CustomerToRead>(errorMessage);
            }
        }

        public async Task UpdateCustomer(CustomerToWrite customer)
        {
            var content = new StringContent(JsonSerializer.Serialize(customer), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync(UriSegment + $"/{customer.Id}", content);
            var name =
                customer.EntityType == EntityType.Person
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