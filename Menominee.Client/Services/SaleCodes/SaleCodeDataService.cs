using Menominee.Shared.Models.SaleCodes;
using System.Net.Http.Json;
using System.Text.Json;
using CSharpFunctionalExtensions;

namespace Menominee.Client.Services.SaleCodes
{
    public class SaleCodeDataService : ISaleCodeDataService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<SaleCodeDataService> logger;
        private const string UriSegment = "api/salecodes";

        public SaleCodeDataService(HttpClient httpClient, ILogger<SaleCodeDataService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<Result<SaleCodeToRead>> AddSaleCodeAsync(SaleCodeToWrite saleCode)
        {
            var response = await httpClient.PostAsJsonAsync(UriSegment, saleCode);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result;
                logger.LogError(message: errorMessage);
                return Result.Failure<SaleCodeToRead>(errorMessage);
            }

            var data = await JsonSerializer.DeserializeAsync<SaleCodeToRead>(await response.Content.ReadAsStreamAsync());

            return Result.Success(data!);
        }

        public async Task<IReadOnlyList<SaleCodeToReadInList>> GetAllSaleCodesAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<SaleCodeToReadInList>>($"{UriSegment}/listing");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get all sale codes");
            }

            return null;
        }

        public async Task<IReadOnlyList<SaleCodeShopSuppliesToReadInList>> GetAllSaleCodeShopSuppliesAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<SaleCodeShopSuppliesToReadInList>>($"{UriSegment}/shopsupplieslist");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get all shop supplies");
            }

            return null;
        }

        public async Task<SaleCodeToRead> GetSaleCodeAsync(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<SaleCodeToRead>($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get sale code with id {id}", id);
            }

            return null;
        }

        public async Task<Result> UpdateSaleCodeAsync(SaleCodeToWrite saleCode, long id)
        {
            var response = await httpClient.PutAsJsonAsync($"{UriSegment}/{id}", saleCode);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result;
                logger.LogError(message: errorMessage);
                return Result.Failure(errorMessage);
            }

            return Result.Success();
        }
    }
}
