using Menominee.Common.Http;
using Menominee.Shared.Models.Payables.Vendors;

namespace Menominee.Client.Services.Payables.Vendors
{
    public interface IVendorDataService
    {
        Task<IReadOnlyList<VendorToRead>> GetAllVendorsAsync();
        Task<VendorToRead> GetVendorAsync(long id);
        Task<PostResponse> AddVendorAsync(VendorToWrite vendor);
        Task UpdateVendorAsync(VendorToWrite vendor, long id);
    }
}
