using SharedKernel;
using System;
using System.Threading.Tasks;

namespace ClientApp.Services
{
    public interface ITenantDataService
    {
        Task<Tenant> GetTenantAsync();
    }
}
