using CSharpFunctionalExtensions;
using Menominee.Domain.Entities;

namespace Menominee.Client.Services
{
    public interface ITenantDataService
    {
        Task<Result<Tenant>> GetAsync();
    }
}
