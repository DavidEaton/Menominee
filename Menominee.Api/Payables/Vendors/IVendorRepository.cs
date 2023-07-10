using Menominee.Domain.Entities.Payables;
using Menominee.Shared.Models.Payables.Vendors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Payables.Vendors
{
    public interface IVendorRepository
    {
        Task AddVendorAsync(Vendor entity);
        Task<IReadOnlyList<VendorToRead>> GetVendorsAsync();
        Task<VendorToRead> GetVendorAsync(long id);
        void DeleteVendor(Vendor entity);
        Task<bool> VendorExistsAsync(long id);
        Task SaveChangesAsync();
        Task<Vendor> GetVendorEntityAsync(long id);
        Task<IReadOnlyList<Vendor>> GetVendorEntitiesAsync(List<long> ids);
    }
}
