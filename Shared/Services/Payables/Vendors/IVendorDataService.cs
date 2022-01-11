using MenomineePlayWASM.Shared.Dtos.Payables.Vendors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Services.Payables.Vendors
{
    public interface IVendorDataService
    {
        Task<IReadOnlyList<VendorToReadInList>> GetAllVendors();
        Task<VendorToRead> GetVendor(long id);
        Task<VendorToRead> AddVendor(VendorToWrite vendor);
        Task UpdateVendor(VendorToWrite vendor, long id);
    }
}
