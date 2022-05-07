using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Payables.Vendors
{
    public interface IVendorDataService
    {
        Task<IReadOnlyList<VendorToReadInList>> GetAllVendors();
        Task<VendorToRead> GetVendor(long id);
        Task<VendorToRead> AddVendor(VendorToWrite vendor);
        Task UpdateVendor(VendorToWrite vendor, long id);
    }
}
