using CustomerVehicleManagement.Shared.Models.SaleCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Services.SaleCodes
{
    public interface ISaleCodeDataService
    {
        Task<IReadOnlyList<SaleCodeToReadInList>> GetAllSaleCodesAsync();
        Task<SaleCodeToRead> GetSaleCodeAsync(long id);
        Task<SaleCodeToRead> AddSaleCodeAsync(SaleCodeToWrite saleCode);
        Task UpdateSaleCodeAsync(SaleCodeToWrite saleCode, long id);
    }
}
