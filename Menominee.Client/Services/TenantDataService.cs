using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Common.Entities;
using System.Net.Http.Json;

namespace Menominee.Client.Services
{
    public class TenantDataService : DataServiceBase<TenantDataService>, ITenantDataService
    {
        private readonly HttpClient httpClient;
        private const string UriSegment = "api/tenants";

        public TenantDataService(HttpClient httpClient,
            ILogger<TenantDataService> logger,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<Result<Tenant>> GetAsync()
        {
            var errorMessage = "Failed to get all tenants";

            try
            {
                var result = await httpClient.GetFromJsonAsync<Result<Tenant>>(UriSegment);
                return result.Value is not null
                    ? Result.Success(result.Value)
                    : Result.Failure<Tenant>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<Tenant>(errorMessage);
            }
        }
    }
}
