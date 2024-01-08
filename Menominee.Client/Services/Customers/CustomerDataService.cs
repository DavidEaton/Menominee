using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.Http;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Customers
{
    public class CustomerDataService : DataServiceBase<CustomerDataService>, ICustomerDataService
    {
        private readonly HttpClient httpClient;
        private const string UriSegment = "api/customers";

        public CustomerDataService(HttpClient httpClient,
            ILogger<CustomerDataService> logger,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }


        public async Task<Result<PostResponse>> AddAsync(CustomerToWrite fromCaller)
        {
            var entityType = "Customer";
            try
            {
                var result = await httpClient.AddAsync(
                    UriSegment,
                    fromCaller,
                    Logger);

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to add {entityType}");
                return Result.Failure<PostResponse>("An unexpected error occurred");
            }
        }

        public async Task<Result<IReadOnlyList<CustomerToReadInList>>> GetAllAsync()
        {
            var errorMessage = "Failed to get all customers";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<CustomerToReadInList>>($"{UriSegment}/list");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<CustomerToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<CustomerToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<CustomerToRead>> GetAsync(long id)
        {
            var errorMessage = $"Failed to get customer with id {id}";

            try
            {
                var result = await httpClient.GetFromJsonAsync<CustomerToRead>(UriSegment + $"/{id}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<CustomerToRead>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<CustomerToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateAsync(CustomerToWrite customerFromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                customerFromCaller,
                Logger,
                customer => $"{customer.ToString}",
                customer => customer.Id);
        }

        public async Task<Result> DeleteAsync(long id)
        {
            var failureMessage = "Failed to delete customer";

            try
            {
                var response = await httpClient.DeleteAsync($"{UriSegment}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Logger.LogError(message: errorMessage);
                    return Result.Failure(failureMessage);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, failureMessage);
                return Result.Failure(failureMessage);
            }
        }
    }
}