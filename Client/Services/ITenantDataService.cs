using SharedKernel;
using System;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface ITenantDataService
    {
        Task<Tenant> GetTenantAsync();
    }
}
