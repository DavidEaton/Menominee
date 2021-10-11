using Menominee.Common.Entities;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public interface ITenantDataService
    {
        Task<Tenant> GetTenantAsync();
    }
}
