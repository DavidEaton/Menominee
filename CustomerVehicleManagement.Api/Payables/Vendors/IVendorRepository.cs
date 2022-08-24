using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.Vendors
{
    public interface IVendorRepository
    {
        Task AddVendorAsync(Vendor entity);
        Task<IReadOnlyList<VendorToRead>> GetVendorsAsync();
        Task<VendorToRead> GetVendorAsync(long id);
        Task<IReadOnlyList<VendorToReadInList>> GetVendorsListAsync();
        void DeleteVendor(Vendor entity);
        void FixTrackingState();
        Task<bool> VendorExistsAsync(long id);
        Task SaveChangesAsync();
        Task<Vendor> GetVendorEntityAsync(long id);
    }
}
