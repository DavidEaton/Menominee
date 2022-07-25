using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services.Payables.Vendors
{
    public interface IVendorDataService
    {
        Task<IReadOnlyList<VendorToReadInList>> GetAllVendorsAsync();
        Task<VendorToRead> GetVendorAsync(long id);
        Task<VendorToRead> AddVendorAsync(VendorToWrite vendor);
        Task UpdateVendorAsync(VendorToWrite vendor, long id);
    }
}
