using MenomineePlayWASM.Shared.Dtos.Payables.Vendors;
using MenomineePlayWASM.Shared.Entities.Payables.Vendors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Repository.Payables
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
