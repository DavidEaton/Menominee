using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Common.Http;
using Menominee.Shared.Models.SaleCodes;
using System.Net.Http.Json;

namespace Menominee.Client.Services.SaleCodes
{
    public class SaleCodeDataService : ISaleCodeDataService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<SaleCodeDataService> logger;
        private const string UriSegment = "api/salecodes";

        public SaleCodeDataService(HttpClient httpClient, ILogger<SaleCodeDataService> logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<PostResponse>> AddAsync(SaleCodeToWrite saleCode)
        {
            var result = await httpClient.AddAsync(
            UriSegment,
            saleCode,
            logger);

            if (result.IsFailure)
                logger.LogError(result.Error);

            return result;
        }

        public async Task<IReadOnlyList<SaleCodeToReadInList>> GetAllAsync()
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

        public async Task<IReadOnlyList<SaleCodeShopSuppliesToReadInList>> GetAllShopSuppliesAsync()
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

        public async Task<SaleCodeToRead> GetAsync(long id)
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

        public async Task<Result> UpdateAsync(SaleCodeToWrite fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                logger,
                saleCode => $"{saleCode.ToString}",
                saleCode => saleCode.Id);
        }
    }
}
