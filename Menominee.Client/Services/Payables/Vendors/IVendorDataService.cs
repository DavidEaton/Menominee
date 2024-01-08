using CSharpFunctionalExtensions;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Payables.Vendors;

namespace Menominee.Client.Services.Payables.Vendors
{
    public interface IVendorDataService
    {
        Task<Result<IReadOnlyList<VendorToRead>>> GetAllAsync();
        Task<Result<VendorToRead>> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(VendorToWrite vendor);
        Task<Result> UpdateAsync(VendorToWrite vendor);
    }
}
