using SharedKernel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientApp.Services
{
    public class TenantDataService : ITenantDataService
    {
        private readonly HttpClient httpClient;

        public TenantDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Tenant> GetTenantAsync()
        {
            return await JsonSerializer.DeserializeAsync<Tenant>
                (await httpClient.GetStreamAsync($"tenants/"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
