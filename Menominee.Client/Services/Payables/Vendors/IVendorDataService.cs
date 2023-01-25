using CustomerVehicleManagement.Shared.Models;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Payables.Vendors
{
    public interface IVendorDataService
    {
        Task<IReadOnlyList<VendorToRead>> GetAllVendorsAsync();
        Task<VendorToRead> GetVendorAsync(long id);
        Task<PostResult> AddVendorAsync(VendorToWrite vendor);
        Task UpdateVendorAsync(VendorToWrite vendor, long id);
    }
}
