using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Payables.Vendors
{
    public interface IVendorRepository
    {
        Task CreateVendorAsync(Vendor vendor);
        Task<Vendor> GetVendorEntityAsync(long id);
        Task<VendorToRead> GetVendorAsync(long id);
        Task<IReadOnlyList<VendorToRead>> GetVendorsAsync();
        Task<IReadOnlyList<VendorToReadInList>> GetVendorsListAsync();
        void UpdateVendorAsync(Vendor vendor);
        Task DeleteVendorAsync(long id);
        Task<bool> VendorExistsAsync(long id);
        Task SaveChangesAsync();
        void FixTrackingState();
    }
}
