using SharedKernel.Entities;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public interface ITenantDataService
    {
        Task<Tenant> GetTenantAsync();
    }
}
