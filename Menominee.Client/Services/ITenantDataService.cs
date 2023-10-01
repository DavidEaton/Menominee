using CSharpFunctionalExtensions;
using Menominee.Common.Entities;

namespace Menominee.Client.Services
{
    public interface ITenantDataService
    {
        Task<Result<Tenant>> GetAsync();
    }
}
