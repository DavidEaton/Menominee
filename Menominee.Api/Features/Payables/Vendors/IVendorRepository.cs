using CSharpFunctionalExtensions;
using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Vendors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Payables.Vendors
{
    public interface IVendorRepository
    {
        void Add(Vendor entity);
        void Delete(Vendor entity);
        Task<IReadOnlyList<VendorToRead>> GetAllAsync();
        Task<VendorToRead> GetAsync(long id);
        Task SaveChangesAsync();
        Task<Result<Vendor>> GetEntityAsync(long id);
        Task<IReadOnlyList<Vendor>> GetEntitiesAsync(List<long> ids);
    }
}