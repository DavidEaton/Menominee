using CSharpFunctionalExtensions;
using Menominee.Common.Extensions;
using Menominee.Common.Http;
using Menominee.Shared.Models.Customers;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Customers
{
    public class CustomerDataService : ICustomerDataService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<CustomerDataService> logger;
        private const string UriSegment = "api/customers";

        public CustomerDataService(HttpClient httpClient, ILogger<CustomerDataService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<Result<PostResponse>> AddCustomer(CustomerToWrite customer)
        {
            var result = await PostCustomer(customer)
                .Bind(HttpResponseMessageExtensions.CheckResponse)
                .Bind(ReadPostResult);

            if (result.IsFailure)
                logger.LogError(result.Error);

            return result;
        }

        private async Task<Result<HttpResponseMessage>> PostCustomer(CustomerToWrite customer)
        {
            try
            {
                return Result.Success(await httpClient.PostAsJsonAsync(UriSegment, customer));
            }
            catch (Exception)
            {
                return Result.Failure<HttpResponseMessage>("Failed to add customer");
            }
        }

        private async Task<Result<PostResponse>> ReadPostResult(HttpResponseMessage response)
        {
            try
            {
                var data = await response.Content.ReadFromJsonAsync<PostResponse>();
                return data is not null
                    ? Result.Success(data)
                    : Result.Failure<PostResponse>("Empty result");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to read the post result.");
                return Result.Failure<PostResponse>("Failed to read the post result.");
            }
        }

        public async Task<Result<IReadOnlyList<CustomerToReadInList>>> GetAllCustomers()
        {
            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<CustomerToReadInList>>($"{UriSegment}/list");
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

        public async Task<Result> UpdateCustomer(CustomerToWrite customer)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"{UriSegment}/{customer.Id}", customer);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = response.Content.ReadAsStringAsync().Result;
                    logger.LogError(message: errorMessage);
                    return Result.Failure<CustomerToRead>(errorMessage);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                var errorMessage = "Failed to update customer with id {customer.Id}";
                logger.LogError(ex, errorMessage);
                return Result.Failure<CustomerToRead>(errorMessage);
            }
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